using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SoapCore;
using System.ServiceModel;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Efatura", Version = "v1" });
            });
            services.TryAddSingleton<IFaturaService, FaturaService>();
            
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
            app.UseSoapEndpoint<IFaturaService>(path: "/Efatura.wsdl", binding: new BasicHttpBinding());


        }
    }
}
