using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pact.Provider.Wrapper.UrlUtilities
{
    /// <summary>
    /// This Url Matcher, takes a / delimited url pattern. each segment in the given pattern can be
    /// a string to be checked for exact match, or a named regular expression with format {regex-name},
    /// or a regular expression with format (reg-ex). 
    /// </summary>
    public class BySegmentUrlMatcher : IUrlMatcher
    {
        private readonly Func<string, bool>[] _matchers;
        private readonly bool _acceptChildren;

        public BySegmentUrlMatcher(string pattern, bool caseSensitive, bool acceptChildren)
        {
            var segments = pattern.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            _matchers = CreateMatchers(segments, caseSensitive);

            _acceptChildren = acceptChildren;
        }

        private Func<string, bool>[] CreateMatchers(string[] segments, bool caseSensitive)
        {
            var matchers = new Func<string, bool>[segments.Length];

            for (var i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];

                var regex = FindNamedRegex(segments[i]);

                if (regex == null)
                {
                    if (segment.StartsWith("(") && segment.EndsWith(")"))
                    {
                        var expressionString = segment.Substring(1, segment.Length - 2);

                        matchers[i] = s =>
                        {
                            var expression = new Regex(expressionString,
                                caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);

                            return expression.IsMatch(s);
                        };
                    }
                    else
                    {
                        matchers[i] = s =>
                        {
                            if (caseSensitive)
                            {
                                return s == segment;
                            }

                            return String.Equals(s, segment, StringComparison.CurrentCultureIgnoreCase);
                        };   
                    }
                }
                else
                {
                    matchers[i] = s =>
                    {
                        var expression = new Regex(regex.RegEx,
                            caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);

                        return expression.IsMatch(s);
                    };
                }
            }

            return matchers;
        }

        private NamedRegularExpression FindNamedRegex(string segment)
        {
            return NamedRegularExpressions.All.FirstOrDefault(r =>
                "{" + r.Name + "}" == segment);
        }

        public bool Matches(string url)
        {
            var segments = url.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length < _matchers.Length)
            {
                return false;
            }

            if (!_acceptChildren && segments.Length > _matchers.Length)
            {
                return false;
            }

            for (var i = 0; i < _matchers.Length; i++)
            {
                if (!_matchers[i](segments[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}