using Core.Domain.Logic.FlatsFeed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic.OtodomService
{
    public interface IFlatsFeedService
    {
        FeedStats FeedStats { get; }
        void StartFeedProcess(string url);
    }
}
