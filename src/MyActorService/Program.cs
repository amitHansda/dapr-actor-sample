using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dapr.Actors;
using Dapr.Actors.AspNetCore;

namespace MyActorService
{
    public class Program
    {
        private const int AppChannelHttpPort = 3000;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseActors(actorRuntime=>{
                        actorRuntime.RegisterActor<MyActor>();
                    })
                    .UseUrls($"http://localhost:{AppChannelHttpPort}/");
                });
    }
}
