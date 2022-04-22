using System.Data;
using System.Data.SqlClient;
using Word_Processor_Load.Model;

namespace Word_Processor_Load
{
    internal class Provider
    {
        private readonly string connectionString = @"Data Source=DESKTOP-RL27D3Q;Initial Catalog=test;Integrated Security=True";

        public void Import(IEnumerable<Word> words)
        {
            using (var sConn = new SqlConnection(connectionString))
            {
                sConn.Open();
                using (var transaction = sConn.BeginTransaction())
                {
                    using var existCommand = GetExistCommand(sConn, transaction);
                    using var insertCommand = GetInsertCommand(sConn, transaction);
                    using var updateCommand = GetUpdateCommand(sConn, transaction);

                    foreach (var word in words)
                    {
                        var wordExist = ParameterizeAndExecuteExistCommand(existCommand, word);

                        if (wordExist)
                        {
                            ParameterizeAndExecuteUpdateCommand(updateCommand, word);
                        }
                        else
                        {
                            ParameterizeAndExecuteInsertCommand(insertCommand, word);
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        private static SqlCommand GetExistCommand(SqlConnection sConn, SqlTransaction transaction)
        {
            var existCommand = sConn.CreateCommand();
            existCommand.Transaction = transaction;
            existCommand.CommandText = @"
select count(word)
from words
where word = @word;
";
            existCommand.Parameters.Add(new SqlParameter("@word", SqlDbType.NVarChar, 20));
            existCommand.Prepare();
            return existCommand;
        }

        private static SqlCommand GetInsertCommand(SqlConnection sConn, SqlTransaction transaction)
        {
            var insertCommand = sConn.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = @"
insert into words (word, mentions) values 
(@word, @mentions);
";
            insertCommand.Parameters.Add(new SqlParameter("@word", SqlDbType.NVarChar, 20));
            insertCommand.Parameters.Add("@mentions", SqlDbType.Int);
            insertCommand.Prepare();
            return insertCommand;
        }

        private static SqlCommand GetUpdateCommand(SqlConnection sConn, SqlTransaction transaction)
        {
            var updateCommand = sConn.CreateCommand();
            updateCommand.Transaction = transaction;
            updateCommand.CommandText = @"
update words
set mentions = mentions + @mentions
where word = @word;
";
            updateCommand.Transaction = transaction;
            updateCommand.Parameters.Add(new SqlParameter("@word", SqlDbType.NVarChar, 20));
            updateCommand.Parameters.Add("@mentions", SqlDbType.Int);
            updateCommand.Prepare();
            return updateCommand;
        }

        private static bool ParameterizeAndExecuteExistCommand(SqlCommand existCommand, Word word)
        {
            existCommand.Parameters["@word"].Value = word.Name;
            var exist = (int)existCommand.ExecuteScalar() > 0;
            return exist;
        }

        private static void ParameterizeAndExecuteInsertCommand(SqlCommand insertCommand, Word word)
        {
            insertCommand.Parameters["@word"].Value = word.Name;
            insertCommand.Parameters["@mentions"].Value = word.Mentions;
            insertCommand.ExecuteNonQuery();
        }

        private static void ParameterizeAndExecuteUpdateCommand(SqlCommand updateCommand, Word word)
        {
            updateCommand.Parameters["@word"].Value = word.Name;
            updateCommand.Parameters["@mentions"].Value = word.Mentions;
            updateCommand.ExecuteNonQuery();
        }
    }
}
