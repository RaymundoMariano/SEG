using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SEG.Client.Aplicacao;
using SEG.Client.Autenticacao;
using SEG.Client.Seguranca;
using SEG.Domain.Contracts.Clients.Aplicacao;
using SEG.Domain.Contracts.Clients.Autenticacao;
using SEG.Domain.Contracts.Clients.Seguranca;
using System;

namespace SEG.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Serviços da Aplicacao
            services.AddTransient<IEventoClient, EventoClient>();
            services.AddTransient<IFormularioClient, FormularioClient>();
            services.AddTransient<IModuloClient, ModuloClient>();
            services.AddTransient<IPerfilClient, PerfilClient>();
            services.AddTransient<IFuncaoClient, FuncaoClient>();
            services.AddTransient<IUsuarioClient, UsuarioClient>();

            //Serviço de Autenticação
            services.AddTransient<IAutenticacaoClient, AutenticacaoClient>();

            //Serviços de Seguranca
            services.AddTransient<ISegurancaClient, SegurancaClient>();

            //Serviço de Cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/Conta/Login";
                });

            //Todos os controllers protegidos contra acesso anônimo 
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllersWithViews();
            services.AddRazorPages();            
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
                app.UseExceptionHandler("/Error");
                //app.UseExceptionHandler("/Home/Error"); //<== Original
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
