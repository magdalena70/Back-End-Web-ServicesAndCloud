using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections;

namespace UsingHttpClient
{
    public class Program
    {
        private const string getAllAds = "http://localhost:53492/api/ads";
        private const string postNewAd = "http://localhost:53492/api/ads";

        static void Main()
        {
            PrintAllAdsAsync();
            Console.WriteLine("Print this before async method.");
            Console.ReadLine();
            /*try
            {
                PrintAllAds();
            }catch(AggregateException ex)
            {
                if(ex.InnerExceptions.Any(e => e is TaskCanceledException))
                {
                    Console.WriteLine("Conection to {0} timed out",
                        getAllAds);
                }
                else
                {
                    throw ex;
                }
            }*/
        }

        private static async void PrintAllAdsAsync()
        {
            var httpClient = new HttpClient();
            using(httpClient)
            {
                var response = await httpClient.GetAsync(getAllAds);
                var ads = await response.Content.ReadAsAsync<IEnumerable<AdsDto>>();
                foreach (var ad in ads)
                {
                    Console.WriteLine("ad: {0} - owner: {1}", ad.Name, ad.Owner.ToString());
                }

                Console.WriteLine(response);
            }
        }

        private static void PrintAllAds()
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                httpClient.Timeout = new TimeSpan(0, 0, 0, 3);
                var response = httpClient.GetAsync(getAllAds).Result;
                //Console.WriteLine(response);
                //var json = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(json);
                var ads = response.Content.ReadAsAsync<IEnumerable<AdsDto>>().Result;
                foreach (var ad in ads)
                {
                    Console.WriteLine("ad: {0} - owner: {1}", ad.Name, ad.Owner.ToString());
                }
            }
        }
    }
}
