About
=============

In my use cases of [Pactnet](https://github.com/pact-foundation/pact-net), i needed to perform broker related tasks in different jobs in cicd, i also wanted to be able to connect PactNet to my own broker. There fore there was some code repeating across all my 
projects that i preferred to have them as a separate package. And here it is.

This library

* Merges all available Pact files
* Performs tests per each interaction
* Publishes the results for each Endpoint.
  * Each request path, is considered an api endpoint, it may have one or more interactions.
  * The result of testing an endpoint, is successful when all interaction tests towards this endpoint is passed.
* Publishers are extendable, the package also contains an Html publisher which produces an html file presenting the results.

In my case, i publish the results to a badge server and i use these badges in my api wiki, so any team member can have see the state of implementation of apis at the api wiki.

 
| __Important Note__ |
| :--- |
| pact-net package wraps the ruby version of pact and when using it and you need to reference its package regarding the platform your tests are running at. So for example 
  in my case, which i run my tests on a 64 bit linux, i add   ```<PackageReference Include="PactNet.Linux.x64" Version="3.0.0" />``` reference alongside the ```<PackageReference Include="Pact.Provider.Wrapper" />``` reference in my csproj file (or via package manager and etc.). Other platforms can be __PactNet.Windows__, __PactNet.Linux.x86__ and __PactNet.OSX__ |
   
   
Examples
================
 
 Without publishing the results
 ------------
 
 ```c#
    // Unit test method based on exceptions like xUnit (with [Fact] attribute or NUnit or etc...
    public void Providers_GivenPacts_ShouldHonorAllPacts(){
        // settup your application
        var app = SetupApp();
        // Start api service(s)
        app.Start();
        // Create test bench
        // This example assumes your services would be up at http://localhost:9222 
        var bench = new PactVerificationBench("http://localhost:9222");
        // Assuming the test projects executable is built at 
        // <project-directory>/bin/<configuration>/<sdk>/<executable>
        // for example: Example.Pact.Test/bin/Debug/netcoreapp3.1/Example.Pact.Test
        bench.Verify("../../../../Pacts");
        // Stop api service(s)
        app.StopAsync().Wait();
    }
 ```


With Original Pact broker
-----------------

 ```c#
    // Unit test method based on exceptions like xUnit (with [Fact] attribute or NUnit or etc...
    public void Providers_GivenPacts_ShouldHonorAllPacts(){
        // settup your application
        var app = SetupApp();
        // Start api service(s)
        app.Start();
        // Create test bench
        // This example assumes your services would be up at http://localhost:9222 
        var bench = new PactVerificationBench("http://localhost:9222");
        // >>>>>>>>>>>>>>>>>>>>>>
        // Use DefaultPactnetVerificationPublish which makes test bench use the
        // Original Pact broker for publishing results besed on _links data. 
        bench.PactnetVerificationPublish = new DefaultPactnetBrokerPublish();
        // Assuming the test projects executable is built at 
        // <project-directory>/bin/<configuration>/<sdk>/<executable>
        // for example: Example.Pact.Test/bin/Debug/netcoreapp3.1/Example.Pact.Test
        bench.Verify("../../../../Pacts");
        // Stop api service(s)
        app.StopAsync().Wait();
    }
 ```


Use Html Publisher
------------------

 ```c#
    // Unit test method based on exceptions like xUnit (with [Fact] attribute or NUnit or etc...
    public void Providers_GivenPacts_ShouldHonorAllPacts(){
        // settup your application
        var app = SetupApp();
        // Start api service(s)
        app.Start();
        // Create test bench
        // This example assumes your services would be up at http://localhost:9222 
        var bench = new PactVerificationBench("http://localhost:9222");
        // >>>>>>>>>>>>>>>>>>>>>>
        // With this line, an html file named Report.html will be generated where 
        // your executable file is placed. The filename argument can point to any 
        // other address you prefer. 
        bench.WithPublishers()
                        .Add(new HtmlReportVerificationPublisher("Report.html"))
        // Assuming the test projects executable is built at 
        // <project-directory>/bin/<configuration>/<sdk>/<executable>
        // for example: Example.Pact.Test/bin/Debug/netcoreapp3.1/Example.Pact.Test
        bench.Verify("../../../../Pacts");
        // Stop api service(s)
        app.StopAsync().Wait();
    }
 ```

Use a Custom Publisher
-------------------
You can implement the interface ```IVerificationPublisher```, and use it for publishing verification results. 
I have my own implementation named: __AcidmanicPactBrokerPublisher__, which takes an address and a token in 
its constructor. Where the address points to where I host my broker. my [broker](https://github.com/Acidmanic/pactbroker-server) 
Also needs a token to communicate, which is also passed to the constructor. 

(In real case scenario, to prevent putting passwords in code, i take it from environment variables, which is useful when you are running your tests in cicd pipelines)    

```c#
    // Unit test method based on exceptions like xUnit (with [Fact] attribute or NUnit or etc...
    public void Providers_GivenPacts_ShouldHonorAllPacts(){
        // settup your application
        var app = SetupApp();
        // Start api service(s)
        app.Start();
        // Create test bench
        // This example assumes your services would be up at http://localhost:9222 
        var bench = new PactVerificationBench("http://localhost:9222");
        // In this example the class YourCustomPactBrokerPublisher, is an implementation
        // of the interface IVerificationPublisher
        bench.WithPublishers().Add(new YourCustomPactBrokerPublisher());
        // Assuming the test projects executable is built at 
        // <project-directory>/bin/<configuration>/<sdk>/<executable>
        // for example: Example.Pact.Test/bin/Debug/netcoreapp3.1/Example.Pact.Test
        bench.Verify("../../../../Pacts");
        // Stop api service(s)
        app.StopAsync().Wait();
    }
 ```


You can also use multiple Publishers together by calling the Add method in a fluent syntax fashion.

```c#
    bench.WithPublishers()
                .Add(new HtmlReportVerificationPublisher("Report.html"))
                .Add(new YourCustomPactBrokerPublisher());
``` 

That's about it. I hope it helps saving your time.

Run Tests Separately
==================

In version >= 1.1.0, you can add following attributes to your test method to control 
what 'Endpoints' are going to be considered for Contract testing:

* ```[SkipAll]```: Will not test any endpoints. All other steps will be done but no endpoints
would be tested. Its useful to check if everything is working before running tests.
* ```[Endpoint("path/to/service")]``` This attribute will add an endpoint to be tested.
* ```[SkipEndpoint("path/to/service")]```: This attribute will remove given endpoint from being tested.


More Details
============

For More detailed version of this readme file, please refer to [Github Page](https://github.com/Acidmanic/PactNetWrapper) for this project.



Regards. 

Mani.