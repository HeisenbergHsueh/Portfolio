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
//�[�ѱK
using Portfolio.Security;
//�s��DB
using Portfolio.Data;
using Microsoft.EntityFrameworkCore;
//cookie�{�ҡB���v
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
//�h��y�t
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Authentication;

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
            string CipherText = Configuration.GetConnectionString("SQL_ConnectionEncryptString");
            string PublicKey = Configuration.GetConnectionString("Public_Key");

            CipherServices CS = new CipherServices();

            //�ѱK�s�u�r��
            string SQLConnectionString = CS.CipherToPlainText(CipherText, PublicKey);

            //�z�L DbContextOptions ����W����k�A�ӳs�� appsetting.json ���A�]�w�n�� SQL server connection string
            //services.AddDbContext<PortfolioContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("HeisenbergHsueh_Portfolio_DB")));
            services.AddDbContext<PortfolioContext>(options => options.UseSqlServer(SQLConnectionString));
            #endregion

            #region Cookie����
            //���Ucookie���Ҫ��A��
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                //�]�wcookie�W��
                options.Cookie.Name = "HeisenbergHsueh_Cookie";
                //browser �|���� cookie �u��g�� http protocol �Ӧs��
                options.Cookie.HttpOnly = true;
                //�]�w�n�J�����A���n�J�ɦ۰ʾɤJ��n�J����
                //options.LoginPath = new PathString("/LoginSystem/Login");
                //�]�w�n�X����(�������i�H�ٲ�)
                //options.LogoutPath = new PathString("/LoginSystem/Logout");
                //��n�J�̤��㦳��U�������X���v���ɡA�|�۰�����즹Forbidden����
                //options.AccessDeniedPath = new PathString("/LoginSystem/Forbidden");
                //�]�w�n�J���Įɶ�(���:����)
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //SlidingExpiration = true�A��ܻ��A�p�Gcookie��e���ɶ��w�g�W�L�}�l�ɶ��P�����ɶ����@�b�A�h���s�{�ocookie
                //options.SlidingExpiration = true;
                options.EventsType = typeof(HeisenbergHsuehCookieAuthenticationEvents);
            });
            #endregion

            services.AddScoped<HeisenbergHsuehCookieAuthenticationEvents>();

            #region ���v
            services.AddAuthorization(options => {
                //��h���v               
                options.AddPolicy("IsIT", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "IT");
                });
            });
            #endregion

            #region �h��y�t�]�w
            //�]�wŪ���귽��(.resx)�����|
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            //�[�J��View�BModel�]�i�ϥΦh��y�t���y�k
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            //�ŧi�@��CultureInfo���M��
            var supportedCultures = new[]
            {
                new CultureInfo("zh-TW"),
                new CultureInfo("en-US")
            };
            //
            services.Configure<RequestLocalizationOptions>(options =>
            {

                options.DefaultRequestCulture = new RequestCulture(culture: "zh-TW", uiCulture: "zh-TW");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
                options.AddInitialRequestCultureProvider(new RouteDataRequestCultureProvider()
                {
                    Options = options,
                    RouteDataStringKey = "culture",
                    UIRouteDataStringKey = "culture"
                });
            });
            #endregion
        }

        #region �ʺA����cookie authentication��LoginPath�BLogoutPath�BAccessDenied
        //�]�������]�w�F�h��y�t�A�]���ɭPuser�b�i�J�ݭn�n�Jor�ݭn���v�~�ݱo���������ɡAcookie authentication�����୶���X�{���D
        //�_�]�O�ѩ�Acookie authentication��LoginPath�BLogoutPath�BAccessDenied�����|�O�g�����A���]���y�t�]�w�A�ҥH�Ʊ�user�b���P�y�t��������
        //�i�H���������y�t�������A�]���Hoverride���覡�A���s��g�FRedirectToLogin�BRedirectToLogout�BRedirectToAccessDenied�������k
        //�ѦҸ��(1) : https://social.msdn.microsoft.com/Forums/en-US/8626b4cf-be32-45e7-9704-e98c6d11b10f/how-can-i-set-dynamic-configureapplicationcookie?forum=aspdotnetcore
        public class HeisenbergHsuehCookieAuthenticationEvents : CookieAuthenticationEvents
        {
            public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
            {
                string GetCurrentContextPath = context.Request.Path.ToString();

                string SetCulture = "";

                if(GetCurrentContextPath.Contains("zh-TW"))
                {
                    SetCulture = "zh-TW";
                }
                else if(GetCurrentContextPath.Contains("en-US"))
                {
                    SetCulture = "en-US";
                }

                context.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}/{SetCulture}/LoginSystem/Login";

                return base.RedirectToLogin(context);
            }

            public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
            {
                string GetCurrentContextPath = context.Request.Path.ToString();

                string SetCulture = "";

                if (GetCurrentContextPath.Contains("zh-TW"))
                {
                    SetCulture = "zh-TW";
                }
                else if (GetCurrentContextPath.Contains("en-US"))
                {
                    SetCulture = "en-US";
                }

                context.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}/{SetCulture}/LoginSystem/Logout";

                return base.RedirectToLogout(context);
            }

            public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
            {
                string GetCurrentContextPath = context.Request.Path.ToString();

                string SetCulture = "";

                if (GetCurrentContextPath.Contains("zh-TW"))
                {
                    SetCulture = "zh-TW";
                }
                else if (GetCurrentContextPath.Contains("en-US"))
                {
                    SetCulture = "en-US";
                }

                context.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}/{SetCulture}/LoginSystem/Forbidden";

                return base.RedirectToAccessDenied(context);
            }
        }
        #endregion

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

            #region �h��y�t�]�w
            app.UseRequestLocalization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=zh-TW}/{controller=JobRecordSystem}/{action=JobRecordSystemIndex}/{id?}");
            });
        }
    }
}
