using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Task8
{
    public class Clarifai
    {
        public void ClarifarTag(Stream imageStream)
        {
            const string ACCESS_TOKEN = "fa80b1ae4e934a3e853c6616bac5b3e5";
            const string CLARIFAI_API_URL = "https://api.clarifai.com/v2/models/{model}/outputs";

            MemoryStream ms = new MemoryStream();
            imageStream.CopyTo(ms);
            string encodedData = Convert.ToBase64String(ms.ToArray());

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer" + ACCESS_TOKEN);

                HttpContent json = new StringContent(
                    "{" +
                "\"inputs\": [" +
                    "{" +
                        "\"data\": {" +
                            "\"image\": {" +
                                "\"base64\": \"" + encodedData + "\"" +
                            "}" +
                       "}" +
                    "}" +
                "]" +
            "}", Encoding.UTF8, "application/json");

                var response = client.PostAsync(CLARIFAI_API_URL, json).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                string body = response.Content.ReadAsStringAsync().Result.ToString();

                Debug.Write(body);
            }
        }
    }
}