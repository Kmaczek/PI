using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic.FlatsFeed
{
    public class FeedStats
    {
        public FeedStats(string serviceName)
        {
            FeedName = serviceName;
        }

        public string FeedName { get; set; }
        public int Updated { get; set; }
        public int Added { get; set; }
        public List<string> Errors => new List<string>();
    }
}
