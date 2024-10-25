﻿using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace TestASBConnection
{
    internal class AzureServiceBusSender
    {
        // Connection string to the Azure Service Bus namespace
        private readonly string _connectionString;
        // Name of the topic
        private readonly string _topicName;
       

        // Constructor for the AzureServiceBusSender class
        public AzureServiceBusSender(string connectionString, string topicName)
        {
            _connectionString = connectionString;
            _topicName = topicName;
        }

        // Method to send a message to the Azure Service Bus topic
        public async Task SendMessageAsync(string message)
        {        
            // Create a new ServiceBusClient object
            HttpClient client = new()
            {
                BaseAddress = new Uri(_connectionString),
            };
            // Sets authorization header as bearer
            client.DefaultRequestHeaders.Add("Authorization", "Bearer 3|qa4au1XriJ5wXJRRQi5kTfhJ4OUJfVuBCczVZQW23095ae57");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            
            // Creates a new HttpRequestMessage object
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _topicName);
            request.Content = new StringContent(message, Encoding.UTF8, "application/json");
            
            // Sends the message to the Azure Service Bus topic
            await client.SendAsync(request)
                .ContinueWith(responseTask =>
                {
                    Console.WriteLine("Response: {0}", responseTask.Result);
                });
        }
    }
}
