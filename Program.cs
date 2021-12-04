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
        /*  .NET Core � ����� ������ ����������� ��� ���������� ����������,
         *  Main ����� ����������� ASP.NET Core � ��������� ���.
        *  � ������ ���������� ����������� ��� �������� �����, �� � ������ Program,
         *  ������� ������ ������ ���� IHostBuilder, � ��� ����������� � ���������� ��� ���.
         *  ����� ��� �������� �������� ��� ASP.NET Core.
         */  
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // ������������� � ������� Startup �����
                    // ������ � MyFile.txt
                    webBuilder.UseStartup<Startup>();
                });
    }
}
