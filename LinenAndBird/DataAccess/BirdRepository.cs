using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LinenAndBird.DataAccess
{
    public class BirdRepository
    {
        static List<Bird> _birds = new List<Bird>
        {
            new Bird
            {
                Id = Guid.NewGuid(),
                Name = "Jimmy",
                Color = "Red",
                Size = "Small",
                Type = BirdType.Dead,
                Accessories = new List<string> { "Beanie", "Gold wing tips" }
            }
        };

        //internal IEnumerable<Bird> GetAll()
        //{
        //    return _birds;
        //}

        internal IEnumerable<Bird> GetAll()
        {
            // connections are like the tunnel between our app and the database
            using var connection = new SqlConnection("Server=localhost;Database=LinenAndBird;Trusted_Connection=True;");

            // connections aren't open by default, we've gotta do that ourself
            connection.Open();

            // this tells SQL what we want to do
            var command = connection.CreateCommand(); // creating a command that is automatically being sent down the pipe
            command.CommandText = @"Select *
                                    From Birds";

            // Execute.Reader is for when we care about getting all the results of our query
            var reader = command.ExecuteReader();

            var birds = new List<Bird>();

            // data readers are weird, only get one row from the results at a time
            while(reader.Read())
            {
                var bird = new Bird();
                bird.Id = reader.GetGuid(0);
                bird.Size = reader["Size"].ToString(); // more readable but harder to work with
                bird.Type = (BirdType)reader["Type"];
                bird.Name = reader["Name"].ToString();
                bird.Color = reader["Color"].ToString();

                birds.Add(bird);
            }

            return birds;
        }

        internal void Add(Bird newBird)
        {
            newBird.Id = Guid.NewGuid();

            _birds.Add(newBird);
        }

        //internal Bird GetById(Guid birdId)
        //{
        //    return _birds.FirstOrDefault(bird => bird.Id == birdId);
        //}

        internal Bird GetById(Guid birdId)
        {
            using var connection = new SqlConnection("Server=localhost;Database=LinenAndBird;Trusted_Connection=True;");
            connection.Open();

            var command = connection.CreateCommand(); // creating a command that is automatically being sent down the pipe
            command.CommandText = $@"Select *
                                    From Birds
                                    Where id = @id";

            // parameterization prevents SQL injection (little bobby tables)
            command.Parameters.AddWithValue("id",birdId);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var bird = new Bird();
                bird.Id = reader.GetGuid(0);
                bird.Size = reader["Size"].ToString(); // more readable but harder to work with
                bird.Type = (BirdType)reader["Type"];
                bird.Name = reader["Name"].ToString();
                bird.Color = reader["Color"].ToString();

                return bird;
            }

            return null;
            //return _birds.FirstOrDefault(bird => bird.Id == birdId);
        }
    }
}
