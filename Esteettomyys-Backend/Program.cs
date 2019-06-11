using System;
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
			var assembly = AppDomain.CurrentDomain.GetAssemblies()
			.Single(o => o.EntryPoint != null);

			var config = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("hosting.json", optional: true);

			config.AddUserSecrets(assembly, optional: false);

			var host = new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(config.Build())
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}

	}
}
