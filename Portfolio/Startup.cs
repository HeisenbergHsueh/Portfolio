using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//�s��DB
using Portfolio.Data;
using Microsoft.EntityFrameworkCore;

//cookie�{�ҡB���v
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Portfolio
{
    public class Startup
    {
        //Startup �غc�l
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            #region DB�s�u�r��
            //�z�L DbContextOptions ����W����k�A�ӳs�� appsetting.json ���A�]�w�n�� SQL server connection string
            services.AddDbContext<PortfolioContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("HeisenbergHsueh_Portfolio_DB")));
            #endregion

            #region Cookie����
            //���Ucookie���Ҫ��A��
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                //�]�wcookie�W��
                options.Cookie.Name = "HeisenbergHsueh_Cookie";
                //browser �|���� cookie �u��g�� http protocol �Ӧs��
                options.Cookie.HttpOnly = true;
                //�]�w�n�J�����A���n�J�ɦ۰ʾɤJ��n�J����
                options.LoginPath = new PathString("/LoginSystem/Login");
                //�]�w�n�X����(�������i�H�ٲ�)
                options.LogoutPath = new PathString("/LoginSystem/Logout");
                //��n�J�̤��㦳��U�������X���v���ɡA�|�۰�����즹Forbidden����
                options.AccessDeniedPath = new PathString("/LoginSystem/Forbidden");
                //�]�w�n�J���Įɶ�(���:����)
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //SlidingExpiration = true�A��ܻ��A�p�Gcookie��e���ɶ��w�g�W�L�}�l�ɶ��P�����ɶ����@�b�A�h���s�{�ocookie
                //options.SlidingExpiration = true;
            });
            #endregion

            #region ���v
            services.AddAuthorization(options => {
                //��h���v               
                options.AddPolicy("IsIT", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "IT");
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();   //�]�w�ϥ��R�A�ɮ�
            app.UseRouting();

            #region cookie����
            //���`�N UseAuthentication() && UseAuthorization() �����Ǥ��i�H�A��
            //�ҥ� cookie ��h
            app.UseCookiePolicy();
            //�ҥ�����
            app.UseAuthentication();
            //�ҥα��v
            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
