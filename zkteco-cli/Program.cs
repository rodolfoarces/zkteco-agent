using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using CommandLine;
using System.Text.Json;
using zkteco_cli.API;
using zkteco_cli.Connections;
using zkteco_cli.ZKTeco;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace zkteco_cli
{
	class Program
	{
		[Option('P', "port", Required = false, HelpText = "Connection port (Default:4370)", Default = 4370)] public int Port { get; set; }
		[Option('p', "password", Required = false, HelpText = "Connection password (Default:0)", Default = 0)] public int Password { get; set; }
		[Option('h', "host", Required = false, HelpText = "Connection IP (Default:192.168.1.201)", Default = "192.168.1.201")] public string Host { get; set; }
		[Option('f', "file", Required = false, HelpText = "Device list (JSON format)", Default = null)] public string JSONDevicesFile { get; set; }
		[Option('e', "endpoint", Required = false, HelpText = "Endpoint list (JSON format)", Default = null)] public string JSONEndpointsFile { get; set; }
		[Option('t', "sleep-time", Required = false, HelpText = "Sleep time between device connection (in minutes)", Default = 1)] public int SleepTime { get; set; }

		/* Set the logger */
		private static readonly ILog ProgramLoggger = LogManager.GetLogger(typeof(Program));

        /* Main programm it calls RunOptions() if everything is OK, and calls HandleParseErrors() if something goes wrong */
        static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<Program>(args).WithParsed(RunOptions).WithNotParsed(HandleParseError);
		}

		static void RunOptions(Program opts)
        {
			if (string.IsNullOrEmpty(opts.JSONDevicesFile))
			{
				/* No JSON file was given, trying with the rest of the parsed information */
				ProgramLoggger.Info("No JSON file was given, trying with the rest of the parsed information");
				Environment.Exit(1);

			}
			else
			{
				ProgramLoggger.Debug("Reading file provided");
				if (File.Exists(opts.JSONDevicesFile))
				{
					using (StreamReader r = new StreamReader(opts.JSONDevicesFile))
					{

						string json = r.ReadToEnd();
                        ProgramLoggger.Debug("Deserializing JSON file" + json);

                        /* Obtain list of devices to connect */
                        List<ConnectionDevice> devices = JsonSerializer.Deserialize< List<ConnectionDevice>>(json);

						/* Show all information of devices */
						foreach (ConnectionDevice device in devices)
						{
							ProgramLoggger.Debug(device.ToString());
						}

						/* Create list of devices with which we'll work */
						List<ZKTecoDevice> zkdevices = new List<ZKTecoDevice>();

						foreach (ConnectionDevice dev in devices)
						{
							ProgramLoggger.Debug("Adding device to list of devices to connect to");
							zkdevices.Add(new ZKTecoDevice(dev));
						}

						foreach (ZKTecoDevice zdev in zkdevices)
						{

							ProgramLoggger.Debug("Connecting to device" + zdev.ToString());
                            
							ProgramLoggger.Debug("Obtaning attendance");
                            zdev.ObtainAttendance();
                            
							ProgramLoggger.Debug("Obtaning users");
                            zdev.ObtainUsers();

						}

                        // Print information to send
                        ProgramLoggger.Info("JSON content to send: " + JsonSerializer.Serialize(zkdevices));
                        // Endpoints to send information
                        if (string.IsNullOrEmpty(opts.JSONEndpointsFile))
						{
							/* No JSON file was given, trying with the rest of the parsed information */
							ProgramLoggger.Info("No JSON file was given, trying with the rest of the parsed information");
							Environment.Exit(1);

						}
						else
						{
							ProgramLoggger.Debug("Reading file provided" + opts.JSONEndpointsFile);
							if (File.Exists(opts.JSONEndpointsFile))
							{
								using (StreamReader e = new StreamReader(opts.JSONEndpointsFile))
								{
									string ep_json = e.ReadToEnd();
									ProgramLoggger.Debug("Deserializing JSON Endpoints file: " + ep_json);
									try
									{
                                        List<ApiEndpoint> endpoints = JsonSerializer.Deserialize<List<ApiEndpoint>>(ep_json);
										
										/* Show all information of endpoints */
										foreach (ApiEndpoint endpoint in endpoints)
										{
											ProgramLoggger.Debug(endpoint.ToString());
                                            Task.Run(async () => await SendDataToEndpoints(endpoint, zkdevices)).Wait();
                                        }
                                    }
									catch (Exception ex)
									{
										ProgramLoggger.Error(ex.ToString());
									}
								}
							}
							else
							{
								ProgramLoggger.Error("The file path given for endpoints doesn't exists or you don't have permissions to access it");
								Environment.Exit(1);
							}

						}
						
					}
					
				}
				else
				{
					ProgramLoggger.Error("The file path given for devices doesn't exists or you don't have permissions to access it");
					Environment.Exit(1);
				}
			}
		}
		static void HandleParseError(IEnumerable<Error> errs)
		{
			
			foreach (Error error in errs)
            {
				ProgramLoggger.Error(error.ToString());
            }
		}
		static async Task SendDataToEndpoints(ApiEndpoint endpoint,List<ZKTecoDevice> devices)
        {
			/* Initial connection to authenticate and obtain token */
			string access_token;
			string refreshToken;
			/* Set the HTTP client connection */

			using (HttpClient client = new HttpClient())
			{
				ProgramLoggger.Info("Initiating connection");
				/* Initiate connection and obtain token */
				try
				{
                    
					// Assamble the initial connection
					ProgramLoggger.Debug("Assembling connection");
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(
						new MediaTypeWithQualityHeaderValue("multipart/form-data"));
					client.DefaultRequestHeaders.Add("User-Agent", "api-client");
					var formContent = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("username", endpoint.GetUsername().ToString()),
					new KeyValuePair<string, string>("password", endpoint.GetPassword()),
					new KeyValuePair<string, string>("application", endpoint.GetApplication()),
					});

					// Making the connection and sending data
					ProgramLoggger.Debug("Sending information");
					var stringTask = client.PostAsync(endpoint.GetLoginURL(), formContent);

					// Get response data
					ProgramLoggger.Debug("Processing response");
					var response = await stringTask;
					var stringContent = await response.Content.ReadAsStringAsync();

					ProgramLoggger.Debug(" First connection response: " + stringContent.ToString());

					/* Convert response content to usable information */
					ApiResponse api_response = JsonSerializer.Deserialize<ApiResponse>(stringContent);
					access_token = api_response.GetData().GetAccessToken();
					refreshToken = api_response.GetData().GetRefreshToken();

					ProgramLoggger.Debug(api_response.ToString());

                    /* Send information */
                    using (HttpClient send_client = new HttpClient())
                    {

                        try
                        {
                            ProgramLoggger.Debug("Initiating second connection to send information");
                            // Assamble the initial connection
                            send_client.DefaultRequestHeaders.Accept.Clear();
                            send_client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
                            send_client.DefaultRequestHeaders.Add("User-Agent", "api-client");
							send_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                            ProgramLoggger.Debug(JsonSerializer.Serialize(devices));
                            var send_httpContent = new StringContent(JsonSerializer.Serialize(devices), System.Text.Encoding.UTF8, "application/json");

                            var send_stringTask = send_client.PostAsync(endpoint.GetUploadURL(), send_httpContent);
                            ProgramLoggger.Debug("Request headers for second connection: " + send_client.DefaultRequestHeaders.ToString());
                            ProgramLoggger.Debug("Content to send: " + send_httpContent.ToString());
                            
                            // Get response data
                            var send_response = await send_stringTask;
                            var send_stringContent = await send_response.Content.ReadAsStringAsync();
                            
							ProgramLoggger.Info("Information sent");
                            ProgramLoggger.Debug(send_response.ToString());
                            ProgramLoggger.Debug(send_stringContent.ToString());

                            // Convert response content to usable information
                            ApiResponse api_send_response = JsonSerializer.Deserialize<ApiResponse>(send_stringContent);
                            ProgramLoggger.Debug(api_send_response.ToString());
                        }
                        catch (System.Net.Http.HttpRequestException ex)
                        {
                            ProgramLoggger.Error(ex.Message);
                        }
                        finally
                        {
                            send_client.Dispose();
                        }
                    }

                }
                catch (System.Net.Http.HttpRequestException ex)
				{
					ProgramLoggger.Error(ex.Message);
				}
				finally
				{
					client.Dispose();
				}
            }
		}
    }
}