using System;
using Microsoft.AspNetCore.Hosting;

namespace Pact.Provider.FunctionalTests
{
    public class Server<TStartup>:IDisposable where TStartup : class
    {
        private IWebHost _app;

        public Server(int port)
        {
            var builder = new WebHostBuilder();

            builder.UseKestrel(options => options.ListenLocalhost(port));

            builder.UseStartup<TStartup>();

            _app = builder.Build();

            _app.Start();
        }
        
        
        public void Dispose()
        {
            try
            {
                _app.StopAsync().Wait();
            }
            catch (Exception ) { }
            try
            {
                _app.Dispose();
            }
            catch (Exception ) { }
        }
    }
}