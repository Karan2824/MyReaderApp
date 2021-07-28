using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyReaderApp
{
    [Activity(Label = "AddFeedActivity")]
    public class AddFeedActivity : AppCompatActivity
    {
        EditText feedtitle, feedurl;
        Button save;
        bool isTitleValid, isUrlValid;
        TextInputLayout feedTitleError, feedUrlError;
        DatabaseHandler handler;
        string username;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_feed);
            username = Intent.GetStringExtra("username");

            handler = new DatabaseHandler();

            feedtitle = FindViewById<EditText>(Resource.Id.feedtitle);
            feedurl = FindViewById<EditText>(Resource.Id.feedurl);
            feedTitleError = FindViewById<TextInputLayout>(Resource.Id.feedTitleError);
            feedUrlError = FindViewById<TextInputLayout>(Resource.Id.feedUrlError);
            save = FindViewById<Button>(Resource.Id.save);
            save.Click += Save_Click;            
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (feedtitle.Text.ToString().Length == 0)
            {
                feedTitleError.Error = "Please Enter Any Feed Title";
                isTitleValid = false;
            }
            else
            {
                isTitleValid = true;
                feedTitleError.ErrorEnabled = false;
            }

            if (feedurl.Text.ToString().Length == 0)
            {
                feedUrlError.Error = "Please Enter Any Feed Url";
                isUrlValid = false;
            }
            else
            {
                isUrlValid = true;
                feedUrlError.ErrorEnabled = false;
            }
            if (isTitleValid && isUrlValid)
            {
                Feed feed = new Feed();
                feed.FeedUrl = feedurl.Text;
                feed.FeedTitle = feedtitle.Text;
                if (handler.AddNewFeed(feed))
                {
                    Toast.MakeText(this, "New Feed is Saved!!!", ToastLength.Long).Show();                    
                }
                else
                {
                    Toast.MakeText(this, "Feed is not Saved!!!", ToastLength.Long).Show();
                }
            }
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
                case Resource.Id.menuHome:
                    Intent intent = new Intent(this, typeof(HomeActivity));
                    intent.PutExtra("username", username);
                    StartActivity(intent);
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}