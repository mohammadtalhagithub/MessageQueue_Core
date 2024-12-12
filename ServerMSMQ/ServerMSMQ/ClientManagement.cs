#if false
namespace ServerMSMQ
{
    public class ClientManagement
    {
        private static Queue<string> _queueMessages = new Queue<string>();

        private static Dictionary<string, MsmqConfig> _dictMSMQ = new Dictionary<string, MsmqConfig>();

        private static readonly object _lockObject = new object();

        public ClientManagement()
        {
            //MsmqConfig clientConfig
            //string key = clientConfig.IpAddress + ":" + clientConfig.QueueName;
            //if (clientConfig.Command != "unregister")
            //{
            //    //RegisterMachines(key, clientConfig);
            //    EnqueueMessages(clientConfig);
            //}
        }

        //public string UnRegisterMachines(string key)
        //{
        //    lock (_lockObject)
        //    {
        //        if (_dictMSMQ != null)
        //        {
        //            if (_dictMSMQ.ContainsKey(key))
        //            {
        //                _dictMSMQ.Remove(key);
        //                return "Unregistered successfully.";
        //            }
        //            return "Already unregistered.";
        //        }
        //        return "No client registered.";
        //    }
        //}

        // Method to safely remove an element from the dictionary by key
        public static bool UnregisterClient(string key)
        {
            lock (_lockObject)
            {
                if (_dictMSMQ.ContainsKey(key))
                {
                    // Remove the element safely
                    _dictMSMQ.Remove(key);
                    return true; // Return true if the element was successfully removed
                }
                return false; // Return false if the key does not exist
            }
        }

        public string RegisterClient(MsmqConfig clientConfig)
        {
            string key = clientConfig.IpAddress + ":" + clientConfig.QueueName;
            lock (_lockObject)
            {
                EnqueueMessages(clientConfig);

                if (!_dictMSMQ.ContainsKey(key))
                {
                    _dictMSMQ.Add(key, clientConfig);

                    return "Client registered";
                }

                return "Already registered";
            }
        }

        private void EnqueueMessages(MsmqConfig msmqConfig)
        {
            lock (_lockObject)
            {
                _queueMessages.Enqueue(msmqConfig.Message);
            }
        }



        /// <summary>
        /// Method to safely dequeue from the static queue
        /// </summary>
        /// <returns></returns>
        public static string DequeueMessages()
        {
            lock (_lockObject)
            {
                if (_queueMessages.Count > 0)
                {
                    return _queueMessages.Dequeue(); // Dequeue the item safely
                }
                return null; // Return null if the queue is empty
            }
        }

        /// <summary>
        /// Method to safely get all values from the dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, MsmqConfig> GetMsmqConfigs()
        {
            lock (_lockObject)
            {
                return new Dictionary<string, MsmqConfig>(_dictMSMQ); // shallow copy of configs
            }
        }


    }
}

#endif