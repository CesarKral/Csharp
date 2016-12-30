using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
//This must be downloaded to project
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.IO;

namespace HttpJson
{
    class Product
    {
        public string name { get; set; }
        public int price { get; set; }
        public string brand { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            mywebclientDATA();          
        }

        static async Task myhttpclientA()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(new Uri("http://localhost:5000")))
                {
                    using (HttpContent content = response.Content)
                    {
                        HttpContentHeaders headers = content.Headers;
                        string mycontent = await content.ReadAsStringAsync();
                        Console.WriteLine(mycontent);
                    }                       
                }                   
            }
        }

        static async Task myhttpclientB()
        {
            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("name", "Cesar"),
                new KeyValuePair<string, string>("car", "BMW")
            };
            HttpContent cont = new FormUrlEncodedContent(queries);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(new Uri("http://localhost:5000/csharp"), cont))
                {
                    using (HttpContent content = response.Content)
                    {
                        HttpContentHeaders headers = content.Headers;
                        string mycontent = await content.ReadAsStringAsync();
                        Console.WriteLine(mycontent);
                    }
                }
            }
        }                      

        public static async Task myhttpclientC()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000");
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("name", "Cesar"),
                new KeyValuePair<string, string>("car", "BMW")
                });
                //var result = await client.PostAsync("/csharp", content).Result;
                var result = await client.PostAsync("/csharp", content);
                //string resultContent = await result.Content.ReadAsStringAsync().Result;
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
            }
        }

        static async Task myhttpclientD()
        {
            using (var client = new HttpClient())
            {
                using (var r = await client.GetAsync(new Uri("http://127.0.0.1:5000")))
                {
                    string result = await r.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
            }
        }

        static async Task myhttpclientE()
        {
            var myurl = "http://localhost:5000/csharp";
            var mybuilder = new UriBuilder(myurl);
            var query = HttpUtility.ParseQueryString(mybuilder.Query);
            query["name"] = "Cesar";
            query["car"] = "BMW";
            mybuilder.Query = query.ToString();
            myurl = mybuilder.ToString();
            //http://localhost:5000/csharp?name=Cesar&car=BMW

            using (var client = new HttpClient())
            {
                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, myurl);           
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                // Just as an example I'm turning the response into a string here
                string responseAsString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseAsString);
            }

                
        }

        static async Task jsonA()
        {
            Product p = new Product();
            p.name = "Surface";
            p.price = 1000;
            p.brand = "Microsoft";

            string jsonvalue = JsonConvert.SerializeObject(p);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000");
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("jsondata", jsonvalue)               
                });
                //var result = await client.PostAsync("/csharp", content).Result;
                var result = await client.PostAsync("/jsonto", content);
                //string resultContent = await result.Content.ReadAsStringAsync().Result;
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
            }

            Product deserialing = JsonConvert.DeserializeObject<Product>(jsonvalue);

            dynamic stuff = JsonConvert.DeserializeObject(@"{ 'Name': 'Jon Smith', 
                                                              'Address': { 'City': 'New York', 'State': 'NY' }, 
                                                              'Age': 42 }");
            string name = stuff.Name;
            string address = stuff.Address.City;

        }

        static void mywebclientGET()
        {
            UriBuilder ub = new UriBuilder();
            ub.Scheme = "http";
            ub.Host = "localhost";
            ub.Port = 5000;
            ub.Path = "csharp";
            //Uri myuri = ub.Uri;
            var query = HttpUtility.ParseQueryString(ub.Query);
            query["name"] = "Cesar";
            query["car"] = "BMW";
            ub.Query = query.ToString();
            var myurl = ub.ToString();
            WebClient client = new WebClient();
            string ret = client.DownloadString(myurl);
            Console.WriteLine(ret);
        }

        static void mywebclientPOST()
        {
            UriBuilder ub = new UriBuilder();
            ub.Scheme = "http";
            ub.Host = "localhost";
            ub.Port = 5000;
            ub.Path = "csharp";
            Uri myuri = ub.Uri;         

            var client = new WebClient();
            var vc = new NameValueCollection();
            vc.Add("name", "Cesar");
            vc.Add("car", "BMW");
            client.UploadValues(myuri, vc);
            /*
            Console.WriteLine(ub.Query);
            Console.WriteLine(myuri.ToString());
            Console.WriteLine(ub.Path);
            Console.WriteLine(ub.Uri);
            Console.WriteLine(myurl);
            Console.WriteLine(query.ToString()); */      
        }

        static void mywebclientDATA()
        {
            Product p = new Product();
            p.name = "Surface";
            p.price = 1000;
            p.brand = "Microsoft";

            Product x = new Product();
            x.name = "Windows Phone";
            x.price = 300;
            x.brand = "Microsoft";

            var sev = new List<Product>();
            sev.Add(p);
            sev.Add(x);

            string jsonvalue = JsonConvert.SerializeObject(sev);

            UriBuilder ub = new UriBuilder();
            ub.Scheme = "http";
            ub.Host = "localhost";
            ub.Port = 5000;
            ub.Path = "dati";
            Uri myuri = ub.Uri;

            var client = new WebClient();
            client.UploadString(myuri, "POST", jsonvalue);

        }
    }
}
