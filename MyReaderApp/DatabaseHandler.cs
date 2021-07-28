using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyReaderApp
{
    public class DatabaseHandler
    {
        private SQLiteConnection connection;

        public string ErrorMessage { get; set; }

        public DatabaseHandler()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            connection = new SQLiteConnection(Path.Combine(path, "reader.db"));
            CreateAndSeedData();
        }

        public void CreateAndSeedData()
        {
            try
            {
                connection.CreateTable<User>();
                connection.CreateTable<Feed>();
            }
            catch (Exception ex)
            {

            }
        }

        public bool ValidUser(string username, string password)
        {
            List<User> users = connection.Query<User>("Select * from User");
            if (users != null && users.Count > 0)
            {
                foreach (User user in users)
                {
                    if (user.UserName.Equals(username) && user.Password.Equals(password))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddNewUser(User user)
        {
            try
            {
                connection.Insert(user);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool AddNewFeed(Feed feed)
        {
            try
            {
                connection.Insert(feed);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public List<Feed> GetAllFeeds()
        {
            List<Feed> feeds = connection.Query<Feed>("Select * from Feed");
            return feeds;
        }

        public Feed GetFeed(int feedid)
        {
            List<Feed> feeds = connection.Query<Feed>("Select * from Feed");
            if(feeds!=null && feeds.Count > 0)
            {
                foreach (Feed feed in feeds)
                {
                    if(feed.FeedID == feedid)
                    {
                        return feed;
                    }
                }
            }
            return null;
        }

        public bool DeleteFeed(int feedid)
        {
            try
            {
                connection.Delete(GetFeed(feedid));
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }
    }
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; }

        [Unique]
        public string UserName { get; set; }

        public string Password { get; set; }
    }
    public class Feed
    {
        [PrimaryKey, AutoIncrement]
        public int FeedID { get; set; }

        public string FeedTitle { get; set; }

        public string FeedUrl { get; set; }
    }
}