using EVendas.Domain;
using EVendas.Repository;
using EVendas.Repository.Interfaces;
using EVendas.Service;
using EVendas.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eVendasEstoque
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RepositoryContext>(opt =>
                                               opt.UseInMemoryDatabase(databaseName: "EVendasDataBaseLoja"));
            

            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddScoped<IProdutoService, ProdutoService>();

            services.AddScoped<IServiceBusProducer, ServiceBusProducer>();

            services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();


            services.AddSwaggerGen();

            services.AddControllers();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();



            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EVendasEstoque");
                c.RoutePrefix = string.Empty;
            });

            var bus = app.ApplicationServices.GetService<IServiceBusConsumer>();
            bus.RegisterOnMessageHandlerAndReceiveMessages();
        }
    }
}
