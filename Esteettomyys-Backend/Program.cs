﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Esteettomyys_Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var isDevelopment = environment == EnvironmentName.Development;

			var assembly = AppDomain.CurrentDomain.GetAssemblies()
			.Single(o => o.EntryPoint != null);

			var config = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("hosting.json", optional: true);

			if (isDevelopment) { 
				config.AddUserSecrets(assembly, optional: false);
			}

			var host = new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(config.Build())
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureLogging((logging) =>
				{
					logging.AddConsole();
					logging.AddDebug();
					logging.AddEventSourceLogger();
				})
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}

	}
}
