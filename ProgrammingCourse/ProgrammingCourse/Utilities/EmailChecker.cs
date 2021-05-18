using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProgrammingCourse.Utilities
{
    public class EmailChecker
    {
        public static async Task<bool> Check(string email)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://truemail.io/api/v1/verify/single?address_info=1&timeout=100&access_token=FlEhq9n0aM6uAtI5gudJRJMERA9peDIqXscJTjQsEVHtTUsu4ZTBocGKZES8thKJ&email={email}"),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var bodyObject = JObject.Parse(body);

                if(bodyObject["result"].ToString() == "valid")
                {
                    return true;
                }

                if (bodyObject["result"].ToString() == "invalid")
                {
                    return false;
                }
            }

            return false;
        }
    }
}
