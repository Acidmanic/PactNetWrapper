namespace Pact.Provider.Wrapper.UrlUtilities
{
    public class NoneUrlMatcher:IUrlMatcher
    {
        public bool Matches(string url)
        {
            return false;
        }
    }
}