
![Avatar](Graphics/pactnet-wrapper.png)

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

Get the library
===============

This library is available on [NuGet.org](https://www.nuget.org/packages/Pact.Provider.Wrapper/), so you can add library to your dotnet core application via .Net Cli using:

```bash 
	dotnet add package Pact.Provider.Wrapper 

```

Or Visualstudio Package Manager, using 

```bash 
	Install-Package Pact.Provider.Wrapper 

```
Or by directly adding reference to .csproj file:

```xml
	<PackageReference Include="Pact.Provider.Wrapper" />

```

 
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

I have my own implementation named:```AcidmanicPactBrokerPublisher```, which takes an broker-address and a token in 
its constructor. Where the address points to where I host my broker. my [broker](https://github.com/Acidmanic/pactbroker-server) 
Also needs a token to communicate, which is also passed to the constructor. 

(In real case scenario, to prevent putting passwords in code, i take it from environment variables, which is useful when you are running your tests in cicd pipelines)    

This publisher is able to publish results per-endpoint AND per-service (resource-name) and per interaction (based on provider state). 
 You can also implement your own badge broker server to receive published results from this publisher. For a performed verification,  
 it will publish results as an object with this structure:
 
 ```json
    {
      "<tag-string>": "<result-string>",
      "<tag-string>": "<result-string>",
      ....
    }
``` 

where &lt;tag-string&gt; is a tag generated to be unique for the specific result (for example an interaction verification),
 and &lt;result-string&gt; is either the value "Success" or "Failure".
 
 The result for each interaction comes directly from PACT tests verifying actual responses for each interaction against expectations 
 for that specific interaction. The results for each Endpoint are the result of logical __AND__ over all interactions under that endpoint. 
 In a same way, The results for each Service are the result of logical __AND__ over all interactions towards that resource.
 
 Tags are being generated for each result following these rules:
 
 * For an endpoint: endpoint:&lt;column-delimited-uri&gt;
    * It Omits the provider state
    * It Omits the http method
 * For a service: service:&lt;resource-name&gt;
    * It Omits the provider state
    * It Omits the http method
    * It Omits the uri based on service name
 * For an interaction: interaction:&lt;column-delimited-uri&gt;:&lt;http-method&gt;:&lt;normalized-provider-state&gt;
    * It Uses all information to specifically points to the exact interaction
    
 * &lt;column-delimited-uri&gt; is the request uri, that all it's slash and back slashes are replaced with a column  character (:)
 * &lt;resource-name&gt; is the first segment of the request uri
 * &lt;http-method&gt; is the http method specified in the request
 * &lt;normalized-provider-state&gt; is provider state string for that interaction but only digits and alphabetical characters are kept
 
 For example, consider a pact with this interaction:
 * Method: POST
 * towards the uri: 'users/125'
 * provider state:  'the user with id = 125 and username @fireman, exists in the database.'
 
a successful verification for this result delivered to ```AcidmanicPactBrokerPublisher```, will cause this publisher
 to generate this json:
 
 ```json
    {
      "service:users": "Success",
      "endpoint:users:125": "Success",
      "interaction:users:125:get:theuserwithid125andusernamefiremanexistsinthedadabase": "Success"
    } 
```
This object then will be send (POST) as a json payload towards the broker-address received from constructor, having 
the token added in headers with the key token. 

 

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


* __NOTE__: If ```SkipAll``` is present, none of tests will run. regardless of presence of 
other attributes.

* __NOTE__: If none of ```SkipAll``` and ```Endpoint``` are present, All available endpoints
 will be tested.  This way you can exclude undesired tests by adding ```SkipEndpoint``` attributes.
 
 * __NOTE__: If a number of ```Endpoint``` attributes are present, the ```SkipAll``` attributes 
 will undo added endpoints using ```Endpoint```. 

* __NOTE__: The ```'path/to/service'``` parameters, are url patterns which can be combined with 
regular expressions. See [Url Matching](#url-matching) for more details on writing url patterns.
 also, both ```Endpoint``` and ```SkipEndpoint``` attributes, can optionally get parameters: 
    * acceptChildren: If true, any request path, starting with given pattern will be accepted. 
    If false, only request paths that fully match with the pattern will be accepted.
    * caseSensitive: effects string comparisons.  

Matchers
================

I Got into some problems using PactNet with matchers at provider side's tests. Since i could not
 find a way to get matchers work yet, i implemented an internal PactVerifier which does support 
 matchers for request body (for now). So using this is also another option. You can use it by calling
  ```bench..UseInternalPactVerifier()```. This will use builtin pact verifier instead of wrapped rubby code.

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
        // Make it use builtin verifier
        bench.UseInternalPactVerifier();
        // Assuming the test projects executable is built at 
        // <project-directory>/bin/<configuration>/<sdk>/<executable>
        // for example: Example.Pact.Test/bin/Debug/netcoreapp3.1/Example.Pact.Test
        bench.Verify("../../../../Pacts");
        // Stop api service(s)
        app.StopAsync().Wait();
    }
 ```


Request Filters
=========

In Some cases, you might need to manipulate a pact interaction details, in provider side tests, just before 
sending the request to your service endpoint. The most common use-case is when you need to put 
a real authorization token instead of dummy/fake token coming from pact file. For that to be done, 
Pact provides a concept called RequestFilters. Request filters will be executed like a middleware before 
sending the request to service and the manipulate requests. This must be implemented at the provider side 
pact-library. You can fipath/to/servicend more details on the concept [Here](https://docs.pact.io/faq/#how-do-i-test-oauth-or-other-security-headers) 
and [Here](https://docs.pact.io/implementation_guides/go/readme/#lifecycle-of-a-provider-verification). 

Using PactNet wrapper versions >= 1.4.0, you can register request filters by calling the ```WithRequestFilters()``` 
method on ```PactVerificationBench``` object. 
The Request filter builder has four methods:
 * Add()
   * This will create a new Request filter, __DO NOT FORGET CALLING ADD BEFORE CREATING EACH FILTER__. even on the first one.
 * Put(data)
   * this is actual data which will replace the information from pact file, for example it 
   can be your real authorization token generated in provider side test method at runtime.
 * At("$...")
   * This method takes a data path, conforming with the format pact uses in its matchers. 
   This ways you can specify, where given data should be put. This can allow to register manipulation for 
     * Request body ("$.body.<field-name>.....<field-name>")
     * Request Headers ("$.headers.<header-name>")
     * Request Queries ("$.query.<parameter-name>")
     * Request Path ("$.path")
 * WithRequestPathUnder("/") or WithRequestPath("/")
   * Optionally, these methods allow to mark one or more url patterns, to filter which endpoints will
    be intercepted for request filtering. These methods can be called multiple times for each request 
    filter, allowing to collect any interactions in mind. 
       
 | __Note__ |
 | :--- |
 | Filters will not introduce new data to interactions. They only manipulate existing data which is read from Pact files. Otherwise, this would be manipulating the structure of the contract causing hard to spot bugs and failures. So ___Request Filters Only work on data which is already present in Pact file___ |
 
 | __Note__ |
 | :--- |
 | Request Filters, Works whether you use PactNet verifier (wrapped Rubby code) or internal pact verifier. |

 | __Note__ |
 | :--- |
 |  Request filters registered for body manipulation, can call Put(.) method with any object type matching with target field type in body object. |
 |  Request filters registered for headers manipulation, can call Put(.) method only with string or string-cast-able types.|
 |  Request filters registered for query manipulation, can call Put(.) method only with string or string-cast-able types.|
 |  Request filters registered for path manipulation, can call Put(.) method only with string or string-cast-able types.|


 | __Note__ |
 | :--- |
 | Updating request path, in provider side's test, might be needed for cases that provider can not seed pre defined value for a part of url, like user id received from authorization. But use this request filter with caution!|

 | __Note__ |
 | :--- |
 | ```WithRequestPathUnder(.)``` accepts all request paths starting with given pattern while ```WithRequestPath(.)```  only accepts exact matched request paths |
 | Both ```WithRequestPathUnder(.)``` and ```WithRequestPath(.)```  methods can accept an argument: _caseSensitive_, determining the string comparisons. |
 | Both ```WithRequestPathUnder(.)``` and ```WithRequestPath(.)```  methods use Url Matching rules as described [here](#url-matching). |


This code example shows registering request filters for body, header and query manipulation. But in real cases, 
you might mostly just use header manipulation for authorization.

 ```c#
    //...
    // Create Test bench object
    var bench = new PactVerificationBench("http://localhost:9222");        
    // Configure Test bench
    bench
        .UseInternalPactVerifier()
        .WithPublishers()
        .Add(new HtmlReportVerificationPublisher("Report.html"));
    // Use RequestFilterCollectionBuilder to register request filters.
    bench.WithRequestFilters()
    // Register a request filter to update Bearer authorization headers (if any present)
        .Add().Put("Bearer eyJzdWaW.F0IjoxNTE2M.jM5MDIyfQ").At("$.headers.authorization")
    // Register a request filter to change the value of email query parameter, if any present.
    // This filter, will be applied only on requests towards endpoints starting with 'users/'.
        .Add().Put("john.connor@resistance.gov").At("$.query.email").WithRequestPathUnder("users")
    // Register a request filter to put current datetime in request body
        .Add().Put(DateTime.Now).At("$.body.updateInformation.lastupdate")
    ;
```

Url Matching
===========

Url patterns, given to EndpointAttribute or SkipEndpointAttributes, and also Url patterns given 
to ```WithRequestPathUnder(.)``` or ```WithRequestPath(.)``` methods for request filters, follow 
a simple format to determine which urls are included or not.

 * by default they behave as exact matchers. ```'/a/b'``` will match a request to address _/a/b_
 * each segment can be replaced with a regular expression in parenthesis. 
    * for example: ```'/a/([0-9]+)'``` will match _/a/0_, _/a/12384_ and will not match _/a/b_
 * for some more common formats like id or email, you can use builtin named regular expressions, 
 using their names in curly braces. 
    * ex:   ```'/users/{IntegerId}'``` will match _'/users/2343/'_ or _'/users/012/'_
    * ex:   ```'/users/{Email}'``` will match _'/users/john.connor@resistance.la/'_
    * These are currently shipped builtin named regular expressions:
        * __Email__: Matches email addresses  
        * __Guid__: Matches guid strings
        * __Time12__: Matches 12 hours time strings 
        * __Time24__: Matches 24 hours time strings
        * __Time__: Matches either of 24 or 12 hours time strings
        * __IntegerId__: Matches integer numbers of any length
        * __PhoneNumber__: Matches most formats for a phone number 
        * __Any__: Any string, including empty string

Settle provider before each interaction
-------------------

This feature allows the provider pact test to prepare provider state according to what is expected in the pact file. While 
you're initializing your test bench, you can add as many provider settle actions as you need using the method 
```SettleProvider(state,settleAction)```. The first argument ```state``` is a string representing the state field of 
the interaction from the pact file. __It Supports Regular Expressions__ so you can tolerate small changes in pact details 
and also it makes it possible to treat similar provider states in a single settle-action.
The PactVerifier would run your action for specified state, right before making the request towards the provider. 
The ```settleAction```, would be an ```Action<PactRequest>``` which allows you to also manipulate the request before being 
sent to provider. In some cases it would be helpful to change the request to make the provider state happening. For example 
for Pact interaction that states the user authentication token is wrong.  

__Example:__

```c#
    //...
    // Create Test bench object
    var bench = new PactVerificationBench("http://localhost:9222");        
    // Configure Test bench
     bench
        .SettleProvider(".*Test User Exists In Database.*", request => _usersServide.Add(new TestUser()))
        .UseInternalPactVerifier()
        .WithPublishers()
        .Add(new HtmlReportVerificationPublisher("Report.html"));
            
                bench.Verify("../../../../Pacts");

```
 
  | __Note__ |
  | :--- |
  | This only works with built-in PactVerifier. This feature is not currently supported for wrapped PactNet verifier since it would need to tear apart pact file per interactions and run each as a pact file with single interaction |
  
  | __Note__ |
  | :--- |
  | The code you add in your settle actions, might throw exceptions. These exceptions are not being thrown since we are interested in exceptions from the pact test itself. But It's possible to receive theme through the ```SettleActionExceptionListener``` property of test bench. |
    
  | __Note__ |
  | :--- |
  | Request filters apply on whole endpoint which makes them more general than ProviderState which are being applied on single interaction. So Provider states would override request filters manipulations |
      

Updates
======

You can trace changes and updates log in [Change log Document](ChangeLog.md).



Regards. 

Mani.
