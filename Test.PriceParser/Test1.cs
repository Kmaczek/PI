using NUnit.Framework;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Test.PriceParser
{
    public class Test1
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestX()
        {

            var browserFetcher = new BrowserFetcher();
            var s = browserFetcher.DownloadAsync().Result;
            using var browser = Puppeteer.LaunchAsync(
                new LaunchOptions { 
                    Headless = true, 
                    Args = new [] { "--lang=pl-PL,pl" }
                }).Result;
            using var page = browser.NewPageAsync().Result;
            page.SetCookieAsync(
                new CookieParam() { Name = "_cmuid", Value = "f4c7b808-1b84-40fc-a388-7fe9f3fff83a", Domain = ".allegro.pl", Path = "/", Size = 42, HttpOnly= false, Secure = true, Expires= 31536000, SameSite = SameSite.None },
                new CookieParam() { Name = "datadome", Value = "1WMsKUgF44zYjfvZKz_~tsotUsZbS_jEt5enKw.TFCNMF_BNeP-cV1mwJ.sTN6y5~InaNpS6y4eujCI~Ww3864TOdlECEA~mxqYuyzgF3k", Expires = 31536000, Domain = ".allegro.pl", Path = "/", Size = 114, HttpOnly = false, Secure = true, SameSite = SameSite.Lax },
                new CookieParam() { Name = "gdpr_permission_given", Value = "1", Domain = ".allegro.pl", Path = "/", Size = 22, HttpOnly = false, Secure = false, Expires = 31536000, SameSite = SameSite.None },
                new CookieParam() { Name = "wdctx", Value = "v3.K0lQs2Hagju_8iablQH4IQBwAPGEt5wmrVmDwsOlvK4q9CLFQ3j9hwwJUP-h5j6P4TngqHc6ZXmWlrSgO3nKLsHEblspRnS3RRgwvhiPs_Bw8h04uFkQYJfqhF6ItzvecgU5M_uM6g2n3_Xn7OARNJzsi65wuvCZY1cCdp488VLxIA3UMrViIUjJIko", Domain = ".allegro.pl", Path = "/", Size = 195, HttpOnly = false, Secure = true, Expires = 31536000, SameSite = SameSite.None }
                ).Wait();
            page.SetGeolocationAsync(new GeolocationOption() { Latitude = 51.1m, Longitude = 17.03333m });
            page.SetJavaScriptEnabledAsync(true).Wait();
            var r = page.GoToAsync("https://allegro.pl/kategoria/bakalie-orzechy-pestki-orzechy-261230?string=orzechy%20laskowe").Result;
            var path = @$"C:\Projects\asp core\scr\{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.png";
            page.ScreenshotAsync(path).Wait();

            using (WebClient client = new WebClient())
            {
                client.Encoding = new UTF8Encoding();

                try
                {
                    //var s = client.DownloadString("https://allegro.pl/oferta/orzechy-laskowe-1kg-luskany-swiezy-10636623588");
                }
                catch(WebException e)
                {
                    var response = e.Response as WebResponse;

                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
