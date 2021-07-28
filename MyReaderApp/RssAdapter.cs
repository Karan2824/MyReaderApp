using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyReaderApp
{
    public class RssAdapter : BaseAdapter<RssSchema>
    {
        private readonly Activity context;
        private readonly IEnumerable<RssSchema> rss;

        public RssAdapter(Activity context, IEnumerable<RssSchema> rss)
        {
            this.rss = rss;
            this.context = context;
        }

        public override int Count
        {
            get {
                if (rss != null)
                {
                    return rss.Count();
                }
                
                return 0;
            }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override RssSchema this[int position]
        {
            get { return rss.ElementAt(position); }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.rss_row, null, false);
            }

            TextView txtTitle = row.FindViewById<TextView>(Resource.Id.txtTitle);
            TextView txtDescription = row.FindViewById<TextView>(Resource.Id.txtDescription);
            RssSchema data = rss.ElementAt(position);
            txtTitle.Text = data.Title;
            txtDescription.Text = data.Summary;
            return row;
        }
    }
}