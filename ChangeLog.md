


1.1.0
-----
* ```[SkipAll]```, ```[Endpoint("path/to/service")]``` and ```[SkipEndpoint("path/to/service")]``` attributes allow to 
 run tests per selected endpoints.


1.2.0
-----
* Support for Internal PactVerifier (instead of PactNet).
    *   Support for Matchers in Response.Body
    
    
1.3.0
-----
* Internal PactVerifier Supports Matchers for Response.Headers
* Internal PactVerifier's type matching improved.


1.3.2
-----
   * Issue "NullReference Exception for null/empty body response" has been fixed.
   * NullReference exception for null MatchingRules, has been fixed.
   
   
1.4.0
-----
   * Fix: Missing query parameters on internal pact verifier.
   * Add Request Filters for request.body, request.headers and request.query.
   
 1.4.1
 -----
   * Fix: unmatched matching paths due to first char being / 
   
   
 1.5.0
 -----
   * Add UrlMatching
   * Use UrlMatching for Endpoint/Skip attributes
   * Use UrlMatching for Filter Requests
   
 1.5.1
 -----
   * Add request filter support for $.path 
   
 1.5.2
 -----
   * Fix Builtin Custom publisher exception (Test failure) on network errors 
   * Update Builtin publisher to publish tags both per endpoint and per interaction