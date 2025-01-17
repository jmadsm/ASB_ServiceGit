using Azure.Messaging.ServiceBus;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestASBConnection
{   
    public class Program
    {
        public static int ExitStatus = 0;
        // Main method
        private static async Task<int> Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the XID as an argument.");
                return 1; // Exit code 1 indicates a missing argument
            }
            // Arguments
            string xid = args[0];
            // Connection strings
            string sqlConnectionString = Properties.ApplicationsSetting.Default.SqlConnection; 
            string asbConnectionString = Properties.ApplicationsSetting.Default.ApiEndPoint ;
            string topicName = "items";
            // Json Batch size
            int batchSize = 2048;

            try
            {
                // Data Access Layer
                SqlAccess dataAccess = new SqlAccess(sqlConnectionString);
                DataTransformer transformer = new DataTransformer();
                AzureServiceBusSender sender = new AzureServiceBusSender(asbConnectionString, topicName);

                int offset = 0;
                List<UpdateMessage> updates;

                // Fetch a batch of updates
                UpdateObject newUpdate = new UpdateObject();
                newUpdate.dealer_id = xid;
                updates = await dataAccess.GetSparePartUpdatesAsync(xid, batchSize, offset);
                List<UpdateMessage> updates_batch = new List<UpdateMessage>();
                int count = 0;

                // Send the updates in batches
                foreach (UpdateMessage upm in updates)
                {
                    count++;
                    updates_batch.Add(upm);
                    if (count % batchSize == 0)
                    {
                        newUpdate.total = (count % batchSize == 0 ? batchSize : count % batchSize);
                        newUpdate.updates = updates_batch;
                        await SendNewUpdate(newUpdate);
                        updates_batch = new List<UpdateMessage>();
                    }
                }
                //Batch size 1000 entries
                newUpdate.total = (count % batchSize == 0 ? batchSize : count % batchSize);
                newUpdate.updates = updates_batch;
                if (newUpdate.total != 0)
                    await SendNewUpdate(newUpdate);

                return ExitStatus; // Return the total number of records sent as the exit code
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return -1; // Exit code -1 indicates an error
            }
            //Connects to the JMA Webparts API and Converts the update into Json.
            static async Task SendNewUpdate(object updates)
            {
                string asbConnectionString = Properties.ApplicationsSetting.Default.ApiEndPoint;
                string topicName = "items";
                DataTransformer transformer = new DataTransformer();
                AzureServiceBusSender sender = new AzureServiceBusSender(asbConnectionString, topicName);
                // Convert to JSON
                string jsonMessage = transformer.ConvertToJson(updates);
                int messageSize = transformer.GetMessageSizeInBytes(jsonMessage);
                // Send message
                await sender.SendMessageAsync(jsonMessage);
            }




        }
    }

}