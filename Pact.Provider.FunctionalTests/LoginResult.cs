namespace Pact.Provider.FunctionalTests
{
    public class LoginResult
    {
        public string Email { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }

        public int ExpiresIn { get; set; }
    }
}