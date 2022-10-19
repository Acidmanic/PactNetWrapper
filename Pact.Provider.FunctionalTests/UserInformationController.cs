using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Pact.Provider.FunctionalTests
{

    [Route("UserInformation")]
    [ApiController]
    public class UserInformationController : ControllerBase
    {
        [HttpGet]
        [Route("Summary")]
        public IActionResult GetSummary(string email)
        {
            return Ok(
                new UserSummary()
                {
                    Email = email,
                    FirstName = "John",
                    LastName = "Connor",
                    MobileNumber = "0912345678",
                    Id=0
                }
            );
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetuserById(long id)
        {
            return Ok(
                new UserSummary()
                {
                    Email = "john.connor@resistance.gov",
                    FirstName = "John",
                    LastName = "Connor",
                    MobileNumber = "0912345678",
                    Id=id
                }
            );
        }
        
        [HttpGet]
        [Route("Noghl")]
        public IActionResult GetNoghl()
        {

            if (Request.Headers["authorization"] != "radkonbere")
            {
                return Unauthorized();
            }
            
            return Ok(
                new UserSummary()
                {
                    Email = "noghl@user.app",
                    FirstName = "John",
                    LastName = "Connor",
                    MobileNumber = "0912345678",
                    Id=0
                }
            );
        }
        
        [HttpGet]
        [Route("Nabaat")]
        public IActionResult GetNabaat()
        {
            return Ok(
                new UserSummary()
                {
                    Email = "nabaat@user.app",
                    FirstName = "John",
                    LastName = "Connor",
                    MobileNumber = "0912345678",
                    Id=0
                }
            );
        }
        
        [HttpGet]
        [Route("AuthParam")]
        public IActionResult GetAuthWithParam(UserSummary summary)
        {

            if (Request.Headers["authorization"] != "radkonbere")
            {
                return Unauthorized();
            }
            
            return Ok(
                summary
            );
        }
        
        [HttpGet]
        [Route("users/{id}")]
        public IActionResult GetUserInformationById(string id)
        {

            return Ok(
                new UserInformation()
                {
                    Email = "john.connor@resistance.gov",
                    FirstName = "John",
                    LastName = "Connor",
                    MobileNumber =  "0912345678",
                    PhoneNumber = "0123456789",
                    YearOfBirth = 1985,
                    NationalId = "0071110011",
                    Address= "Off The Grid",
                    PostCode = "1000000001",
                    CompanyName = null,//"Cyberdine Systems",
                    EconomicCode = null,//"10101010101",
                    RealPerson = false,
                    Id=Guid.NewGuid().ToString()
                }
            );
        }

    }
}