using Microsoft.Data.SqlClient;

namespace Word_Processor_Read
{
    internal class Provider
    {
        private readonly string connectionString = @"Data Source=DESKTOP-RL27D3Q;Initial Catalog=test;Integrated Security=True";

        public List<string> Read(string prefix)
        {
            var Words = new List<string>();
            using (var sConn = new SqlConnection(connectionString))
            {
                sConn.Open();

                using (var command = sConn.CreateCommand())
                {
                    command.CommandText = @"
select top(5) word
from words
where word like @prefix order by mentions desc, word asc;";
                    command.Parameters.AddWithValue("@prefix", prefix + "%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Words.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return Words;
        }
    }
}
