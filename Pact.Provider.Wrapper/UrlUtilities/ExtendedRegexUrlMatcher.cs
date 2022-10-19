namespace Pact.Provider.Wrapper.UrlUtilities
{
    public class ExtendedRegexUrlMatcher:RegexUrlMatcher
    {
        public ExtendedRegexUrlMatcher(string regex, bool caseSensitive) : base(ExpandRegex(regex), caseSensitive)
        {
            
        }

        private static string ExpandRegex(string regex)
        {
            string regexExpanded = regex;
            
            foreach (var r in NamedRegularExpressions.All)
            {
                regexExpanded = regexExpanded.Replace("{" + r.Name + "}", "(r.RegEx)");
            }

            return regexExpanded;
        }

        public ExtendedRegexUrlMatcher(string regex) : this(regex,true)
        {
        }
    }
}