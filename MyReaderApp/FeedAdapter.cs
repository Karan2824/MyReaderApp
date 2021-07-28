using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyReaderApp
{
    public class FeedAdapter : BaseAdapter<Feed>
    {
        private readonly Activity context;
        private readonly List<Feed> feeds;
        private View.IOnClickListener clickListener;

        public FeedAdapter(Activity context, List<Feed> feeds, View.IOnClickListener clickListener)
        {
            this.feeds = feeds;
            this.clickListener = clickListener;
            this.context = context;
        }

        public override int Count
        {
            get { return feeds.Count; }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Feed this[int position]
        {
            get { return feeds[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.feed_row, null, false);
            }

            TextView txtTitle = row.FindViewById<TextView>(Resource.Id.txtTitle);
            Button readFeed = row.FindViewById<Button>(Resource.Id.readFeed);
            Button removeFeed = row.FindViewById<Button>(Resource.Id.removeFeed);
            Feed feed = feeds[position];
            txtTitle.Text = feed.FeedTitle;
            readFeed.Tag = feed.FeedUrl;
            removeFeed.Tag = feed.FeedID;
            readFeed.SetOnClickListener(clickListener);
            removeFeed.SetOnClickListener(clickListener);

            return row;
        }
    }
}