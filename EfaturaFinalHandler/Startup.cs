using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SoapCore;
using System;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace EfaturaFinalHandler
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
            services.AddSoapCore();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EfaturaFinalHandler", Version = "v1" });
            });
            services.TryAddSingleton<IFaturaService, FaturaService>();
            services.AddSoapServiceOperationTuner(new EfaturaServiceOperationTuner());
            services.AddSoapExceptionTransformer((ex) => ex.Message);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EfaturaFinalHandler v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                //endpoints.UseSoapEndpoint<IFaturaService>("/Efatura.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
                //endpoints.UseSoapEndpoint<IFaturaService>("/Efatura.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
                endpoints.MapControllers();
            });


            app.Use(async (context, next) =>
            {
                var initialBody = context.Request.Body;

                using (var bodyReader = new StreamReader(context.Request.Body))
                {
                    string body = await bodyReader.ReadToEndAsync();
                    Console.WriteLine(body);
                    context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
                    var lw = new LogWriter();
                    lw.Logcu(body, context, context.Connection.RemoteIpAddress.ToString());
                    await next.Invoke();
                    context.Request.Body = initialBody;
                }
            });
            app.UseSoapEndpoint<IFaturaService>(path: "/gibhandler.wsdl", binding: new BasicHttpBinding());
            app.UseSoapEndpoint<IFaturaService>(path: "/gibhandler", binding: new BasicHttpBinding());


        }
    }
}
