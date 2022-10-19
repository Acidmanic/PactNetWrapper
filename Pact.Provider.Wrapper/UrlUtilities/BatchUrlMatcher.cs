using System.Collections.Generic;

namespace Pact.Provider.Wrapper.UrlUtilities
{
    public class BatchUrlMatcher:IUrlMatcher
    {
        private readonly List<IUrlMatcher> _urlMatchers;
        

        public BatchUrlMatcher()
        {
             _urlMatchers = new List<IUrlMatcher>();
        }


        public BatchUrlMatcher Add(IUrlMatcher matcher)
        {
            _urlMatchers.Add(matcher);

            return this;
        }


        public bool Matches(string url)
        {
            foreach (var urlMatcher in _urlMatchers)
            {
                if (urlMatcher.Matches(url))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// If A Batch Matcher has no children added, it never matches any urls, therefore the client code
        ///  should be able to check it its empty or not. 
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _urlMatchers.Count == 0;
        }
    }
}