using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;


//using Android.Support.V7.App ;

namespace App4
{
    [Activity(Label = "App4", MainLauncher = true)]
    public class MainActivity : Activity

    {
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                string put_name = data.GetStringExtra("city");
                var cityname = FindViewById<EditText>(Resource.Id.namecity);
                cityname.Text = put_name;
                //put_name and put_position should now hold the results you want, you can do whatever you want with these two values now in your MainActivity
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.top_menus , menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
           ;
            switch (item.ItemId)
            {
                case Resource.Id.menu_search:
                    {
                        Intent i = new Intent(this, typeof(Resources.Activity2));
                        // StartActivity(i);
                        StartActivityForResult(i, 0);
                        break;
                    }
                case Resource.Id.menu_preferences:
                    {
                        // add your code  
                        return true;
                    }

            }
                    return base.OnOptionsItemSelected(item);
                    //Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                    // ToastLength.Short).Show();

            }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
         SetActionBar(toolbar);
           toolbar.Title = "My Toolbar";
            Button txtCity = FindViewById<Button>(Resource.Id.txtCity );
            txtCity.Click += txtCity_Click;
        }

        private void txtCity_Click(object sender, EventArgs e)
        {

            GetWeather();
        }

        public async void GetWeather()
        {

            try
            {
               var api_key = "03e8168ffbb8cafb6b8b6679c528ec97";
                //string URL = ("http://api.openweathermap.org/data/2.5/weather?lat=58.34&lon=11.94&appid="+ api_key);

                var cityname = FindViewById<EditText>(Resource.Id.namecity);

                var citytext = cityname.Text;
                string URL = ("http://api.openweathermap.org/data/2.5/weather?q="+citytext+"&appid="+ api_key);


                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherList = JsonConvert.DeserializeObject<RootObject>(content);
                    
                    //   var editText = FindViewById<TextView>(Resource.Id.weather_text);
                    // editText.Text = weatherList.main.ToString();
                    //weather_text.  weatherList.main;
                    //Databind the list
                    //  lstWeather.ItemsSource = weatherList;
                    var tempr = FindViewById<TextView>(Resource.Id.tempr);
                    tempr.Text = weatherList.main.temp.ToString();
                }
            }
            catch (Exception ex)
            {
                //ToDo Give errormessage to user and possibly log error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}

