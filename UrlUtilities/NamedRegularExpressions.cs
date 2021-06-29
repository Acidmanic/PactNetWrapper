using System.Collections.Generic;

namespace Pact.Provider.Wrapper.UrlUtilities
{
    public class NamedRegularExpressions
    {
        public static readonly NamedRegularExpression Email = new NamedRegularExpression
        {
            Name = "Email",
            RegEx = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"
        };
        
        public static readonly NamedRegularExpression Guid = new NamedRegularExpression
        {
            Name = "Guid",
            RegEx = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$"
        };
        public static readonly NamedRegularExpression Time12 = new NamedRegularExpression
        {
            Name = "Time12",
            RegEx = @"^(?:0?[0-9]|1[0-2]):[0-5][0-9] [ap]m$"
        };
        public static readonly NamedRegularExpression Time24 = new NamedRegularExpression
        {
            Name = "Time24",
            RegEx = @"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$"
        };
        public static readonly NamedRegularExpression Time = new NamedRegularExpression
        {
            Name = "Time",
            RegEx = @"^(?:(?:0?[0-9]|1[0-2]):[0-5][0-9] [ap]m|(?:[01][0-9]|2[0-3]):[0-5][0-9])$"
        };
        public static readonly NamedRegularExpression IntegerId = new NamedRegularExpression
        {
            Name = "IntegerId",
            RegEx = @"[0-9]+$"
        };
        public static readonly NamedRegularExpression PhoneNumber = new NamedRegularExpression
        {
            Name = "IntegerId",
            RegEx = @"(\+[0-9]{2}|\+[0-9]{2}\(0\)|\(\+[0-9]{2}\)\(0\)|00[0-9]{2}|0)([0-9]{9}|[0-9\-\s]{9,18})$"
        };
        public static readonly NamedRegularExpression Any = new NamedRegularExpression
        {
            Name = "Any",
            RegEx = @".*$"
        };
        
        public static IEnumerable<NamedRegularExpression> All = new List<NamedRegularExpression>()
        {
            Email,Guid,Time12,Time24,Time,IntegerId,PhoneNumber,Any
        }; 
    }
}