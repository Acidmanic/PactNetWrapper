using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Pact.Provider.FunctionalTests
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginCredentials credentials)
        {
            
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            Console.WriteLine("Authorization Header: " + authHeader);
            
            return Ok(
                new LoginResult
                {
                    Email = credentials.Email,
                    LastName = "Moayedi",
                    Token = "susssschchchchchchc.biiiiiiggggggg.toooookeeeeeeeeeen",
                    ExpiresIn = 3600
                }
            );
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult LogOut()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            
            Console.WriteLine("Authorization Header: " + authHeader);

            if (authHeader == "Bearer this-is-acidmanic-the-ha")
            {
                return Ok(new
                {
                    Success = true
                });
            }
            return BadRequest();
        }
    }
}