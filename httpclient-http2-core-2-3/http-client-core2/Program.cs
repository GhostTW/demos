using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace http_client_core22
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(http_client_core22)}");
            //AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false); // turn http2 on !
            Test_Default().Wait();
            Test_Default_HttpRequestMessage().Wait();
            Test_Set_HttpRequestMessage_HttpVersion20().Wait();
        }

        private static async Task Test_Default()
        {
            Console.WriteLine($"{nameof(Test_Default)}");
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("https://http2.pro/api/v1");
                Console.WriteLine($"receive response with http protocol {result.Version}");
            }
            Console.WriteLine();
        }

        private static async Task Test_Default_HttpRequestMessage()
        {
            Console.WriteLine($"{nameof(Test_Default_HttpRequestMessage)}");
            using (var client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://http2.pro/api/v1");
                Console.WriteLine($"{nameof(httpRequestMessage)} version {httpRequestMessage.Version}");
                var result = await client.SendAsync(httpRequestMessage);
                Console.WriteLine($"receive response with http protocol {result.Version}");
            }
            Console.WriteLine();
        }

        private static async Task Test_Set_HttpRequestMessage_HttpVersion20()
        {
            Console.WriteLine($"{nameof(Test_Set_HttpRequestMessage_HttpVersion20)}");
            using (var client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://http2.pro/api/v1") { Version = System.Net.HttpVersion.Version20 };
                Console.WriteLine($"{nameof(httpRequestMessage)} version {httpRequestMessage.Version}");
                var result = await client.SendAsync(httpRequestMessage);
                Console.WriteLine($"receive response with http protocol {result.Version}");
            }
            Console.WriteLine();
        }

        private static async Task Test_Set_HttpRequestMessage_Version20()
        {
            Console.WriteLine($"{nameof(Test_Set_HttpRequestMessage_Version20)}");
            using (var client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://http2.pro/api/v1") { Version = new Version(2, 0) };
                Console.WriteLine($"{nameof(httpRequestMessage)} version {httpRequestMessage.Version}");
                var result = await client.SendAsync(httpRequestMessage);
                Console.WriteLine($"receive response with http protocol {result.Version}");
            }
            Console.WriteLine();
        }
    }
}
//https://stackoverflow.com/questions/53764083/use-http-2-with-httpclient-in-net