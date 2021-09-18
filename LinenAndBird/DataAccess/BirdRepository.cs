using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;

namespace LinenAndBird.DataAccess
{
    public class BirdRepository
    {
        const string _connectionString = "Server=localhost;Database=LinenAndBird;Trusted_Connection=True;";

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
            using var db = new SqlConnection(_connectionString);

            // Query<T> is for getting results from the database and putting them into a C# type
            db.Query<Bird>(@"Select * From Birds");

            return birds;

            
            
            // Execute.Reader is for when we care about getting all the results of our query
            //var reader = command.ExecuteReader();

            //var birds = new List<Bird>();

            // data readers are weird, only get one row from the results at a time
            //while(reader.Read())
            //{
            //    var bird = new Bird();
            //    bird.Id = reader.GetGuid(0);
            //    bird.Size = reader["Size"].ToString(); // more readable but harder to work with
            //    bird.Type = (BirdType)reader["Type"];
            //    bird.Name = reader["Name"].ToString();
            //    bird.Color = reader["Color"].ToString();

            //    birds.Add(bird);
            //}

            //return birds;
        }

        internal object Update(Guid id, Bird bird)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"Update Birds
                        Set Color = @color,
                            Name = @name,
                            Type = @type
                            Size = @size,
                        output inserted.*
                        Where id = @id";

            bird.Id = id;
            var updatedBird = db.QuerySingleOrDefault(sql, bird);
            return updatedBird;

            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText = @"Update Birds
            //                      Set Color = @color,
            //                          Name = @name,
            //                          Type = @type
            //                          Size = @size,
            //                      output inserted.*
            //                      Where id = @id";

            //command.Parameters.AddWithValue("Type", bird.Type);
            //command.Parameters.AddWithValue("Color", bird.Color);
            //command.Parameters.AddWithValue("Size", bird.Size);
            //command.Parameters.AddWithValue("Name", bird.Name);
            //command.Parameters.AddWithValue("id", id);

            //var reader = command.ExecuteReader();

            //if (reader.Read())
            //{
            //    var updatedBird = new Bird();
            //    updatedBird.Id = reader.GetGuid(0);
            //    updatedBird.Size = reader["Size"].ToString(); // more readable but harder to work with
            //    updatedBird.Type = (BirdType)reader["Type"];
            //    updatedBird.Name = reader["Name"].ToString();
            //    updatedBird.Color = reader["Color"].ToString();

            //    return updatedBird;
            //}

            //return null;
        }

        internal void Remove(Guid id)
        {
            using var db = new SqlConnection(_connectionString);
            var sql = @"Delete
                                   From Birds
                                   Where Id = @id";

            db.Execute(sql, new { id = id } ); // or can use { id } because they match


            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText = @"Delete
            //                       From Birds
            //                       Where Id = @id";
            //command.Parameters.AddWithValue("id", id);

            //command.ExecuteNonQuery();
        }

        //internal void Add(Bird newBird)
        //{
        //    newBird.Id = Guid.NewGuid();

        //    _birds.Add(newBird);
        //}

        internal void Add(Bird newBird)
        {
            using var db = new SqlConnection(_connectionString);

            var sql = @"insert into birds(Type,Color,Size,Name)
                                    output inserted.Id
                                    values (@Type,@Color,@Size,@Name)";

            var id = db.ExecuteScalar<Guid>(sql, newBird);
            newBird.Id = id;



            //connection.Open();

            //var command = connection.CreateCommand();
            //command.CommandText = @"insert into birds(Type,Color,Size,Name)
            //                        output inserted.Id
            //                        values (@Type,@Color,@Size,@Name)";

            //command.Parameters.AddWithValue("Type", newBird.Type);
            //command.Parameters.AddWithValue("Color", newBird.Color);
            //command.Parameters.AddWithValue("Size", newBird.Size);
            //command.Parameters.AddWithValue("Name", newBird.Name);

            //// execute the query, but don't care about the results, just number of rows
            ////var numberOfRowsAffected = command.ExecuteNonQuery();

            //var numberOfRowsAffected = (Guid) command.ExecuteScalar();
        }

        //internal Bird GetById(Guid birdId)
        //{
        //    return _birds.FirstOrDefault(bird => bird.Id == birdId);
        //}

        internal Bird GetById(Guid birdId)
        {
            using var db = new SqlConnection(_connectionString);
            
            var sql = @"Select * From Birds where id = @id";

            var bird = db.QuerySingleOrDefault<Bird>(sql, new {id = birdId } );

            return bird;
            
            
            //connection.Open();

            //var command = connection.CreateCommand(); // creating a command that is automatically being sent down the pipe
            //command.CommandText = $@"Select *
            //                        From Birds
            //                        Where id = @id";

            //// parameterization prevents SQL injection (little bobby tables)
            //command.Parameters.AddWithValue("id",birdId);

            //var reader = command.ExecuteReader();

            //if (reader.Read())
            //{
            //    var bird = new Bird();
            //    bird.Id = reader.GetGuid(0);
            //    bird.Size = reader["Size"].ToString(); // more readable but harder to work with
            //    bird.Type = (BirdType)reader["Type"];
            //    bird.Name = reader["Name"].ToString();
            //    bird.Color = reader["Color"].ToString();

            //    return bird;
            //}

            //return null;
            //return _birds.FirstOrDefault(bird => bird.Id == birdId);
        }

        
        // DAPPER IS DOING THIS FOR US BELOW
        //Bird MapFromReader(SqlDataReader reader)
        //{
        //    var bird = new Bird();
        //    bird.Id = reader.GetGuid(0);
        //    bird.Size = reader["Size"].ToString();
        //    bird.Type = (BirdType)reader["Type"];
        //    bird.Color = reader["Color"].ToString();
        //    bird.Name = reader["Name"].ToString();

        //    return bird;
        //}
    }
}
