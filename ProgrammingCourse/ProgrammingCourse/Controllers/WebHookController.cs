using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private const string VERIFY_TOKEN = "nttung";
        private const string PAGE_ACCESS_TOKEN = "EAAqn5V1o3dkBABriTtswMfDsaRGxS456SKLRrXE4I62bxjJfBIGwPlyMxqb8NcqUlMx9gLPBHdZCGZCIu2ynDEU5G4mxjAby2gbj8BbqO2biLS30Hn7EeJBBsiF52FtxZCP6fjvG4aJs7UZBpZCGFzPgxMhb6S4JVeLDq6RfZCfGahMxYouKtE";
        [HttpGet]
        public IActionResult Get()
        {
            string mode = Request.Query["hub.mode"];
            string token = Request.Query["hub.verify_token"];
            string challenge = Request.Query["hub.challenge"];

            Console.WriteLine(mode);
            Console.WriteLine(token);
            Console.WriteLine(challenge);

            if (mode != null && token != null)
            {
                if (mode == "subscribe" && token == VERIFY_TOKEN)
                {
                    Console.WriteLine("WEBHOOK_VERIFIED");
                    return Ok(challenge);
                }
            }

            return StatusCode(403);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();

                dynamic data = JObject.Parse(body);

                Console.WriteLine(data);

                if (data["object"] == "page")
                {
                    foreach (dynamic entry in data["entry"])
                    {
                        string sender_psid = entry["messaging"][0]["sender"]["id"];

                        //Message
                        foreach (JProperty variable in entry["messaging"][0])
                        {
                            if (variable.Name == "message")
                            {
                                await handleMessage(sender_psid, variable.Value);
                            }
                        }

                        //Postback
                        foreach (JProperty variable in entry["messaging"][0])
                        {
                            if (variable.Name == "postback")
                            {
                                await handlePostback(sender_psid, variable.Value);
                            }
                        }
                    }


                    return Ok("EVENT_RECEIVED");

                }
            }

            return NotFound();
        }


        //Handles messages events
        private async Task handleMessage(string sender_psid, dynamic received_message)
        {
            object response = new { text = "Hi" };

            //text
            //dynamic text = received_message.GetType().GetProperty("text").GetValue(received_message);


            if (received_message["text"] != null)
            {
                response = new
                {
                    text = $"You sent the message: {received_message["text"]}.Now send me an image!"
                };
            }


            //attachments
            //dynamic attachments = received_message.GetType().GetProperty("attachments").GetValue(received_message, null);

            if (received_message["attachments"] != null)
            {
                string attachment_url = received_message["attachments"][0]["payload"]["url"];

                response = new
                {
                    attachment = new
                    {
                        type = "template",
                        payload = new
                        {
                            template_type = "generic",
                            elements = new object[] {new
                            {
                                title = "Is this the right picture?",
                                subtitle = "Tap a button to answer.",
                                image_url = attachment_url,
                                buttons = new object[] {
                                    new
                                    {
                                        type = "postback",
                                        title = "Yes!",
                                        payload = "yes"
                                    },
                                    new
                                    {
                                        type = "postback",
                                        title = "No!",
                                        payload = "no"
                                    } }
                            } }
                        }
                    }
                };
            }

            await callSendAPI(sender_psid, response);
        }

        //Handles messaging_postbacks events
        private async Task handlePostback(string sender_psid, dynamic received_postback)
        {
            object response = new { text = "bye" };
            if(received_postback["payload"] == "get_started")
            {
                response = new
                {
                    attachment = new
                    {
                        type = "template",
                        payload = new
                        {
                            template_type = "button",
                            text = "What can I do to help you today?",
                            buttons = new object[]
                            {
                                new
                                {
                                    type = "postback",
                                    title = "Categories",
                                    payload = "categories"
                                },
                                new
                                {
                                    type = "postback",
                                    title = "Lookup Course",
                                    payload = "lookup_course"
                                },
                                new
                                {
                                    type = "postback",
                                    title = "Talk to an agent",
                                    payload = "talk_to_an_agent"
                                }
                            }
                        }
                    }
                };
            } 
            else if (received_postback["payload"] == "yes")
            {
                response = new { text = "Thanks!" };
            }
            else if (received_postback["payload"] == "no")
            {
                response = new { text = "Oops, try sending another image." };
            }
            await callSendAPI(sender_psid, response);
        }

        //Sends response messages via the send API
        private async Task callSendAPI(string sender_psid, object response)
        {
            var request_body = new
            {
                recipient = new
                {
                    id = sender_psid
                },
                message = response
            };

            Console.WriteLine(JsonConvert.SerializeObject(request_body));
            using (var httpClient = new HttpClient())
            {
                string uri = "https://graph.facebook.com/v2.6/me/messages";
                string requestUri = QueryHelpers.AddQueryString(uri, "access_token", PAGE_ACCESS_TOKEN);

                StringContent content = new StringContent(JsonConvert.SerializeObject(request_body), Encoding.UTF8, "application/json");

                var result = await httpClient.PostAsync(requestUri, content);

                Console.WriteLine(result);

            }
        }
    }
}
