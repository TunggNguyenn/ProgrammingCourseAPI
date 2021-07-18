using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProgrammingCourse.Repositories;
using ProgrammingCourse.Services;
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
        private const string PAGE_ACCESS_TOKEN = "EAAqn5V1o3dkBAObZBjdjBdJFjIpNQJDnSytmMeLZAfHZC6l13ytzLakiLiAhdyw4w7XvI3EhsotRyItoXTZCxQ7tuNDDOuKWVxK9PoSPxujO5p1RWdRyAZAVBnZABam7LwRZCv46rdQN5sdTFl2MEH0ZARhzL6mZCYRmWZANPtYZBfsMKzSSQMjxZBa2";

        private CategoryService categoryService;
        private CourseService courseService;

        public WebHookController(CategoryService categoryService, CourseService courseService)
        {
            this.categoryService = categoryService;
            this.courseService = courseService;
        }

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

        [HttpPost]
        [Route("FindCourse")]
        public async Task<IActionResult> FindCourse([FromForm] string psid, [FromForm] string keywords)
        {
            object response = new { text = "Finding..." };

            if (keywords != "")
            {
                var course = await courseService.FindCourse(keywords);

                if (course == null)
                {
                    response = new
                    {
                        text = "I don't find any courses matching your key words!"
                    };

                    await callSendAPI(psid, response);
                }
                else
                {
                    await ShowDetails(psid, course.Id);
                }
            }
            else
            {
                response = new
                {
                    text = "I don't find any courses matching your key words!"
                };

                await callSendAPI(psid, response);
            }


            await Continue(psid);

            return Ok("EVENT_RECEIVED");
        }

        private async Task Continue(string sender_psid)
        {
            object response = new { text = "" };

            response = new
            {
                text = "Do you want to continue...?",
                quick_replies = new object[]
    {
                        new
                        {
                             content_type = "text",
                             title = "Yes",
                             payload = "yes"
                        },
                        new
                        {
                             content_type = "text",
                             title = "No",
                             payload = "no"
                        },
                        new
                        {
                             content_type = "text",
                             title = "Main Menu",
                             payload = "main_menu"
                        }
                }
            };

            await callSendAPI(sender_psid, response);
        }

        //Handles messages events
        private async Task handleMessage(string sender_psid, dynamic received_message)
        {
            object response = new { text = "Hi" };


            //Text
            if (received_message["text"] != null)
            {
                if (received_message["quick_reply"] == null)
                {
                    response = new
                    {
                        text = $"I don’t recognize {received_message["text"]}. Sorry, I'm just a bot ^_^"
                    };

                    await callSendAPI(sender_psid, response);

                    await GetStarted(sender_psid);
                }
                else
                {
                    if (received_message["quick_reply"]["payload"] == "categories")
                    {
                        await Categories(sender_psid);
                    }
                    else if (received_message["quick_reply"]["payload"] == "lookup_course")
                    {
                        await LookupCourse(sender_psid);
                    }
                    else if (received_message["quick_reply"]["payload"] == "talk_to_an_agent")
                    {
                        await TalkToAnAgent(sender_psid);
                    }
                    else if (received_message["quick_reply"]["payload"] == "yes")
                    {
                        await LookupCourse(sender_psid);
                    }
                    else if (received_message["quick_reply"]["payload"] == "no")
                    {
                        await GetStarted(sender_psid);
                    }
                    else if (received_message["quick_reply"]["payload"] == "main_menu")
                    {
                        await GetStarted(sender_psid);
                    }
                }
            }

            //Attachments
            if (received_message["attachments"] != null)
            {
                //string attachment_url = received_message["attachments"][0]["payload"]["url"];

                response = new
                {
                    text = $"I don’t recognize the attachments. Sorry, I'm just a bot ^_^"
                };

                await callSendAPI(sender_psid, response);

                await GetStarted(sender_psid);
            }
        }

        //Handles messaging_postbacks events
        private async Task handlePostback(string sender_psid, dynamic received_postback)
        {
            object response = new { text = "Bye" };
            if (received_postback["payload"] == "get_started")
            {
                response = new
                {
                    text = $"Hi, Welcome to Programming Course!"
                };

                await callSendAPI(sender_psid, response);

                await GetStarted(sender_psid);
            }
            else if (received_postback["payload"] == "main_menu")
            {
                await GetStarted(sender_psid);
            }
            else if (received_postback["payload"] == "categories")
            {
                await Categories(sender_psid);
            }
            else if (received_postback["payload"] == "talk_to_an_agent")
            {
                await TalkToAnAgent(sender_psid);
            }
            else
            {
                if (received_postback["title"] == "Show Courses")
                {
                    string categoryId = received_postback["payload"];
                    await ShowCourses(sender_psid, int.Parse(categoryId));
                }
                else if (received_postback["title"] == "Show Details")
                {
                    string courseId = received_postback["payload"];
                    await ShowDetails(sender_psid, int.Parse(courseId));
                }
            }
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

        private async Task GetStarted(string sender_psid)
        {
            object response = new { text = "" };

            response = new
            {
                text = "You can use the menu below to navigate through the features or talk with a live agent."
            };

            await callSendAPI(sender_psid, response);

            response = new
            {
                text = "What can I do to help you today?",
                quick_replies = new object[]
                {
                        new
                        {
                             content_type = "text",
                             title = "Categories",
                             payload = "categories"
                        },
                        new
                        {
                             content_type = "text",
                             title = "Lookup Course",
                             payload = "lookup_course"
                        },
                        new
                        {
                             content_type = "text",
                             title = "Talk to an agent",
                             payload = "talk_to_an_agent"
                        }
                }
            };

            await callSendAPI(sender_psid, response);
        }

        private async Task Categories(string sender_psid)
        {
            object response = new { text = "" };

            var categories = await categoryService.GetAll();
            object[] elements = new object[categories.Count];

            for (int i = 0; i < categories.Count; i++)
            {
                object element = new
                {
                    title = categories[i].Name,
                    image_url = categories[i].ImageUrl,
                    buttons = new object[]
                    {
                        new
                        {
                            type = "web_url",
                            url = "https://webnc-az3r.vercel.app/",
                            title = "View on Website"
                        },
                        new
                        {
                            type = "postback",
                            title = "Show Courses",
                            payload = categories[i].Id
                        }
                    }
                };
                elements[i] = element;
            }

            response = new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "generic",
                        elements = elements
                    }
                }
            };

            await callSendAPI(sender_psid, response);
        }

        private async Task LookupCourse(string sender_psid)
        {
            object response = new { text = "" };

            response = new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "button",
                        text = "Please type course's keywords that you are want to search in Set info...",
                        buttons = new object[]
                            {
                                new
                                {
                                    type = "web_url",
                                    url = "https://programmingcourse.herokuapp.com/StaticFiles/lookup_course.html",
                                    title = "Set info",
                                    webview_height_ratio = "tall", //display on mobile
                                    messenger_extensions = true    //false : open the webview in new tab
                                },
                                new
                                {
                                    type = "postback",
                                    title = "Main Menu",
                                    payload = "main_menu"
                                }
                            }
                    }
                }
            };

            await callSendAPI(sender_psid, response);
        }

        private async Task TalkToAnAgent(string sender_psid)
        {
            object response = new { text = "" };

            response = new
            {
                text = "Someone real will be with you in a few minutes."
            };

            await callSendAPI(sender_psid, response);
        }

        private async Task ShowCourses(string sender_psid, int categoryId)
        {
            object response = new { text = "" };

            var category = await categoryService.GetWithAllInfoById(categoryId); //
            object[] elements = new object[category.Courses.Count];

            for (int i = 0; i < category.Courses.Count; i++)
            {
                object element = new
                {
                    title = category.Courses[i].Name,
                    image_url = category.Courses[i].ImageUrl,
                    subtitle = $"${category.Courses[i].Price} (Discount: {category.Courses[i].Discount})",
                    buttons = new object[]
                    {
                        new
                        {
                            type = "web_url",
                            url = "https://webnc-az3r.vercel.app/",
                            title = "View on Website"
                        },
                        new
                        {
                            type = "postback",
                            title = "Show Details",
                            payload = category.Courses[i].Id
                        },
                        new
                        {
                            type = "postback",
                            title = "Main Menu",
                            payload = "main_menu"
                        }
                    }
                };
                elements[i] = element;
            }

            response = new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "generic",
                        elements = elements
                    }
                }
            };

            await callSendAPI(sender_psid, response);
        }

        private async Task ShowDetails(string sender_psid, int courseId)
        {
            object response = new { text = "" };
            //var course = await courseRepository.GetById(courseId);
            var course = await courseService.GetWithAllInfoById(courseId);

            response = new
            {
                text = $"Course Name: {course.name} \n" +
                $"Price: {course.price}$ (Discount: {course.discount}) \n" +
                $"Rating: {course.rating} \n" +
                $"View: {course.viewNumber} \n" +
                $"Number of Lecture: {course.lectures.Count} \n" +
                $"Number of Registered Student: {course.registeredNumber} \n" +
                $"Short Discription: {course.shortDiscription}"
            };

            await callSendAPI(sender_psid, response);

            response = new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "generic",
                        elements = new object[]
                        {
                            new
                            {
                                title = course.name,
                                image_url = course.imageUrl,
                                subtitle = $"${course.price} (Discount: {course.discount})",
                                buttons = new object[]
                                {
                                    new
                                    {
                                        type = "web_url",
                                        url = "https://webnc-az3r.vercel.app/",
                                        title = "View on Website"
                                    },
                                    new
                                    {
                                        type = "postback",
                                        title = "Back to categories",
                                        payload = "categories"
                                    },
                                    new
                                    {
                                        type = "postback",
                                        title = "Main Menu",
                                        payload = "main_menu"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            await callSendAPI(sender_psid, response);
        }
    }
}
