using MsmqDll;
using System.Diagnostics;

namespace WinFormsMSMQClient
{
    public partial class Form1 : Form
    {
        private readonly string _queueName;

        MSMQReceiver _mSMQReceiver;
        //MSMQSender _mSMQSender;


        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            _queueName = "winform_1";

            InitializeMsmq();
        }

        /// <summary>
        /// Method to initialize the resources to receive messages and register client on server.
        /// </summary>
        private void InitializeMsmq()
        {
            try
            {
                _mSMQReceiver = new MSMQReceiver(_queueName);

                //_mSMQSender = new MSMQSender(_queueName);

                CallServer server = new CallServer();
                // Call function to send MSMQ configuration to Web server for queue registration such as some Asp.net core service
                bool bResponse = server.RegisterQueueOnServer(_queueName);

                MsmqConfig msmqConfig = new MsmqConfig();
                msmqConfig.QueueName = _queueName;

                _mSMQReceiver.MessageReceived += (s, e) =>
                {
                    // anonymous event handler

                    Task.Run(() =>
                    {
                        UI_Update(e.MessageBody, e.Label);
                    });


                };
                _mSMQReceiver.StartReceiving();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in app closing: " + ex.Message);
            }
        }


        private void UI_Update(string message, string label)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((Action)(() =>
                    {
                        listBoxMessages.Items.Add($"Message: {message}, Label: {label}");
                    }));
                }
                else
                {
                    listBoxMessages.Items.Add($"Message: {message}, Label: {label}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in app closing: " + ex.Message);
            }
        }


        /// <summary>
        /// Send the message on button click, to the server for distribution to multiple registered clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                CallServer server = new CallServer();
                server.CallCommonService(txtBxMessage.Text, txtBxLabel.Text, _queueName, "message");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in app closing: " + ex.Message);
            }
        }

        /// <summary>
        /// Before application closes, handle resource cleanup, clients unregistration, and delete the local MSMQ created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _mSMQReceiver?.Dispose();

                // Unregister the client machine from server collection (pending call)

                _mSMQReceiver.DeleteQueue();// Delete the MSMQ from client machine
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in app closing: " + ex.Message);
            }


        }


    }
}
