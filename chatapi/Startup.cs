using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Akka.Actor;
using chatapi.Domain.Actor;
using static chatapi.ActorProviders;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Http;
using chatapi.Module.WebSocketManager;
using chatapi.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;

namespace chatapi
{
    public class Startup
    {

        static private String appName = ".net core chat api";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSession();

            // Register ActorSystem
            services.AddSingleton(_ => ActorSystem.Create("chatapi", ConfigurationLoader.Load()));
            
            services.AddSingleton<SessionManagerProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                IActorRef sessionManager = actorSystem.ActorOf(Props.Create(() => new LocalSessionManager()));
                return ()=> sessionManager;
            });

            services.AddWebSocketManager();
            

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = appName, Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", appName + "V1");
            });

            app.UseSession();

            app.UseMvc();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            
            app.UseWebSockets(webSocketOptions);

            app.MapWebSocketManager("/ws", serviceProvider.GetService<CSChatHandler>());

            app.UseFileServer();
            
            lifetime.ApplicationStarted.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>(); // start Akka.NET
            });
            lifetime.ApplicationStopping.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>().Terminate().Wait();
            });

        }
        
    }
}
