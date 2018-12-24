using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace pluralsight_aspdotnet_core_fundamentals
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IGreeter greeter, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.Use ( next =>
             {
                 logger.LogInformation("Request Incomming");
                 // this is were the middleware implementatio -> called on each request
                 return async context => { 
                        if( context.Request.Path.StartsWithSegments("/mym")) {
                            await context.Response.WriteAsync("PP");
                            logger.LogInformation("Request Handeld");
                        } else {
                            await next(context); // let other on the cain to react to that request
                            logger.LogInformation("Response outgoing");
                        }
                 };
             }
            );

            app.UseWelcomePage(new WelcomePageOptions {
                Path="/wp"
            });

            app.Run(async (context) =>
            {
                
                string greetering = greeter.GetMessageOfADay();
                await context.Response.WriteAsync(greetering);
            });
        }
    }
}
