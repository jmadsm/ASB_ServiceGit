using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestASBConnection
{
    internal class UpdateObject
    {

        // Properties of the UpdateObject class
        public string dealer_id = "";
        public int total = 0;
        // List of UpdateMessage objects
        public List<UpdateMessage> updates;
        // Constructor for the UpdateObject class
        public UpdateObject()
        {
            // Initialize the list of UpdateMessage objects
            updates = new List<UpdateMessage>();
        }
    }
}
