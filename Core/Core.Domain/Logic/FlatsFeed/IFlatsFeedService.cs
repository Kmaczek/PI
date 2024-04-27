using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic.FlatsFeed
{
    public interface IFlatsFeedService
    {
        FeedStats FeedStats { get; }
        void StartFeedProcess(string url);
    }
}
