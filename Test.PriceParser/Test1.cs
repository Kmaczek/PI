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
                //client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0");
                //client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                //client.Headers.Add(HttpRequestHeader.AcceptEncoding, "Accept-Encoding");
                //client.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl,en-US;q=0.7,en;q=0.3");
                //client.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                //client.Headers.Add(HttpRequestHeader.Connection, "keep-alive");
                //client.Headers.Add(HttpRequestHeader.Host, "allegro.pl");
                //client.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
                //client.Headers.Add("DNT", "1");
                //client.Headers.Add(HttpRequestHeader.Referer, "https://allegro.pl/kategoria/bakalie-orzechy-pestki-orzechy-261230?string=orzechy%20laskowe");
                //client.Headers.Add("Sec-Fetch-Dest", "document");
                //client.Headers.Add("Sec-Fetch-Mode", "navigate");
                //client.Headers.Add("Sec-Fetch-Site", "same-origin");
                //client.Headers.Add("Sec-Fetch-User", "?1");
                //client.Headers.Add("Upgrade-Insecure-Requests", "1");
                //client.Headers.Add(HttpRequestHeader.Cookie, @"cartVersion=1; cartTime=1572382505103; ws3=LFG0et4pT1cAgF1k3CJx6xfdcTGvMVTBI; dc1=YWFmOAZfUQNYWVQNNWZhNw%3D%3D; cartUserId=39049825-ffff-ffff-ffff-ffffffffffffffffffff-ffff-ffff-ffff-ffffffffffff; datadome=GnugS3yNoPqNdC~LqTODppAkdu127R1.RquAJfXsp_66ybGlWlh5KsE~HvrgHE6y6cOnU0dfUSSHHsSxrqAjlB7ypxEkOlWAXI7JEs0VgL; gdpr_permission_given=1; QXLSESSID=c2f41261d30f67063e4e8576e66014ff8d975248846f2f//03; qeppo_login2=%7B%22welcome%22%3A%22Witaj%2C%22%2C%22id%22%3A%2239049825%22%2C%22username%22%3A%22flosbox%40gmai…EEbib%2BGr9WQGJ1sZgd6xnMxYjqTT1k3aPzYb6yPWVkhpIz%2FppoAYV%2FKz%2FCDeUyU%3D; qeppo_priv_cookie=YWFmOAZfUQNYWVQNNWZhNw%3D%3D; userIdentity=%7B%22id%22%3A%2239049825%22%7D; wdctx=v3.xzRxRyQSNneiv8V1Bi99DWyjH9N2OrpJaXjG8VoRU4rnpLUQKYA-mrFFftVKIjuCEardFUk4Pnj__-BybfONjxl9I9QgqeJ2D-BpnSLTwxUR02duZdjl6Y_jsvAnkIWgx8_q2T-pYCEBTtW6odBNeZVeK9O5wD2P-OMjmZsfymvVpwsk4Pp6FjH87C8u; _cmuid=3f14595e-700e-4a8b-b768-26b5f7c9fcc6; all_rct=rct300028689be362678; asf_csrf_token=1dc3a197-ed3c-4057-9dd5-c9604a9b0b67; qeppo_cookie=1");
                //client.Headers.Add(HttpRequestHeader.Up, "keep-alive");
                //client.Headers.Add(HttpRequestHeader.Connection, "keep-alive");

                client.Encoding = new UTF8Encoding();
                //client.Headers[HttpRequestHeader.Accept] = "application/vnd.allegro.public.v1+json";
                //client.Headers["Authorization"] = $"Bearer {allegroToken.AccessToken}";
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
