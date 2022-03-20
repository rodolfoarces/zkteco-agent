using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using log4net;
using CommandLine;
using Jil;
using zkteco_cli.Components;

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
				ConnectionDevice device = new ConnectionDevice(0,opts.Host,opts.Port,opts.Password);

            }
			else
            {
				ProgramLoggger.Debug("Reading file provided");
				if (File.Exists(opts.JSONDevicesFile))
				{
					using (StreamReader r = new StreamReader(opts.JSONDevicesFile))
					{
						string json = r.ReadToEnd();
						ProgramLoggger.Debug("Deserializing JSON file");
						List<ConnectionDevice> devices = JSON.Deserialize<List<ConnectionDevice>>(json);
						foreach (ConnectionDevice device in devices)
                        {
							ProgramLoggger.Debug(device.ToString());
						}
						
						
						List<ZKTecoDevice> zkdevices = new List<ZKTecoDevice>();


						foreach (ConnectionDevice dev in devices)
						{
							ProgramLoggger.Debug("Adding devices to connect");
							zkdevices.Add(new ZKTecoDevice(dev));
                        }

						foreach (ZKTecoDevice zdev in zkdevices)
                        {
							ProgramLoggger.Debug("Connecting to device");
							zdev.ObtainAttendance();
							zdev.ObtainUsers();
							

						}

						ProgramLoggger.Info(JSON.Serialize(zkdevices));				}
				}
				else
				{
					ProgramLoggger.Error("The file path given for devices doesn't exists or you don't have permissions to access it");
				}
				// Endpoints to send information
				if (string.IsNullOrEmpty(opts.JSONEndpointsFile))
				{
					/* No JSON file was given, trying with the rest of the parsed information */
					ProgramLoggger.Info("No JSON file was given, trying with the rest of the parsed information");

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
							foreach (ConnectionEndpoint  endpoint in endpoints)
							{
								ProgramLoggger.Debug(endpoint.ToString());
							}
						}
					}
					else
                    {
						ProgramLoggger.Error("The file path given for endpoints doesn't exists or you don't have permissions to access it");
					}
					
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
	}
}
