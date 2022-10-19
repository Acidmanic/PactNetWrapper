using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Models.Augment;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.Wrapper.PactnetVerificationPublish
{
    public class TaggedLinkPactnetBrokerPublish:IPactnetVerificationPublish
    {
        private static readonly string VerificationResultKey;
        private readonly string _verificationResultBrokerLink;
        private readonly IPublicationTagger _tagger;

        public TaggedLinkPactnetBrokerPublish(string verificationResultBrokerLink, IPublicationTagger tagger)
        {
            _verificationResultBrokerLink = verificationResultBrokerLink;
            _tagger = tagger;
        }

        public TaggedLinkPactnetBrokerPublish(string verificationResultBrokerLink)
        :this(verificationResultBrokerLink, new ColumnDelimitedPublicationTagger())
        {
        }

        static TaggedLinkPactnetBrokerPublish()
        {
            VerificationResultKey = "pb:publish-verification-results";
        }
        
        public bool InterceptPactBeforeVerification(Models.Pact pact, Interaction runningInteraction)
        {
            string tag = _tagger.TagInteraction(new InteractionInfo().UpdateFrom(pact,runningInteraction));
            
            string link = ResolveUrl(this._verificationResultBrokerLink, tag);

            if (pact._links == null)
            {
                pact._links = new BrockerApi();
            }

            pact._links[VerificationResultKey] = new Link
            {
                Title = "Publish verification results",
                Href = link
            };
            return true;
        }
        
        private static string ResolveUrl(string baseUrl, string tag)
        {
            var url = baseUrl;

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url + "/" + tag;
        }
    }
}