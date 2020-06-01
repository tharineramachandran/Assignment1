using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Assignment1
{
    public partial class Task1 : System.Web.UI.Page
    { 
        public class regions
        {
            private string east;
            private string north;
            private string south;
            private string central;

            private string west;


            public string South   // property
            {
                get { return south; }   // get method
                set { south = value; }  // set method
            }

            public string West   // property
            {
                get { return west; }   // get method
                set { west = value; }  // set method
            }
            public string North   // property
            {
                get { return north; }   // get method
                set { north = value; }  // set method
            }

            public string East   // property
            {
                get { return east; }   // get method
                set { east = value; }  // set method
            }

            public string Central   // property
            {
                get { return central; }   // get method
                set { central = value; }  // set method
            }

            public string end;
            public string start;


            public string End   // property
            {
                get { return end; }   // get method
                set { end = value; }  // set method
            }

            public string Start   // property
            {
                get { return start; }   // get method
                set { start = value; }  // set method
            }
        }         protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument wsResponseXmlDoc = new XmlDocument();

            //http://api.worldweatheronline.com/premium/v1/weather.ashx?key=****&q=London&format=xml&num_of_days=5
            //id=jipx(spacetime0)
            UriBuilder url = new UriBuilder();
            String TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
            url.Scheme = "https";// Same as "http://" 

            url.Host = "api.data.gov.sg";
            url.Path = "v1/environment/24-hour-weather-forecast";// change to v2
            url.Query = "date_time="+TodayDate + "T15%3A28%3A01&date=" + TodayDate ;
            //    https://api.data.gov.sg/v1/environment/24-hour-weather-forecast?date_time=2020-05-25T15%3A28%3A01&date=2020-05-25

            //Make a HTTP request to the global weather web service
            // var json = MakeRequest(url.ToString());

            HttpWebRequest request = WebRequest.Create(url.ToString()) as HttpWebRequest;
            // Set timeout to 15 seconds
            request.Timeout = 15 * 1000;
            request.KeepAlive = false;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            var json = new StreamReader(response.GetResponseStream()).ReadToEnd();
 
            XNode node = JsonConvert.DeserializeXNode(json, "root");


            XmlReader reader = node.CreateReader();
            reader.MoveToContent();
            XmlDocument doc = new XmlDocument();
            XmlNode cd = doc.ReadNode(reader);
            doc.AppendChild(cd); 


           // Response.Write(doc.OuterXml);
// Response.Write(node.ToString());

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(response.GetResponseStream());

            if (doc != null)
            {
                //display the XML response for user
                //String xmlString = doc.InnerXml;
                //Response.ContentType = "text/xml";
                //Response.Write(xmlString);
                  XmlDeclaration xmldecl;
                xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

                //Add the new node to the document.
                XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmldecl, root);


                List<regions> periodlist = new List< regions >();
                XmlNodeList idNodes = doc.SelectNodes("root/items/periods"); 
                  foreach (XmlNode nodes in idNodes)
                {

                    regions region = new regions(); 


                    XmlNode urlNode = nodes.SelectSingleNode("regions");

                   // Person myObj = new Person();
                     XmlNode regionNode = nodes.SelectSingleNode("regions");
                    region.South = regionNode.SelectSingleNode("south").InnerText;
                    region.North = regionNode.SelectSingleNode("north").InnerText;
                    region.East = regionNode.SelectSingleNode("east").InnerText;
                    region.West = regionNode.SelectSingleNode("west").InnerText;
                    region.Central = regionNode.SelectSingleNode("central").InnerText;
                    
                    XmlNode timeNode = nodes.SelectSingleNode("time");
                    region.Start = timeNode.SelectSingleNode("start").InnerText;
                    region.End = timeNode.SelectSingleNode("end").InnerText;

                    

                   periodlist.Add(region);

                   GridView1.DataSource = periodlist;
                    GridView1.DataBind();
                    
             //       // display the XML response for user
             //       String xmlString = doc.InnerXml;
             //Response.ContentType = "text/xml";
             //       Response.Write(xmlString);

             //       // Save the document to a file and auto-indent the output.
             //       XmlTextWriter writer = new XmlTextWriter(Server.MapPath("xmlweather.xml"), null);
             //       writer.Formatting = System.Xml.Formatting.Indented;
             //       wsResponseXmlDoc.Save(writer);

             //       //You're never closing the writer, so I would expect it to keep the file open. That will stop future attempts to open the file

             //       writer.Close();
                    


                }


                 
                //var regions = nodes.Attributes["regions"].Attributes["south"].Value;
                //periodsd.regions = nodes.Attributes["regions"].Attributes["south"].Value;
                // list.Add(periodsd);

                // Save the document to a file and auto-indent the output.
                //XmlTextWriter writer = new XmlTextWriter(Server.MapPath("xmlweather.xml"), null);
                //writer.Formatting = System.Xml.Formatting.Indented;
                //wsResponseXmlDoc.Save(writer);

                //You're never closing the writer, so I would expect it to keep the file open. That will stop future attempts to open the file

                //   writer.Close();
            }
            else
            {
                Response.ContentType = "text/html";
                Response.Write("error");
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //public static XmlDocument MakeRequest(string requestUrl)
        //{
        //    try
        //    {
        //        HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
        //        // Set timeout to 15 seconds
        //        request.Timeout = 15 * 1000;
        //        request.KeepAlive = false;
        //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.Load(response.GetResponseStream());
        //        return (xmlDoc);
        //    }
        //    catch (Exception ex)
        //    { return null; }


        /// <summary>
        /// form1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>


    }
    }


