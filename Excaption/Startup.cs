using BusinessLogicLayer.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Excaption
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
            services.AddControllersWithViews();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
                app.UseExceptionHandler(new ExceptionHandlerOptions {
                ExceptionHandler = (c) =>
                {
             
                    
                    var exception = c.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = exception.Error.GetType().Name switch
                    {
                        "ArgumentException" => HttpStatusCode.BadRequest,
                        _ => HttpStatusCode.ServiceUnavailable
                    };

                   
                    StackTrace st = new StackTrace(exception.Error,true);
                    string stackIndent = "";

                    StackFrame sf = st.GetFrame(0);

                    var methodName = sf.GetMethod().Name;
                    var fileLineNumber = sf.GetFileLineNumber();
                    var errorMessage = exception.Error.Message;
                    var source = exception.Error.Source;
                    var StackTrace = exception.Error.StackTrace;
                    var errorType = exception.Error.GetType();

                    stackIndent += " methodName : " + methodName;
                    stackIndent += " fileLineNumber : " + fileLineNumber;
                    stackIndent += " errorMessage : " + errorMessage;
                    stackIndent += " source : " + source;
                    stackIndent += " StackTrace : " + StackTrace;
                    stackIndent += " errorType : " + errorType;




                    //Exception ex = GetInnerException(exception.Error);

                    c.Response.StatusCode = (int)statusCode;
                    /* var contentBuilder = new StringBuilder();
                     contentBuilder.Append($"statusCode: /n" + statusCode);
                     contentBuilder.Append($"exceptionErrorMessage:" + exception.Error.Message);
                     contentBuilder.Append($"exceptionErrorSource:" + exception.Error.Source);
                     contentBuilder.Append($"stackTrace:" + exception.Error.StackTrace);
                     contentBuilder.Append($"targetSite:" + exception.Error.TargetSite);*/
                    



                    var content = Encoding.UTF8.GetBytes($"Error [{stackIndent}]");
                    c.Response.Body.WriteAsync(content, 0, content.Length);

                    MailHelper mail = new MailHelper();
                    mail.SendMail("Projenizde Hata Oluþtu", stackIndent);

                    return Task.CompletedTask;
                }
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

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
