using Azure.Messaging.ServiceBus.Administration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestASBConnection
{
    public class DataTransformer
    {
        // Method to convert a list of UpdateMessage objects to JSON
        public string ConvertToJson(object updates)
        {
            var batchObject = new { topic = "items", data = updates };
            // Formatting.Indented is used to make the JSON message more readable
            return JsonConvert.SerializeObject(batchObject, Formatting.Indented);
        }

        // Method to get the size of a JSON message in bytes
        public int GetMessageSizeInBytes(string jsonMessage)
        {
            return Encoding.UTF8.GetByteCount(jsonMessage);
        }

        // Overloaded method to get the size of a list of UpdateMessage objects in bytes
        public int GetMessageSizeInBytes(List<UpdateMessage> updates)
        {
            string jsonMessage = ConvertToJson(updates);
            return Encoding.UTF8.GetByteCount(jsonMessage);
        }
    }

}


