using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyReaderApp
{
    [Activity(Label = "ReadFeedActivity")]
    public class ReadFeedActivity : AppCompatActivity
    {
        string url;
        ListView listview;
        IEnumerable<RssSchema> schemas;
        RssAdapter adapter;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_feeds);
            url = Intent.GetStringExtra("url");

            listview = FindViewById<ListView>(Resource.Id.listFeeds);

            //Toast.MakeText(this, url, ToastLength.Long).Show();
            schemas = await Parse(url);
            adapter = new RssAdapter(this, schemas);
            listview.Adapter = adapter;
            Task startupWork = new Task(() =>
            {
                ProcessTask();
            });

            startupWork.Start();
            Task.WaitAll(startupWork);
            adapter = new RssAdapter(this, schemas);
            listview.Adapter = adapter;
            Task.Factory.StartNew(() =>
            {
                ProcessTask();
                Task.Delay(3000);
                adapter = new RssAdapter(this, schemas);
                listview.Adapter = adapter;
            });



        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // set the menu layout on Main Activity  
            MenuInflater.Inflate(Resource.Menu.mainmenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuLogOut:
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                    Finish();
                    return true;
                case Resource.Id.menuAddFeed:           
                    StartActivity(new Intent(this, typeof(AddFeedActivity)));
                    Finish();
                    return true;
                case Resource.Id.menuHome:
                    StartActivity(new Intent(this, typeof(HomeActivity)));
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        async Task<IEnumerable<RssSchema>> ProcessTask()
        {
            return await Task.Run(() =>
            {
                return Parse(url);
            });
        }
        public async Task<IEnumerable<RssSchema>> Parse(string url)
        {
            string feed = null;

            using (var client = new HttpClient())
            {
                feed = await client.GetStringAsync(url);
            }

            if (feed == null) return new List<RssSchema>();

            var parser = new RssParser();
            var rss = parser.Parse(feed);
            return rss;
        }




    }
}