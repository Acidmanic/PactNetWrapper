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
   
   
Simple Example
================
 
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

Other Feature
===========
 * Run Tests Separately
 * Use Matchers
 * Register Request Filters
 * Publish Results 
    * Using Pact Broker
    * To Html file
    * Using Custom Publisher(s)
 * Verify Using PactNet (Rubby Wrapper) Or BuiltIn C# Pact verifier (InternalVerifier)
 * Url Matching Patterns
    * Url Matching for Endpoint test selection
    * Url Matching for Request Filtering
    
More Details
----

For More detailed version of this readme file, please refer to [Github Page](https://github.com/Acidmanic/PactNetWrapper) for this project.

Updates
======

You can trace changes and updates log in [Change log Document](https://github.com/Acidmanic/PactNetWrapper/blob/master/ChangeLog.md).


Regards. 

Mani.