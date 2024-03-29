using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RR_hookah.Data;
using RR_hookah.Utility;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace RR_hookah
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // MyFile.txt
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // �����������
            services.AddLocalization(opt => { opt.ResourcesPath = "Recourses";  });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();


            // ��� �����
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpContextAccessor();
            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // ����������� ������ ����������
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // ������������ ������� �� ���������� ����������.
            app.UseHttpsRedirection();
            // ���� ����� (js/ts/jsx, img, scss/css ���...)
            app.UseStaticFiles();
            // �������, �� ��������
            app.UseRouting();

            // ��������������
            app.UseAuthentication();

            // �����������
            app.UseAuthorization();

            // middleware ��� ��������� ������
            app.UseSession();


            // �����������
            var supportedCultres = new[] { "ru", "en" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultres[0]).AddSupportedCultures(supportedCultres).AddSupportedUICultures(supportedCultres);
            app.UseRequestLocalization(localizationOptions);

            // ������� ����� �������� 
            app.UseEndpoints(endpoints =>
            {
                // ��� razor �������
                endpoints.MapRazorPages();
                // ������� �� ��������� MVC
                // ������ ����� ��� ���� razer ���
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
