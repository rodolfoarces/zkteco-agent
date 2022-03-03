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
		[Option('f', "file", Required = false, HelpText = "Device list (JSON format)", Default = null)] public string JOSNFile { get; set; }
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
			if (string.IsNullOrEmpty(opts.JOSNFile))
            {
				/* No JSON file was given, trying with the rest of the parsed information */
				ProgramLoggger.Info("No JSON file was given, trying with the rest of the parsed information");
				ConnectionDevice device = new ConnectionDevice(0,opts.Host,opts.Port,opts.Password);

            }
			else
            {
				ProgramLoggger.Debug("Reading file provided");
				if (File.Exists(opts.JOSNFile))
				{
					using (StreamReader r = new StreamReader(opts.JOSNFile))
					{
						string json = r.ReadToEnd();
						ProgramLoggger.Debug("Deserializing JSON file");
						List<ConnectionDevice> devices = JSON.Deserialize<List<ConnectionDevice>>(json);
						foreach (ConnectionDevice device in devices)
                        {
							ProgramLoggger.Debug(device.ToString());
						}
						
						
						List<ZKTekoDevice> zkdevices = new List<ZKTekoDevice>();


						foreach (ConnectionDevice dev in devices)
						{
							ProgramLoggger.Debug("Adding devices to connect");
							zkdevices.Add(new ZKTekoDevice(dev));
                        }

						foreach (ZKTekoDevice zdev in zkdevices)
                        {
							ProgramLoggger.Debug("Connecting to device");
							zdev.ObtainAttendance();
							

						}

						ProgramLoggger.Debug(JSON.Serialize(zkdevices));				}
				}
				else
				{
					ProgramLoggger.Error("The file path given doesn't exists or you don't have permissions to access it");
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
