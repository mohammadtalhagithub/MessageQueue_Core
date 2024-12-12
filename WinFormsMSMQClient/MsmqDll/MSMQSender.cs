using MSMQ.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsmqDll
{
    public class MSMQSender
    {
        private static Queue<MsmqConfig> _queueMessages = new Queue<MsmqConfig>();
        //private static Queue<string> _queueMessages = new Queue<string>();

        /// <summary>
        /// Collection that contains the registered clients configurations.
        /// </summary>
        private static Dictionary<string, MsmqConfig> _dictMSMQ = new Dictionary<string, MsmqConfig>();

        static readonly object _lockObject = new object();

        public MSMQSender(MsmqConfig clientConfig)
        {
            string key = clientConfig.IpAddress + ":" + clientConfig.QueueName;
            if (clientConfig.Command != "unregister")
            {

                RegisterMachines(key, clientConfig);
                EnqueueMessages(clientConfig);
            }
        }

        /// <summary>
        /// Method to send message to multiple clients by accessing shared resources from ClientManagement class
        /// </summary>
        public void ProcessQueue()
        {
            // Dequeue items safely
            string message;
            lock (_lockObject)
            {
                while ((message = ClientManagement.DequeueMessages()) != null)
                {
                    // Process the dequeued message

                    foreach (var item in ClientManagement.GetMsmqConfigs())
                    {
                        //string queuePath = $@"FormatName:DIRECT=TCP:{_dictMSMQ[item].IpAddress}\Private$\{_dictMSMQ[key].QueueName}"; // 192.168.1.122
                    }


                }
            }
        }

        public string UnRegisterMachines(string key)
        {
            lock (_lockObject)
            {
                if (_dictMSMQ != null)
                {
                    if (_dictMSMQ.ContainsKey(key))
                    {
                        _dictMSMQ.Remove(key);
                        return "Unregistered successfully.";
                    }
                    return "Already unregistered.";
                }
                return "No client registered.";
            }
        }


        public string RegisterMachines(string key, MsmqConfig msmqConfig)
        {
            lock (_lockObject)
            {
                if (!_dictMSMQ.ContainsKey(key))
                {
                    _dictMSMQ.Add(key, msmqConfig);

                    return "Client registered";
                }

                return "Already registered";
            }
        }

        private void EnqueueMessages(MsmqConfig msmqConfig)
        {
            lock (_lockObject)
            {
                _queueMessages.Enqueue(msmqConfig);
                //_queueMessages.Enqueue(msmqConfig.Message);
            }
        }


        public string SendMessageToClients() // string message, string queueName
        {

            try
            {
                // needs double loops, one on queue, and other on dictionary
                lock (_lockObject)
                {
                    while (_queueMessages.Count > 0)
                    {
                        var message = _queueMessages.Dequeue();
                        //string message = _queueMessages.Dequeue();

                        foreach (var key in _dictMSMQ.Keys)
                        {
                            string queuePath = $@"FormatName:DIRECT=TCP:{_dictMSMQ[key].IpAddress}\Private$\{_dictMSMQ[key].QueueName}"; // 192.168.1.122
                            // OR 
                            using (MessageQueue queue = new MessageQueue(queuePath))
                            {

                                Message Message = new Message();
                                Message.Body = message.Message;
                                Message.Label = message.Label;

                                Message.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });

                                queue.Send(Message.Body, Message.Label);

                            }
                        }
                    }
                }

                return "messages sent to all clients.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
