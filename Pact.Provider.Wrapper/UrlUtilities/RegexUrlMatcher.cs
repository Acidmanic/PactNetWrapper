using System;
using System.Text.RegularExpressions;

namespace Pact.Provider.Wrapper.UrlUtilities
{
    public class RegexUrlMatcher:IUrlMatcher
    {
        private readonly Regex _regex;
        private readonly bool _caseSensitive;

        public RegexUrlMatcher(string regex, bool caseSensitive)
        {
            regex = Normalize(regex);
            _regex = new Regex(regex);
            _caseSensitive = caseSensitive;
        }

        private string Normalize(string url)
        {
            if (url.StartsWith("/"))
            {
                url =  url.Substring(1,url.Length-1);
            }
            if (!url.EndsWith("/"))
            {
                url += "/";
            }
            return url;
        }

        public RegexUrlMatcher(string regex):this(regex,true)
        {
            
        }


        public bool Matches(string url)
        {
            var matchingUrl = Normalize(url);

            try
            {
                return _regex.IsMatch(matchingUrl);
            }
            catch { }

            return false;
        }
    }
}