﻿using Core.Model.FlatsModels;
using Flats.Core.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Flats.Core.Scraping
{
    public class OtoDomScrapper : Scraper
    {
        // price 100k to 500k, price per m 1750 to 8500
        public const string AllOffers = @"https://www.otodom.pl/sprzedaz/mieszkanie/wroclaw/?search%5Bfilter_float_price%3Afrom%5D=100000&search%5Bfilter_float_price%3Ato%5D=500000&search%5Bfilter_float_price_per_m%3Afrom%5D=1750&search%5Bfilter_float_price_per_m%3Ato%5D=8500&search%5Bdescription%5D=1&search%5Border%5D=filter_float_price%3Adesc&search%5Bdist%5D=0&search%5Bsubregion_id%5D=381&search%5Bcity_id%5D=39&nrAdsPerPage=72&page={0}";
        public const string PrivateOffers = "https://www.otodom.pl/sprzedaz/mieszkanie/wroclaw/?search%5Bfilter_float_price%3Afrom%5D=100000&search%5Bfilter_float_price%3Ato%5D=500000&search%5Bfilter_float_price_per_m%3Afrom%5D=1750&search%5Bfilter_float_price_per_m%3Ato%5D=8500&search%5Bdescription%5D=1&search%5Bprivate_business%5D=private&search%5Border%5D=filter_float_price%3Adesc&search%5Bdist%5D=0&search%5Bsubregion_id%5D=381&search%5Bcity_id%5D=39&nrAdsPerPage=72&page={0}";
        
        public OtoDomScrapper()
        {
            ScrapingUrl = PrivateOffers;
        }

        public override string Name => "OtoDom";
        public string Url { get; set; }

        protected override int GetPageCount(HtmlDocument document)
        {
            var sth = document.DocumentNode.SelectSingleNode(@"//*[@class='current']");
            if (sth == null)
                return 1;
            else
                return int.Parse(sth.InnerText.Trim());
        }

        protected override HtmlNodeCollection GetOffers(HtmlDocument document)
        {
            return document.DocumentNode.SelectNodes(@"//*[@id='body-container']/div/div/div[2]/div/article");
        }

        protected override FlatDataBM ParseOffer(HtmlNode node)
        {
            var detailsNode = node.SelectSingleNode(@".//*[@class='offer-item-details']");
            var bottomDetails = node.SelectSingleNode(@".//*[@class='offer-item-details-bottom']");
            var errors = new List<string>();

            if (IsPromo(detailsNode, errors))
            {
                return null;
            }

            Url = GetUrl(detailsNode, errors);
            var Rooms = GetRooms(detailsNode, errors);
            var TotalPrice = GetPrice(detailsNode, errors);
            var SquareMeters = GetArea(detailsNode, errors);

            var data = new FlatDataBM(SquareMeters, TotalPrice, Rooms, Url);
            //data.Location = GetLocation(detailsNode);
            return data;
        }

        private bool IsPromo(HtmlNode node, List<string> errors)
        {
            var promoString = string.Empty;
            try
            {
                var urlNode = node.SelectSingleNode(@".//header/h3/a");
                promoString = urlNode.GetAttributeValue("data-featured-tracking", "");
            }
            catch(Exception e)
            {
                errors.Add($"Can't tell if promo offer. Offer: {Url}. Exception {e.Message}");
            }
            
            return "promo_top_ads".Equals(promoString) || "promo_vip".Equals(promoString);
        }

        private string GetLocation(HtmlNode node, List<string> errors)
        {
            var location = string.Empty;
            var normalized = string.Empty;

            try
            {
                var urlNode = node.SelectSingleNode(@".//header/p");
                location = urlNode.InnerText;
            }
            catch(Exception e)
            {
                errors.Add($"Can't parse Location. Offer: {Url}. Exception {e.Message}");
            }

            return location;
        }

        private decimal GetArea(HtmlNode node, List<string> errors)
        {
            decimal squareMeters = 0m;
            var normalized = string.Empty;

            try
            {
                var areaNode = node.SelectSingleNode(@".//*[@class='hidden-xs offer-item-area']");
                normalized = areaNode.InnerText.Trim();
                var str = normalized.Remove(normalized.Length - 3).Replace(" ", "");

                squareMeters = decimal.Parse(str, CultureInfo.CreateSpecificCulture("pl-PL"));
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Area [{normalized}]. Offer: {Url}. Exception {e.Message}");
            }

            return squareMeters;
        }

        private decimal GetPrice(HtmlNode node, List<string> errors)
        {
            decimal price = 0m;
            var normalized = string.Empty;

            try
            {
                var urlNode = node.SelectSingleNode(@".//*[@class='offer-item-price']");
                normalized = urlNode.InnerText.Trim();
                var str = normalized.Remove(normalized.Length - 3).Replace(" ", "").Replace(",", ".");

                price = decimal.Parse(str, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Price [{normalized}]. Offer: {Url}. Exception {e.Message}");
            }

            return price;
        }

        private int GetRooms(HtmlNode node, List<string> errors)
        {
            int rooms = 0;
            var normalized = string.Empty;

            try
            {
                var roomsNode = node.SelectSingleNode(@".//*[@class='offer-item-rooms hidden-xs']");
                normalized = roomsNode.InnerText.Trim().Split(' ')[0].Replace(">", "");

                rooms = int.Parse(normalized);
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Rooms [{normalized}]. Offer: {Url}. Exception {e.Message}");
            }

            return rooms;
        }

        private string GetUrl(HtmlNode node, List<string> errors)
        {
            string url = string.Empty;
            var normalized = string.Empty;

            try
            {
                var urlNode = node.SelectSingleNode(@".//header/h3/a");
                normalized = urlNode.InnerText.Trim().Split(' ')[0].Replace(">", "");

                url = urlNode.GetAttributeValue("href", "");
            }
            catch (Exception e)
            {
                errors.Add($"Can't parse Url [{normalized}]. Exception {e.Message}");
            }

            return url;
        }
    }
}