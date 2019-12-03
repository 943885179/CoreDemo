﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreDemo_Logging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
           /* .ConfigureLogging((context, logger) =>{
                logger.AddFilter("System", LogLevel.Warning);
                logger.AddFilter("Microsoft", LogLevel.Warning);
                logger.AddLog4Net();
            })*/
            .ConfigureLogging(option=> {
                option.ClearProviders();
                option.AddConsole();
            });
    }
}
