using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using NUnit.Framework;


namespace KPI_lab6
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            const string connectionString = "Server=127.0.0.1;Port=5432;Database=test6;User Id=postgres;Password=postgres;";
            const string query = "SELECT number, COUNT(*) from orders group by number order by number";
            var connection = new NpgsqlConnection(connectionString);
            var command = new NpgsqlCommand(query, connection);
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            var result = new List<object>();
            var expected = new List<object>();
            while (await reader.ReadAsync())
            {
                result.Add(new {Number = reader.GetValue(0).ToString(), Count = reader.GetValue(1).ToString()});
            }
            await reader.CloseAsync();
            await connection.CloseAsync();
            
            var csvreader = new StreamReader(@"C:\Users\onest\RiderProjects\KPI_lab6\KPI_lab6\test1.csv");
            while (!csvreader.EndOfStream)
            {
                var line = await csvreader.ReadLineAsync();
                var vals = line?.Split(",");
                expected.Add(new {Number = vals[0], Count = vals[1]});
            }
            csvreader.Close();
            for(var i = 0; i<result.Count; i++)
            {
                if (!Equals(result[i], expected[i]))
                {
                    await File.AppendAllTextAsync(@"C:\Users\onest\RiderProjects\KPI_lab6\KPI_lab6\log.txt",
                        $"{1+i} : {result[i]} ::: {expected[i]}");
                }
            }
            Assert.AreEqual(expected, result);
        }
        [Test]
        public async Task Test2()
        {
            var result = new List<Song>();
            var expected = new List<Song>();
            const string connectionString = "Server=127.0.0.1;Port=5432;Database=test6;User Id=postgres;Password=postgres;";
            const string query = "SELECT * FROM song";
            
            var connection = new NpgsqlConnection(connectionString);
            var command = new NpgsqlCommand(query, connection);
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new Song()
                {
                    Artist = reader.GetValue(0).ToString(), SongName = reader.GetValue(1).ToString(),
                    Duration = Convert.ToInt32(reader.GetValue(2))
                });
            }
            await reader.CloseAsync();
            await connection.CloseAsync();
            
            var csvreader = new StreamReader(@"C:\Users\onest\RiderProjects\KPI_lab6\KPI_lab6\test2.csv");
            while (!csvreader.EndOfStream)
            {
                var line = await csvreader.ReadLineAsync();
                var vals = line?.Split(",");
                expected.Add(new Song(){Artist = vals[0], SongName = vals[1],
                    Duration = Convert.ToInt32(vals[2])});
            }
            csvreader.Close();
            for(var i = 0; i<result.Count; i++)
            {
                if (!Equals(result[i], expected[i]))
                {
                    await File.AppendAllTextAsync(@"C:\Users\onest\RiderProjects\KPI_lab6\KPI_lab6\log.txt",
                        $"{1+i} : {result[i]} ::: {expected[i]}");
                }
            }
            Assert.AreEqual(expected, result);
        }
    }

}