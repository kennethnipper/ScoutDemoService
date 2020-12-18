using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ScoutDemoService.Core.Models;

namespace ScoutDemoService.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoutDemoController : ControllerBase
    {
        private string ConnectionString = "Server=scoutdemoservice.db;Uid=root;Pwd=scoutdemopassword1234!;";
        /*
         * HttpPost in this format with Content-Type = application/json
         * https://localhost:44313/ScoutDemo
         * [{"firstName":"Bob","lastName":"Smith","emailAddress":"anyone@anywhere.com","address1":"123 Main St.","address2":"Apt 100","city":"Rome","state":"GA","zipCode":"12345"}]
         */
        [HttpPost]
        public IEnumerable<PersonModel> Index([FromBody] List<PersonModel> InputPeople)
        {
            List<PersonModel> People = new List<PersonModel>();
            foreach (PersonModel Person in InputPeople)
            {
                Boolean AlreadyExists = false;
                if (GetPersonByID(Person.ID) != null) { AlreadyExists = true; }
                Int32 PersonID = InsertPerson(Person, AlreadyExists);
                if (PersonID > 0)
                {
                    People.Add(GetPersonByID(PersonID));
                }
            }
            return People;
        }
        [HttpGet]
        public IEnumerable<PersonModel> Get(Int32? ID)
        {
            PopulateEmptyDB();
            List<PersonModel> PersonList = new List<PersonModel>();
            if (ID.HasValue)
            {
                PersonList.Add(GetPersonByID((Int32)ID));
            }
            else
            {
                PersonList = GetPeople();
            }
            return PersonList;
        }
        public void PopulateEmptyDB()
        {
            using (MySqlConnection Connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand Command = new MySqlCommand("CREATE SCHEMA IF NOT EXISTS scoutdemo;", Connection))
                {
                    if (Connection.State != System.Data.ConnectionState.Open) { Connection.Open(); }
                    Command.ExecuteNonQuery();
                }
                using (MySqlCommand Command = new MySqlCommand("CREATE TABLE IF NOT EXISTS scoutdemo.people(`id` int NOT NULL AUTO_INCREMENT, " +
                        "`firstname` varchar(45) DEFAULT NULL, " +
                        "`lastname` varchar(45) DEFAULT NULL, " +
                        "`emailaddress` varchar(45) DEFAULT NULL, " +
                        "`address1` varchar(45) DEFAULT NULL, " +
                        "`address2` varchar(45) DEFAULT NULL, " +
                        "`city` varchar(45) DEFAULT NULL, " +
                        "`state` varchar(45) DEFAULT NULL, " +
                        "`zipcode` varchar(45) DEFAULT NULL, " +
                        "PRIMARY KEY (`id`), " +
                        "UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE)", Connection))
                {
                    if (Connection.State != System.Data.ConnectionState.Open) { Connection.Open(); }
                    Command.ExecuteNonQuery();
                }
                using (MySqlCommand Command = new MySqlCommand("select count(id) from scoutdemo.people", Connection))
                {
                    if (Connection.State != System.Data.ConnectionState.Open) { Connection.Open(); }
                    if (Int32.Parse(Command.ExecuteScalar().ToString()) == 0)
                    {
                        InsertPerson(new PersonModel() { FirstName = "Bob", LastName = "Smith", EmailAddress = "anyone@anywhere.com", Address1 = "123 Main St.", Address2 = "Apt 100", City = "Rome", State = "GA", ZipCode = "12345" }, false);
                    }
                }
            }
        }
        public Int32 InsertPerson(PersonModel PM, Boolean AlreadyExists)
        {
            using (MySqlConnection Connection = new MySqlConnection(ConnectionString))
            {
                if (!AlreadyExists)
                {
                    using (MySqlCommand Command = new MySqlCommand("insert into scoutdemo.people(firstname, lastname, emailaddress, address1, address2, city, state, zipcode) values (@firstname, @lastname, @emailaddress, @address1, @address2, @city, @state, @zipcode);", Connection))
                    {
                        if (Connection.State != System.Data.ConnectionState.Open) { Connection.Open(); }
                        Command.Parameters.Add("@firstname", MySqlDbType.VarChar).Value = PM.FirstName;
                        Command.Parameters.Add("@lastname", MySqlDbType.VarChar).Value = PM.LastName;
                        Command.Parameters.Add("@emailaddress", MySqlDbType.VarChar).Value = PM.EmailAddress;
                        Command.Parameters.Add("@address1", MySqlDbType.VarChar).Value = PM.Address1;
                        Command.Parameters.Add("@address2", MySqlDbType.VarChar).Value = PM.Address2;
                        Command.Parameters.Add("@city", MySqlDbType.VarChar).Value = PM.City;
                        Command.Parameters.Add("@state", MySqlDbType.VarChar).Value = PM.State;
                        Command.Parameters.Add("@zipcode", MySqlDbType.VarChar).Value = PM.ZipCode;
                        Command.ExecuteNonQuery();
                        return (Int32)Command.LastInsertedId;
                    }
                }
                else
                {
                    using (MySqlCommand Command = new MySqlCommand("update scoutdemo.people set firstname = @firstname, lastname = @lastname, emailaddress = @emailaddress, address1 = @address1, address2 = @address2, city = @city, state = @state, zipcode = @zipcode where id = @id;", Connection))
                    {
                        if (Connection.State != System.Data.ConnectionState.Open) { Connection.Open(); }
                        Command.Parameters.Add("@id", MySqlDbType.Int32).Value = PM.ID;
                        Command.Parameters.Add("@firstname", MySqlDbType.VarChar).Value = PM.FirstName;
                        Command.Parameters.Add("@lastname", MySqlDbType.VarChar).Value = PM.LastName;
                        Command.Parameters.Add("@emailaddress", MySqlDbType.VarChar).Value = PM.EmailAddress;
                        Command.Parameters.Add("@address1", MySqlDbType.VarChar).Value = PM.Address1;
                        Command.Parameters.Add("@address2", MySqlDbType.VarChar).Value = PM.Address2;
                        Command.Parameters.Add("@city", MySqlDbType.VarChar).Value = PM.City;
                        Command.Parameters.Add("@state", MySqlDbType.VarChar).Value = PM.State;
                        Command.Parameters.Add("@zipcode", MySqlDbType.VarChar).Value = PM.ZipCode;
                        Command.ExecuteNonQuery();
                        return PM.ID;
                    }
                }
            }
        }
        public List<PersonModel> GetPeople()
        {
            List<PersonModel> People = new List<PersonModel>();
            using (MySqlConnection Connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand Command = new MySqlCommand("Select id, firstname, lastname, emailaddress, address1, address2, city, state, zipcode from scoutdemo.people", Connection))
                {
                    if (Connection.State != System.Data.ConnectionState.Open) { Connection.Open(); }
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PersonModel PM = new PersonModel()
                            {
                                ID = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32(reader.GetOrdinal("id")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("firstname")) ? string.Empty : reader.GetString(reader.GetOrdinal("firstname")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("lastname")) ? string.Empty : reader.GetString(reader.GetOrdinal("lastname")),
                                EmailAddress = reader.IsDBNull(reader.GetOrdinal("emailaddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("emailaddress")),
                                Address1 = reader.IsDBNull(reader.GetOrdinal("address1")) ? string.Empty : reader.GetString(reader.GetOrdinal("address1")),
                                Address2 = reader.IsDBNull(reader.GetOrdinal("address2")) ? string.Empty : reader.GetString(reader.GetOrdinal("address2")),
                                City = reader.IsDBNull(reader.GetOrdinal("city")) ? string.Empty : reader.GetString(reader.GetOrdinal("city")),
                                State = reader.IsDBNull(reader.GetOrdinal("state")) ? string.Empty : reader.GetString(reader.GetOrdinal("state")),
                                ZipCode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? string.Empty : reader.GetString(reader.GetOrdinal("zipcode")),
                            };
                            People.Add(PM);
                        }
                    }
                }
            }
            return People;
        }
        public PersonModel GetPersonByID(Int32 ID)
        {
            using (MySqlConnection Connection = new MySqlConnection(ConnectionString))
            {
                using MySqlCommand Command = new MySqlCommand("Select firstname, lastname, emailaddress, address1, address2, city, state, zipcode from scoutdemo.people where id = @id", Connection);
                if (Connection.State != System.Data.ConnectionState.Open) { Connection.Open(); }
                Command.Parameters.Add("@id", MySqlDbType.Int32).Value = ID;
                using MySqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    PersonModel PM = new PersonModel()
                    {
                        ID = ID,
                        FirstName = reader.IsDBNull(reader.GetOrdinal("firstname")) ? string.Empty : reader.GetString(reader.GetOrdinal("firstname")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("lastname")) ? string.Empty : reader.GetString(reader.GetOrdinal("lastname")),
                        EmailAddress = reader.IsDBNull(reader.GetOrdinal("emailaddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("emailaddress")),
                        Address1 = reader.IsDBNull(reader.GetOrdinal("address1")) ? string.Empty : reader.GetString(reader.GetOrdinal("address1")),
                        Address2 = reader.IsDBNull(reader.GetOrdinal("address2")) ? string.Empty : reader.GetString(reader.GetOrdinal("address2")),
                        City = reader.IsDBNull(reader.GetOrdinal("city")) ? string.Empty : reader.GetString(reader.GetOrdinal("city")),
                        State = reader.IsDBNull(reader.GetOrdinal("state")) ? string.Empty : reader.GetString(reader.GetOrdinal("state")),
                        ZipCode = reader.IsDBNull(reader.GetOrdinal("zipcode")) ? string.Empty : reader.GetString(reader.GetOrdinal("zipcode")),
                    };
                    return PM;
                }
            }
            return null;
        }
    }
}
