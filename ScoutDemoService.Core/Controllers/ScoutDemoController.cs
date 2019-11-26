using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ScoutDemoService.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoutDemoController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Models.PersonModel> Get()
        {
            List<Models.PersonModel> PersonList = new List<Models.PersonModel>();

            Models.PersonModel FirstPerson = new Models.PersonModel
            {
                FirstName = "Bob",
                LastName = "Smith",
                EmailAddress = "anyone@anywhere.com",
                Address1 = "123 anystreet",
                Address2 = "Suite 123",
                City = "Anytown",
                State = "GA",
                ZipCode = "12345"
            };

            PersonList.Add(FirstPerson);

            return PersonList;
        }
    }
}
