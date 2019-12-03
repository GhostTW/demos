using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace http_client_core3
{
    class Program
    {
        static void Main(string[] args)
        {
            Test_Default().Wait();
            Test_Default_HttpRequestMessage().Wait();
            Test_Set_HttpRequestMessage_Version20().Wait();
        }

        private static async Task Test_Default()
        {
            Console.WriteLine($"{nameof(Test_Default)}");
            var client = new HttpClient();
            var result = await client.GetAsync("https://http2.pro/api/v1");
            Console.WriteLine($"receive response with http protocol {result.Version}");
            Console.WriteLine();
        }

        private static async Task Test_Default_HttpRequestMessage()
        {
            Console.WriteLine($"{nameof(Test_Default_HttpRequestMessage)}");
            var client = new HttpClient();
            Console.WriteLine($"{nameof(client)} version {client.DefaultRequestVersion}");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://http2.pro/api/v1");
            Console.WriteLine($"{nameof(httpRequestMessage)} version {httpRequestMessage.Version}");
            var result = await client.SendAsync(httpRequestMessage);
            Console.WriteLine($"receive response with http protocol {result.Version}");
            Console.WriteLine();
        }

        private static async Task Test_Set_HttpRequestMessage_Version20()
        {
            Console.WriteLine($"{nameof(Test_Set_HttpRequestMessage_Version20)}");
            var client = new HttpClient();
            Console.WriteLine($"{nameof(client)} version {client.DefaultRequestVersion}");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://http2.pro/api/v1") { Version = new Version(2, 0) };
            Console.WriteLine($"{nameof(httpRequestMessage)} version {httpRequestMessage.Version}");
            var result = await client.SendAsync(httpRequestMessage);
            Console.WriteLine($"receive response with http protocol {result.Version}");
            Console.WriteLine();
        }
    }
}
