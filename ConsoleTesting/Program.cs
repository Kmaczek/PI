using AutoMapper;
using CenyMieszkan.Models.FlatData;
using CenyMieszkan.Scraping;
using Common;
using Core.Domain.Logic.EmailGeneration;
using Core.Model;
using Core.Model.FlatsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using xAPI.Records;
using Xtb.Core;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var otodomHtmlGenerator = new OtodomHtmlGenerator(new FlatOutput()
            {
                PrivateFlatsByCategory = new List<FlatCalculationsVM>()
                {
                    new FlatCalculationsVM(){Amount =1, AvgPrice=1212433.234m, AvgPricePerMeter=1435.2352352352352352m, FlatSize="small"},
                    new FlatCalculationsVM(){Amount =2, AvgPrice=346.346534634m, AvgPricePerMeter=3423423523.23523523m, FlatSize="big"},
                    new FlatCalculationsVM(){Amount =3, AvgPrice=7432423.4234m, AvgPricePerMeter=6341523523.235235m, FlatSize="medium"}
                }
            });
            
            EmailAssembler emailAssembler = new EmailAssembler(
                new List<IHtmlGenerator>
                {
                    //xtbHtmlGenerator,
                    otodomHtmlGenerator
                });

            var emailSender = new EmailService();
            emailSender.SendEmail(emailAssembler.GenerateEmail());

            Console.WriteLine("Email Sent");

            Console.ReadLine();
        }

        public class FlatCalculations
        {
            private List<FlatOutput> scrapedFlats = new List<FlatOutput>();

            public List<FlatOutput> ScrapedFlats { get; }

            public FlatCalculations(List<FlatOutput> flats)
            {
                scrapedFlats = flats;
            }


        }
    }
}
