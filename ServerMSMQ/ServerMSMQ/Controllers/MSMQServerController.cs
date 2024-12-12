using Microsoft.AspNetCore.Mvc;
using MsmqDll;

namespace ServerMSMQ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MSMQServerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly ClientManagement _clientsManagement;// = new ClientManagement();

        public MSMQServerController(ClientManagement clientsManagement)
        {
            _clientsManagement = clientsManagement;

        }
        // var remoteIpAddress = HttpContext?.Connection?.RemoteIpAddress?.ToString();
        [HttpPost("RegisterQueueOnServer")]
        public IActionResult RegisterQueueOnServer(MsmqConfig clientConfig)
        {
            try
            {
                string sOutput = string.Empty;
                _clientsManagement.RegisterClient(clientConfig);
                
                sOutput = "test ClientManagement";





#if true
                MSMQSender senderMSMQ = new MSMQSender(clientConfig);

                sOutput = senderMSMQ.RegisterMachines(clientConfig.IpAddress + ":" + clientConfig.QueueName, clientConfig); 
#endif

                var objOK = new
                {
                    status = 200,
                    message = sOutput
                };

                return Ok(objOK);
            }
            catch (Exception ex)
            {
                var objOK = new
                {
                    status = 500,
                    message = ex.Message
                };

                return null;
            }
        }


        // MSMQServerController
        [HttpPost("CommonServiceMsmq")]
        public IActionResult CommonServiceMsmq(MsmqConfig clientConfig) //, string queueAddress)
        {
            try
            {
                string sOutput = string.Empty;
                // Code for Msmq
                //var remoteIpAddress = HttpContext?.Connection?.RemoteIpAddress?.ToString();


#if true
                MSMQSender sender = new MSMQSender(clientConfig);

                sender.ProcessQueue(); 
#endif

                MSMQSender senderMSMQ = new MSMQSender(clientConfig);


                sOutput = senderMSMQ.SendMessageToClients();

                var objOK = new
                {
                    status = 200,
                    message = sOutput
                };

                return Ok(objOK);
            }
            catch (Exception ex)
            {
                var objOK = new
                {
                    status = 500,
                    message = ex.Message
                };

                return null;
            }
        }

    }
}
