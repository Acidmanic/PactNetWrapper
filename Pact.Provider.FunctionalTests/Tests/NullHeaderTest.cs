using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Pact.Provider.Wrapper.IO;
using Pact.Provider.Wrapper.Verification;
using Pact.Provider.Wrapper.Verification.Publishers;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher;

namespace Pact.Provider.FunctionalTests.Tests
{
    public class NullHeaderTest
    {
        private class NullHeaderTestServer
        {
            public NullHeaderTestServer(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    // endpoints.MapControllers();
                    endpoints.MapGet("/currencies", c =>
                    {
                        var responseObject = new
                        {
                            Currencies = new List<object>
                            {
                                new
                                {
                                    Id = 1,
                                    ShortSign = "﷼",
                                    Separator = "و",
                                    NamedSteps = new List<object>
                                    {
                                        new { Name = "", Boundary = 1 },
                                        new { Name = "هزار", Boundary = 1000 },
                                        new { Name = "میلیون", Boundary = 1000000 },
                                        new { Name = "میلیارد", Boundary = 1000000000 },
                                    }
                                }
                            }
                        };

                        var json = JsonConvert.SerializeObject(responseObject);

                        return c.Response.WriteAsync(json);
                    });
                });
            }
        }

        public void Main()
        {
            var pactsPath = "../../../../Pacts";

            using var server = new Server<NullHeaderTestServer>(9222);

            var bench = new PactVerificationBench("http://localhost:9222");

            bench
                .UseInternalPactVerifier()
                .WithPublishers()
                .Add(new HtmlReportVerificationPublisher("Report.html"));


            bench.Verify(pactsPath);
        }
    }
}