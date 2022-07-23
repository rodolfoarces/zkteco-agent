using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using log4net;
using CommandLine;
using Jil;
using zkteco_cli.API;
using zkteco_cli.Connections;
using zkteco_cli.ZKTeco;
using System.Net;
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

		static async void RunOptions(Program opts)
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
						if (ProgramLoggger.IsDebugEnabled)
                        {
							ProgramLoggger.Debug("Deserializing JSON file");
						}

						/* Obtain list of devices to connect */
						List<ConnectionDevice> devices = JSON.Deserialize<List<ConnectionDevice>>(json);

						if (ProgramLoggger.IsDebugEnabled)
						{
							/* Show all information of devices */
							foreach (ConnectionDevice device in devices)
							{
								ProgramLoggger.Debug(device.ToString());
							}
						}

						/* Create list of devices with which we'll work */
						List<ZKTecoDevice> zkdevices = new List<ZKTecoDevice>();

						foreach (ConnectionDevice dev in devices)
						{
							if (ProgramLoggger.IsDebugEnabled)
                            {
								ProgramLoggger.Debug("Adding device to list of devices to connect to");
							}
							zkdevices.Add(new ZKTecoDevice(dev));
						}

						foreach (ZKTecoDevice zdev in zkdevices)
						{
							if (ProgramLoggger.IsDebugEnabled)
                            {
								ProgramLoggger.Debug("Connecting to device" + zdev.ToString());
							}
								
							if (ProgramLoggger.IsDebugEnabled)
                            {
								ProgramLoggger.Debug("Obtaning attendance");
							}
								
							zdev.ObtainAttendance();

							if (ProgramLoggger.IsDebugEnabled)
                            {
								ProgramLoggger.Debug("Obtaning users");
							}
								
							zdev.ObtainUsers();

						}

						// Information to send to endpoints
						// JSON.Serialize(zkdevices)

						// Print information to send
						ProgramLoggger.Info(JSON.Serialize(zkdevices));
						// Endpoints to send information
						if (string.IsNullOrEmpty(opts.JSONEndpointsFile))
						{
							/* No JSON file was given, trying with the rest of the parsed information */
							ProgramLoggger.Info("No JSON file was given, trying with the rest of the parsed information");
							Environment.Exit(1);

						}
						else
						{
							ProgramLoggger.Debug("Reading file provided");
							if (File.Exists(opts.JSONEndpointsFile))
							{
								using (StreamReader e = new StreamReader(opts.JSONEndpointsFile))
								{
									string ep_json = e.ReadToEnd();
									ProgramLoggger.Debug("Deserializing JSON Endpoints file");
									List<ConnectionEndpoint> endpoints = JSON.Deserialize<List<ConnectionEndpoint>>(ep_json);
									
									if (ProgramLoggger.IsDebugEnabled)
									{
										/* Show all information of endpoints */
										foreach (ConnectionEndpoint endpoint in endpoints)
										{
											ProgramLoggger.Debug(endpoint.ToString());
										}
									}

									// Calls connection to endpoints to send data
									await SendDataToEndpoints(endpoints, zkdevices);

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
		static async Task SendDataToEndpoints(List<ConnectionEndpoint> endpoints,List<ZKTecoDevice> devices)
        {
			List<ApiEndpoint> api_endpoints = new List<ApiEndpoint>();

			foreach (ConnectionEndpoint endpoint in endpoints)
            {
				api_endpoints.Add(new ApiEndpoint(endpoint));
            }

			foreach (ApiEndpoint endpoint in api_endpoints)
			{
				/* Initial connection to authenticate and obtain token */
				/* Set the HTTP client connection */
				using (HttpClient client = new HttpClient())
				{

					try
					{
						/* Assamble the initial connection */
						client.DefaultRequestHeaders.Accept.Clear();
						client.DefaultRequestHeaders.Accept.Add(
							new MediaTypeWithQualityHeaderValue("multipart/form-data"));
						client.DefaultRequestHeaders.Add("User-Agent", "api-client");
						var formContent = new FormUrlEncodedContent(new[]
						{
						new KeyValuePair<string, string>("username", endpoint.GetUsername()),
						new KeyValuePair<string, string>("password", endpoint.GetPassword()),
						new KeyValuePair<string, string>("application", endpoint.GetApplication()),
						});

						/* Making the connection and sending data*/
						var stringTask = client.PostAsync(endpoint.GetLoginURL(), formContent);

						/* Get response data */
						var response = await stringTask;
						var stringContent = await response.Content.ReadAsStringAsync();
						if (ProgramLoggger.IsDebugEnabled)
						{
							ProgramLoggger.Debug(response.ToString());
							ProgramLoggger.Debug(stringContent);
						}

						/* Convert response content to usable information */
						endpoint.SetApiResponse(JSON.Deserialize<ApiResponse>(stringContent));
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
				/* Send information */
				using (HttpClient client = new HttpClient())
				{

					try
					{
						/* Assamble the initial connection */
						client.DefaultRequestHeaders.Accept.Clear();
						client.DefaultRequestHeaders.Accept.Add(
							new MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Add("User-Agent", "api-client");
						client.DefaultRequestHeaders.Add("Authorization", "Bearer " + endpoint.GetApiResponse().GetData().GetAccessToken());
						client.DefaultRequestHeaders.Add("Cookie", "refreshToken=" + endpoint.GetApiResponse().GetData().GetRefreshToken());

						var httpContent = new StringContent(JSON.Serialize(devices), Encoding.UTF8, "application/json");

						var stringTask = client.PostAsync(endpoint.GetUploadURL(), httpContent);

						/* Get response data */
						var response = await stringTask;
						var stringContent = await response.Content.ReadAsStringAsync();
						if (ProgramLoggger.IsDebugEnabled)
						{
							ProgramLoggger.Debug(response.ToString());
							ProgramLoggger.Debug(stringContent);
						}

						/* Convert response content to usable information */
						endpoint.SetApiResponse(JSON.Deserialize<ApiResponse>(stringContent));

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
}