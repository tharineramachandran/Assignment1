using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Task5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


         
        public async System.Threading.Tasks.Task<ActionResult> bitly()
        {

            var payload = new
            {
                destination = "https://www.youtube.com/channel/UCHK4HD0ltu1-I212icLPt3g",
                domain = new
                {
                    fullName = "rebrand.ly",
                    slashtag = "task8_45239ewrtwetr"
                }
                //, slashtag = "A_NEW_SLASHTAG"
                //, title = "Rebrandly YouTube channel"
            };

            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.rebrandly.com") })
            {
                httpClient.DefaultRequestHeaders.Add("apikey", "dcabacac92074eeea9be758a1cfc88aa");
                httpClient.DefaultRequestHeaders.Add("workspace", "dced5565b5e04a6baab34c7ffe0b4e82");

                var body = new StringContent(
                    JsonConvert.SerializeObject(payload), UnicodeEncoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("/v1/links", body))
                {
                    response.EnsureSuccessStatusCode();

                    var link = JsonConvert.DeserializeObject<dynamic>(
                        await response.Content.ReadAsStringAsync());

                    Console.WriteLine($"Long URL was {payload.destination}, short URL is {link.shortUrl}");
                }
            }
            //    try
            //    {
            //        String value = Request.Form["pLongUrl"];
            //        String apiKey = "ccc2dd00b5ca21ea9e40ca108af58329439a6383";
            //        String username = "tharaerqwerqerwq";
            //        // Create a request using a URL that can receive a post.
            //        WebRequest request = WebRequest.Create("https://api-ssl.bitly.com/v3/shorten?login=" + username + "&access_token=" + apiKey + "&format=json&longUrl=" + value);
            //        // Set the Method property of the request to POST.
            //        request.Method = "POST";

            //        // Create POST data and convert it to a byte array.
            //        string postData = "This is a test that posts this string to a Web server.";
            //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //        // Set the ContentType property of the WebRequest.
            //        request.ContentType = "application/x-www-form-urlencoded";
            //        // Set the ContentLength property of the WebRequest.
            //        request.ContentLength = byteArray.Length;

            //        // Get the request stream.
            //        Stream dataStream = request.GetRequestStream();
            //        // Write the data to the request stream.
            //        dataStream.Write(byteArray, 0, byteArray.Length);
            //        // Close the Stream object.
            //        dataStream.Close();

            //        // Get the response.
            //        WebResponse response = request.GetResponse();
            //        // Display the status.
            //        var status = (((HttpWebResponse)response).StatusDescription);

            //        if (status == "OK")
            //        {
            //            JObject json;
            //            // Get the stream containing content returned by the server.
            //            // The using block ensures the stream is automatically closed.
            //            using (dataStream = response.GetResponseStream())
            //            {
            //                // Open the stream using a StreamReader for easy access.
            //                StreamReader reader = new StreamReader(dataStream);
            //                // Read the content.
            //                string responseFromServer = reader.ReadToEnd();
            //                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            //                json = JObject.Parse(responseFromServer);

            //                // Display the content.
            //                Console.WriteLine(responseFromServer);
            //            }

            //            // Close the response.
            //            response.Close();

            //            //  ViewBag.Message = "Your contact page.";



            //            var key1 = json.GetValue("data");
            //            var values = key1["url"];


            //            Response.StatusCode = (int)HttpStatusCode.OK;
            //            return Json(values.ToString());
            //        }
            //        else {
            //            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //            return Json("Error occured");

            //        }
            //    }catch
            //    {

            //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //        return Json("Error occured");

            //    }
            return Json("sfadfa");
        }



    }
}