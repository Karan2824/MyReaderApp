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
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity
    {
        EditText name,  password;
        Button register;
        TextView login;
        bool isNameValid, isPasswordValid;
        TextInputLayout nameError,  passError;
        DatabaseHandler handler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);

            handler = new DatabaseHandler();

            name = FindViewById<EditText>(Resource.Id.name);
            password = FindViewById<EditText>(Resource.Id.password);
            nameError = FindViewById<TextInputLayout>(Resource.Id.nameError);
            passError = FindViewById<TextInputLayout>(Resource.Id.passError);
            register = FindViewById<Button>(Resource.Id.register);
            login = FindViewById<TextView>(Resource.Id.login);
            register.Click += Register_Click;
            login.Click += Login_Click;
        }

        private void Login_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this,typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            if (name.Text.ToString().Length==0)
            {
                nameError.Error = Resources.GetString(Resource.String.name_error);
                isNameValid = false;
            }
            else
            {
                isNameValid = true;
                nameError.ErrorEnabled = false;
            }

            if (password.Text.ToString().Length == 0)
            {
                passError.Error = Resources.GetString(Resource.String.password_error);
                isPasswordValid = false;
            }
            else if (password.Text.ToString().Length < 6)
            {
                passError.Error = Resources.GetString(Resource.String.error_invalid_password);
                isPasswordValid = false;
            }
            else
            {
                isPasswordValid = true;
                passError.ErrorEnabled = false;
            }
            if(isNameValid && isPasswordValid)
            {
                User user = new User();
                user.UserName = name.Text;
                user.Password = password.Text;
                if(handler.AddNewUser(user))
                {
                    Toast.MakeText(this, "New Account is Created!!!", ToastLength.Long).Show();
                    Intent intent = new Intent(this, typeof(HomeActivity));
                    intent.PutExtra("username", user.UserName);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Account is not Created!!!", ToastLength.Long).Show();
                }
            }
        }
    }
}