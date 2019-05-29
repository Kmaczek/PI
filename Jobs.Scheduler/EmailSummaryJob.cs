using AutoMapper;
using CenyMieszkan.Scraping;
using Common;
using Core.Domain.Logic.EmailGeneration;
using Core.Model;
using Core.Model.FlatsModels;
using System;
using System.Collections.Generic;
using System.Linq;
//using Xtb.Core;

namespace ConsoleTesting
{
    public static class EmailSummaryJob
    {
        public static void EmailJob()
        {
            Console.WriteLine($"Starting Email Summary job at {DateTime.Now.ToLocalTime()}.");
            //var service = new Service();
            //var balance = service.GetBalance();

            //XtbOutput balanceOutput = Mapper.Map<XtbOutput>(balance);
            //var xtbHtmlGenerator = new XtbHtmlGenerator(balanceOutput);
            var otoDomScrapper = new OtoDomScrapper();
            var scrapeResult = otoDomScrapper.Scrape();
            var flatsOutput = new FlatOutput();
            //flatsOutput.FlatsCount = scrapeResult.Count();
            //flatsOutput.TotalPrice = scrapeResult.Average(x => x.TotalPrice);
            //flatsOutput.PricePerMeter = scrapeResult.Average(x => x.TotalPrice / x.SquareMeters);
            var otodomHtmlGenerator = new OtodomHtmlGenerator(flatsOutput);

            EmailAssembler emailAssembler = new EmailAssembler(
                new List<IHtmlGenerator>
                {
                    //xtbHtmlGenerator,
                    otodomHtmlGenerator
                });

            //var emailSender = new EmailService();
            //emailSender.SendEmail(emailAssembler.GenerateEmail());
            //Console.WriteLine($"Email Summary job finished at {DateTime.Now.ToLocalTime()}.");
        }
    }
}
