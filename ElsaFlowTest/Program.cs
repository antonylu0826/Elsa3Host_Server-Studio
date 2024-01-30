using System.Text;

namespace ElsaFlowTest
{
    internal class Program
    {
        const string BaseAddress = "https://localhost:5001/workflows/";
        static async Task Main(string[] args)
        {
            await PostToWorkflow(BaseAddress + "users");

            Console.ReadLine();
        }

        async static Task PostToWorkflow(string url)
        {
            string data = @"{
""id"": 1,
""email"": ""bob@acme.com"",
""name"": ""Bob Jones""
}";
            await PostJsonAsync(url, data);
        }

        static async Task PostJsonAsync(string apiUrl, string jsonData)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the content type to application/json
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Create the content as a StringContent object with UTF-8 encoding
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    // Send the POST request to the API endpoint
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("POST request successful!");
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"POST request failed with status code {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }


    
}
