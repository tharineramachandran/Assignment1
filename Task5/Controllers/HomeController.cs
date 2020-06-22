using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
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
          var time  = DateTime.Now;
            String timestamp = time.ToString("dd_MM_yyyy_hh_mm_ss"); 
            var file  = Request.Files["image"];
            if (file!=null)
            {
                string path = Path.Combine(Server.MapPath("~/"),
                                          Path.GetFileName(file.FileName));
                file.SaveAs(path);


                bool value = amazon(path, timestamp);
                var image = Image.FromFile(path);

                image.Dispose();

                if (value)
                {
                    try
                    {
                        String photolink = "https://task4-csc.s3.amazonaws.com/" + timestamp;
                        String Filename = timestamp;

                        int index = Filename.IndexOf(".");
                        if (index > 0)
                            Filename = Filename.Substring(0, index);
                        String slashtagstr = String.Format("ST0280_{0}_ISO8601_{1}_", time.ToString("dd_MM_yyyy"), Filename);

                        var payload = new
                        {
                            destination = photolink,
                            domain = new
                            {
                                fullName = "rebrand.ly"

                            }
                            ,
                            slashtag = slashtagstr
                            //, title = "Rebrandly YouTube channel"
                        };

                        using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.rebrandly.com") })
                        {
                            httpClient.DefaultRequestHeaders.Add("apikey", "11205a30dee141198102aece40e951c3");
                            httpClient.DefaultRequestHeaders.Add("workspace", "876fc66de4e14985a049ef8dc836ced2");

                            var body = new StringContent(
                                JsonConvert.SerializeObject(payload), UnicodeEncoding.UTF8, "application/json");

                            using (var response = await httpClient.PostAsync("/v1/links", body))
                            {
                                response.EnsureSuccessStatusCode();

                                var link = JsonConvert.DeserializeObject<dynamic>(
                                    await response.Content.ReadAsStringAsync());

                                string strlink = $"https://{link.shortUrl}";
                                Response.StatusCode = 200;
                                return Json(strlink);
                            }
                        }
                    }
                    catch (Exception e) { 
                    

                        Response.StatusCode = 400;
                        return Json("An Error Occured"); 
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json("Could not save to amazon S3");

                }
            }
            Response.StatusCode = 400;
            return Json("Add an Image");
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

        }
         
        public bool amazon(String filePath , String timestamp)
        {
            var bucketName = "task4-csc"; 
   
        // Specify your bucket region (an example region is shown).
         RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
         IAmazonS3 s3Client;

         
            s3Client = new AmazonS3Client("ASIATS2W2CQOXOXZXJE4", "NQ7f/a/VqM8JXzuE2ERew+tFZNW06l4cYpAVgR0Z", "FwoGZXIvYXdzENP//////////wEaDFKnn549zfzz2Guo6iLOAcfGFCv1pig6ZGgySpsPqXdqqm8htRwjRRdSTgI7BbF3SGKbEKfYrp8r/dnGnw+aYjCrldnT/O4AoyGefYBHPls9FkdRewLQqBGXjwdqLjPu65gojFQ9q1nAThP2sk7Z5dGyVNO3QztHZAshcDPJVqs8RLK3LcJZqvNxgcwtMPMRhV3TE76xGoIHEsqKEKVbnVuDAXrMnb4R0cwglaJIpL4j/SyLQlCuzlNKhM2EQUvjd01+YTBLKAeWB6cxh6mUswP0o9YatDnN6hyq0ldtKMG2vvcFMi32Ii2DALNTsI5582s+zeQ9mgXTMGhtWI2e+e5k9kJ85lfpLqwIE4p+VSe3mnk=",   bucketRegion);
         

         
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                  fileTransferUtility.UploadAsync(filePath, bucketName, timestamp);
                Console.WriteLine("Upload 1 completed");

                //// Option 2. Specify object key name explicitly.
                //await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                //Console.WriteLine("Upload 2 completed");

                //// Option 3. Upload data from a type of System.IO.Stream.
                //using (var fileToUpload =
                //    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //{
                //    await fileTransferUtility.UploadAsync(fileToUpload,
                //                               bucketName, keyName);
                //}
                //Console.WriteLine("Upload 3 completed");

                //// Option 4. Specify advanced settings.
                //var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                //{
                //    BucketName = bucketName,
                //    FilePath = filePath,
                //    StorageClass = S3StorageClass.StandardInfrequentAccess,
                //    PartSize = 6291456, // 6 MB.
                //    Key = keyName,
                //    CannedACL = S3CannedACL.PublicRead
                //};
                //fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                //fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                //await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                //Console.WriteLine("Upload 4 completed");

                return true; 
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }
             
        }
    }

    }
  

             
         
    