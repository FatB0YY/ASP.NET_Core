using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RR_hookah
{
    public class Program
    {
        /*  .NET Core в самом начале запускаетс€ как консольное приложение,
         *  Main метод настраивает ASP.NET Core и запускает его.
        *  ¬ методе вызываетс€ конструктор дл€ создани€ хоста, он в классе Program,
         *  который ретурн объект типа IHostBuilder, в нем запускаетс€ и собираетс€ наш код.
         *  ƒалее оно начинает работать как ASP.NET Core.
         */  
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // настраиваетс€ с помощью Startup файла
                    // разбор в MyFile.txt
                    webBuilder.UseStartup<Startup>();
                });
    }
}
