using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestASBConnection
{
    internal class SqlAccess
    {
        private readonly string _connectionString;
        
        public SqlAccess(string connectionString)
        {
            _connectionString = connectionString;
        }
        // This method is used to get the updates from the database
        public async Task<List<UpdateMessage>> GetSparePartUpdatesAsync(string XID, int batchSize, int offset)
        {
            // Create a list to store the updates
            List<UpdateMessage> updates = new List<UpdateMessage>();

            // Open a connection to the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Create a command to execute the stored procedure
                string commandText = $"exec [web].[ItemChanges] '{XID}'";
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    // Execute the command and read the results
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string item = reader.GetString(0);
                            string updateType = reader.GetString(1);
                            string department = reader.GetString(2);

                            // Add the update to the list
                            updates.Add(new UpdateMessage { item = item, updateType = updateType, department = department });

                           // Console.WriteLine(item, updateType, department);

                     
                        }

                    }
                }
            }

            return updates;
        }
    }
    // This class is used to store the updates
    public class UpdateMessage
    {
        public string item { get; set; }
        public string updateType { get; set; }

        public string department { get; set; }
    }
    
}
