using MSMQ.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsmqDll
{
    public class MSMQReceiver
    {
        private readonly string _queuePath;// = @".\Private$\{0}"; // App1
        private MessageQueue _messageQueue;

        /// <summary>
        /// Custom event to expose the received message outside this class
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public MSMQReceiver(string queueName)//, QueueType queueType)
        {
            _queuePath = $@".\Private$\{queueName}";

            CreateQueue();
        }

        /// <summary>
        /// Returns true if queue was present and this function has deleted it.
        /// </summary>
        /// <returns></returns>
        public bool DeleteQueue()
        {
            try
            {
                if (!MessageQueue.Exists(_queuePath))
                {
                    Console.WriteLine("Queue does not exist.");
                    return false;
                }

                MessageQueue.Delete(_queuePath); // else delete if exists
                Console.WriteLine("Queue was present, now deleted.");

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns true if queue was not present, and function created it.
        /// </summary>
        /// <param name="QueuePath"></param>
        /// <returns></returns>
        public bool CreateQueue()
        {
            try
            {
                bool bOutput = false;
                DeleteQueue();
                if (!MessageQueue.Exists(_queuePath))
                {
                    Console.WriteLine("Queue does not exist. Creating it...");
                    MessageQueue.Create(_queuePath);
                    //ReceiveMessage(_QueuePath);
                    bOutput = true;
                }
                _messageQueue = new MessageQueue(_queuePath)
                {
                    Formatter = new XmlMessageFormatter(new[] { "System.String" })
                };

                _messageQueue.ReceiveCompleted += OnReceiveCompleted;
                Console.WriteLine("Queue already exists.");
                bOutput = false;

                return bOutput;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        /// <summary>
        /// Starts asynchronous message receiving.
        /// </summary>
        public void StartReceiving()
        {
            _messageQueue?.BeginReceive();
        }

        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                if (_messageQueue != null)
                {
                    // Complete the asynchronous receive operation that was started by StartReceiving method
                    var message = _messageQueue.EndReceive(e.AsyncResult);

                    // Raise the external event
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs
                    {
                        MessageBody = message.Body?.ToString(),
                        Label = message.Label
                    });

                    // Restart asynchronous receiving
                    _messageQueue.BeginReceive();
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_messageQueue != null)
            {
                _messageQueue.Close();
                _messageQueue.Dispose();
                _messageQueue = null;
            }

            DeleteQueue();
        }
    }

    // <summary>
    // Event arguments for the MessageReceived event.
    // </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        public string MessageBody { get; set; }
        public string Label { get; set; }
    }
}
