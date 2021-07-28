using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyReaderApp
{
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : AppCompatActivity,View.IOnClickListener
    {
        string username;
        ListView listview;
        DatabaseHandler handler;
        FeedAdapter adapter;
        List<Feed> feeds;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_home);
            username = Intent.GetStringExtra("username");
            handler = new DatabaseHandler();
            listview = FindViewById<ListView>(Resource.Id.listFeeds);
            feeds = handler.GetAllFeeds();
            adapter = new FeedAdapter(this, feeds,this);
            listview.Adapter = adapter;            
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
                    Intent intent = new Intent(this, typeof(AddFeedActivity));
                    intent.PutExtra("username", username);
                    StartActivity(intent);
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void OnClick(View v)
        {
            if(v is Button)
            {
                Button button = v as Button;
                if(button.Id == Resource.Id.readFeed)
                {
                    string url = button.Tag.ToString();
                    Intent intent = new Intent(this, typeof(ReadFeedActivity));
                    intent.PutExtra("url", url);
                    StartActivity(intent);
                    Finish();
                }
                else if (button.Id == Resource.Id.removeFeed)
                {
                    int feedid = (int)button.Tag;
                    if(handler.DeleteFeed(feedid))
                    {
                        Toast.MakeText(this, "Feed is Removed", ToastLength.Long).Show();
                        RemoveAndUpdate(feedid);
                    }
                    else
                    {
                        Toast.MakeText(this, "Feed is not Removed", ToastLength.Long).Show();
                    }
                }
            }
        }

        public void RemoveAndUpdate(int feedid)
        {
            if(feeds != null && feeds.Count>0)
            {
                bool found = false;
                int index = 0;
                foreach (Feed feed in feeds)
                {
                    if(feed.FeedID == feedid)
                    {
                        found = true;
                        break;
                    }
                    index++;
                }
                if(found)
                {
                    feeds.RemoveAt(index);
                    adapter.NotifyDataSetChanged();
                }
            }
        }
    }
}