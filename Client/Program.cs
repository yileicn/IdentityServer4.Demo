using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",

                Scope = "api1.read"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            apiClient.BaseAddress = new Uri("http://localhost:5001/");
            //1.调用读接口(Scope控制)
            var response = await apiClient.GetAsync("api/values");
            Output(response);
            //2.调用写接口(Scope控制)
            response = await apiClient.DeleteAsync("api/values/1");
            Output(response);
            //3.通过管理员接口(角色控制)
            response = await apiClient.GetAsync("api/admin");
            Output(response);

            Console.ReadKey();
        }

        public static async void Output(HttpResponseMessage response)
        {
            Console.WriteLine("开始输出结果：");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Result:"+content);
            }
        }
    }
}
