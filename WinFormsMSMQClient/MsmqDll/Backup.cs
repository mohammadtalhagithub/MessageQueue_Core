using MSMQ.Messaging;
using System;

public class MsmqReceiverV1
{
	private readonly string _queuePath;
	private MessageQueue _messageQueue;

	// Event to expose the received message
	public event EventHandler<MessageReceivedEventArgs> MessageReceived;

	public MsmqReceiverV1(string queuePath)
	{
		_queuePath = queuePath;
		InitializeQueue();
	}

	private void InitializeQueue()
	{
		// Ensure the queue exists
		if (!MessageQueue.Exists(_queuePath))
		{
			MessageQueue.Create(_queuePath);
		}

		// Initialize the MessageQueue instance
		_messageQueue = new MessageQueue(_queuePath)
		{
			Formatter = new XmlMessageFormatter(new[] { "System.String" }) // Adjust formatter as needed
		};

		// Attach internal ReceiveCompleted handler
		_messageQueue.ReceiveCompleted += OnReceiveCompleted;
	}

	/// <summary>
	/// Starts asynchronous message receiving.
	/// </summary>
	public void StartReceiving()
	{
		_messageQueue?.BeginReceive();
	}

	/// <summary>
	/// Stops receiving messages and releases resources.
	/// </summary>
	public void StopReceiving()
	{
		if (_messageQueue != null)
		{
			_messageQueue.ReceiveCompleted -= OnReceiveCompleted;
			_messageQueue.Close();
			_messageQueue.Dispose();
			_messageQueue = null;
		}
	}

	private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
	{
		try
		{
			if (_messageQueue != null)
			{
				// Complete the asynchronous receive operation
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
}

/// <summary>
/// Event arguments for the MessageReceived event.
/// </summary>
public class MessageReceivedEventArgs : EventArgs
{
	public string MessageBody { get; set; }
	public string Label { get; set; }
}



#if false
            //txtBxQueueName.Text;// "WinForm1";
            _mSMQReceiver = new MSMQReceiver(_queueName);

            //_mSMQSender = new MSMQSender(_queueName);


            // Call function to send MSMQ configuration to Web server for queue registration such as some Asp.net core service
            CallServer server = new CallServer();
            bool bResponse = server.RegisterQueueOnServer(_queueName);

            MsmqConfig msmqConfig = new MsmqConfig();
            msmqConfig.QueueName = _queueName;

            _mSMQReceiver.MessageReceived += (s, e) =>
            {
                // anonymous event handler
                if (this.InvokeRequired)
                {
                    this.Invoke((Action)(() =>
                    {
                        listBoxMessages.Items.Add($"Message: {e.MessageBody}, Label: {e.Label}");
                    }));

                    //this.Invoke((MethodInvoker)(() =>
                    //{
                    //    listBoxMessages.Items.Add($"Message: {e.MessageBody}, Label: {e.Label}");

                    //}));
                }
                else
                {
                    listBoxMessages.Items.Add($"Message: {e.MessageBody}, Label: {e.Label}");
                }
            };
            _mSMQReceiver.StartReceiving(); 
#endif



#if false
                    if (this.InvokeRequired)
                    {
                        this.Invoke((Action)(() =>
                        {
                            listBoxMessages.Items.Add($"Message: {e.MessageBody}, Label: {e.Label}");
                        }));

                        //this.Invoke((MethodInvoker)(() =>
                        //{
                        //    listBoxMessages.Items.Add($"Message: {e.MessageBody}, Label: {e.Label}");

                        //}));
                    }
                    else
                    {
                        listBoxMessages.Items.Add($"Message: {e.MessageBody}, Label: {e.Label}");
                    } 
#endif