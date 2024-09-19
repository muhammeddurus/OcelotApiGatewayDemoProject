using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Net.Sockets;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace CustomerAPI
{
    public static class ConsulExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient>(consul => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(configuration["Consul:Host"]);
            }));

            return services;
        }

        public static IApplicationBuilder RegisterServiceToConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();
            //var features = app.ServerFeatures as FeatureCollection;
            //var features = app.Properties["server.Features"] as FeatureCollection;
            //var addresses = features.Get<IServerAddressesFeature>();
            //var address = addresses.Addresses.First();
            //var a = features.Select(x => x.Value).ToList();
            //var uri = new Uri(address);
            //Socket s;
            //IPEndPoint remoteIpEndPoint = s.RemoteEndPoint as IPEndPoint; 
            //IPEndPoint localIpEndPoint = s.LocalEndPoint as IPEndPoint; 

            var registration = new AgentServiceRegistration()
            {
                ID = $"Customer",
                Name = "Customer",
                Address = $"https://localhost",
                Port = 7277,
                Tags = new[] { "Customer Service", "Customr" }
            };
            logger.LogInformation("Registering with Consul");

            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
}
