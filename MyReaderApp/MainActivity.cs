using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Content;

namespace MyReaderApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText email, password;
        Button login;
        TextView register;
        bool isEmailValid, isPasswordValid;
        TextInputLayout emailError, passError;
        DatabaseHandler handler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_login);
            handler = new DatabaseHandler();

            email = FindViewById<EditText>(Resource.Id.email);
            password = FindViewById<EditText>(Resource.Id.password);
            emailError = FindViewById<TextInputLayout>(Resource.Id.emailError);
            passError = FindViewById<TextInputLayout>(Resource.Id.passError);
            login = FindViewById<Button>(Resource.Id.login);
            register = FindViewById<TextView>(Resource.Id.register);
            login.Click += Login_Click;
            register.Click += Register_Click;
        }

        private void Register_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            StartActivity(intent);
            Finish();
        }

        private void Login_Click(object sender, System.EventArgs e)
        {
            if (email.Text.ToString().Length == 0)
            {
                emailError.Error = Resources.GetString(Resource.String.name_error);
                isEmailValid = false;
            }
            else
            {
                isEmailValid = true;
                emailError.ErrorEnabled = false;
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
            if (isEmailValid && isPasswordValid)
            {
                User user = new User();
                user.UserName = email.Text;
                user.Password = password.Text;
                if (handler.ValidUser(user.UserName,user.Password))
                {
                    Toast.MakeText(this, "Successfull Login", ToastLength.Long).Show();
                    Intent intent = new Intent(this, typeof(HomeActivity));
                    intent.PutExtra("username", user.UserName);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Invalid User Name and Password", ToastLength.Long).Show();
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}