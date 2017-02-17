
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/Write")
                {
                    var path = Environment.GetEnvironmentVariable("DEFAULT_PERSISTENT_PATH");
                    try 
                    {
                        var createText = "print('Hello NFS');" + Environment.NewLine;
                        File.WriteAllText(path, createText);    
                        var readText = "Data read from "+ path + " is " +File.ReadAllText(path);
                        Console.WriteLine(readText);
                        await context.Response.WriteAsync(readText);
                    } 
                    catch (Exception e) 
                    {
                        Console.WriteLine(e);
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync(e.Message);
                    }
                }
                else if (context.Request.Path == "/Write2")
                {
                    var path = Environment.GetEnvironmentVariable("DEFAULT_PERSISTENT_PATH");
                    try 
                    {
                        var createText = "print('Hello NFS');" + Environment.NewLine;
                        var fs = File.Create(path);
                        using(var r = new StreamWriter(fs))
                        {
                            r.WriteLine(createText);
                        }
                        File.ReadAllText(path); 
                        var readText = "Data read from "+ path + " is " +File.ReadAllText(path);
                        Console.WriteLine(readText);
                        await context.Response.WriteAsync(readText);
                    } 
                    catch (Exception e) 
                    {
                        Console.WriteLine(e);
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync(e.Message);
                    }
                }
                else
                {
                    await context.Response.WriteAsync("Hello World!");
                }
                
            });
        }
    }
}