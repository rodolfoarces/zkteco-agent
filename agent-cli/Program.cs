using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using CommandLine;
using agent_cli.Components;

namespace agent_cli
{
	class Program
	{
		[Option('P', "port", Required = false, HelpText = "Connection port (Default:4370)", Default = 4370)] public int Port { get; set; }
		[Option('p', "password", Required = false, HelpText = "Connection password (Default:0)", Default = 0)] public int Password { get; set; }
		[Option('h', "host", Required = false, HelpText = "Connection IP (Default:192.168.1.201)", Default = "192.168.1.201")] public string Host { get; set; }

		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<Program>(args).WithParsed(RunOptions).WithNotParsed(HandleParseError);
		}

		static void RunOptions(Program opts)
        {
			Device device = new Device();
			device.Id = 1;
			device.Name = "Test";
			device.UID = "aa11";
			device.MAC = "00:17:61:12:C0:F0";

			log.Debug("Setting IP");
			if (opts.Host != "192.168.1.201")
			{
				log.Debug("Host set to: " + opts.Host);
				device.Host = opts.Host;
			}

			log.Debug("Setting port");
			if (opts.Port != 4370)
			{
				log.Debug("Port set to: " + opts.Port.ToString());
				device.Port = opts.Port;
			}

			log.Debug("Setting password");
			if (opts.Port != 0)
			{
				log.Debug("Password set to: " + opts.Password.ToString());
				device.Password = opts.Password;
			}
			

			log.Info(device.ToString());

			string serial = device.GetSerial();

			log.Info(serial);

		}
		static void HandleParseError(IEnumerable<Error> errs)
		{
			
			foreach (Error error in errs)
            {
				log.Error(error.ToString());
            }
		}
	}
}
