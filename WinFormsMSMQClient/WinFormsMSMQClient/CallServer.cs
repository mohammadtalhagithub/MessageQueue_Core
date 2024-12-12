using MsmqDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace WinFormsMSMQClient
{
    public class CallServer
    {
        private static readonly string _localIpAddress = GetLocalMachineIP();


        private static readonly string _serverBaseUrl = "192.168.1.165:5081"; // 192.168.0.103:5081

        public bool RegisterQueueOnServer(string QueueName) //MsmqConfig msmqConfig)
        {
            MsmqConfig msmqConfig = new MsmqConfig()
            {
                Command = "REGISTER",
                IpAddress = _localIpAddress,
                Message = string.Empty,
                Label = string.Empty,
                QueueName = QueueName,
                Type = string.Empty
            };

            string jsonPayload = JsonConvert.SerializeObject(msmqConfig);

            string url = $"http://{_serverBaseUrl}/MSMQServer/RegisterQueueOnServer";
            var client = new RestClient(url);
            var restRequest = new RestRequest(url, Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jsonPayload, ParameterType.RequestBody);

            //RestResponse restResponse = await client.ExecuteAsync(restRequest);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                var respContent = restResponse.Content;
                Console.WriteLine(respContent);

                return true;
            }
            else
            {
                Console.WriteLine("Error from Server.");
                return false;
            }
        }

        public async Task<bool> CallCommonService(string message, string label, string queueName, string command) //, string queueAddress)
        {
            MsmqConfig payload = new MsmqConfig()
            {
                IpAddress = _localIpAddress,
                Message = message,
                Label = label,
                QueueName = queueName,
                Command = command,
                Type = ""
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            string url = $"http://{_serverBaseUrl}/MSMQServer/CommonServiceMsmq"; // ?message={message}&queueName={queueName}&command={command}
            var client = new RestClient(url);
            var restRequest = new RestRequest(url, Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jsonPayload, ParameterType.RequestBody);
            RestResponse restResponse = await client.ExecuteAsync(restRequest);
            if (restResponse.IsSuccessful)
            {
                var respContent = restResponse.Content;
                Console.WriteLine(respContent);

                return true;
            }
            else
            {
                Console.WriteLine("Error from Server.");
                return false;
            }
        }


        public async Task<bool> UnRegisterClient(string queueName, string command)
        {
            MsmqConfig payload = new MsmqConfig()
            {
                IpAddress = _localIpAddress,
                Message = string.Empty,
                Label = string.Empty,
                QueueName = queueName,
                Command = command,
                Type = ""
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            string url = $"http://{_serverBaseUrl}/MSMQServer/CommonServiceMsmq";
            var client = new RestClient(url);
            var restRequest = new RestRequest(url, Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jsonPayload, ParameterType.RequestBody);
            RestResponse restResponse = await client.ExecuteAsync(restRequest);
            if (restResponse.IsSuccessful)
            {
                var respContent = restResponse.Content;
                Console.WriteLine(respContent);

                return true;
            }
            else
            {
                Console.WriteLine("Error from Server.");
                return false;
            }
        }

        static string GetLocalMachineIP()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName); ;
                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
