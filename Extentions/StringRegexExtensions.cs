using System.Text.RegularExpressions;


// ReSharper disable once CheckNamespace
namespace System
{
    public static class StringRegexExtensions
    {

        public static bool Matches(this string stringInQuestion, string regEx)
        {
            Match m = Regex.Match(stringInQuestion, regEx, RegexOptions.IgnoreCase,
                new TimeSpan(0,0,1));

            return m.Success;
        }
    }
}