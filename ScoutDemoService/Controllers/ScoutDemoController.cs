using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ScoutDemoService.Controllers
{
    public class ScoutDemoController : ApiController
    {
        public String Get()
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
            
            return JsonConvert.SerializeObject(PersonList);
        }
    }
}