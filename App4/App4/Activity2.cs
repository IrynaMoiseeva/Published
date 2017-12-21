using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps.Model;
using Android.Views.InputMethods;
using System.Resources;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace App4.Resources
{
    [Activity(Label = "Activity2")]
    class Activity2 : Activity
    {
        const string strGoogleApiKey = "AIzaSyAqB61v3YI6H7Q-jhx3HVSPNBDvX-dr_yY";
        const string strGeoCodingUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        const string strAutoCompleteGoogleApi = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=";
        //to soft keyboard hide
        string autoCompleteOptions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mytoolbar);

            Button Search = FindViewById<Button>(Resource.Id.Search);
            Search.Click += Search_Click;
            var txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch);
            txtSearch.TextChanged += async delegate (object sender, Android.Text.TextChangedEventArgs e)
            {
                try
                {
                    autoCompleteOptions = await fnDownloadString(strAutoCompleteGoogleApi + txtSearch.Text + "&key=" + strGoogleApiKey);

                    if (autoCompleteOptions == "Exception")
                    {
                        Toast.MakeText(this, "Unable to connect to server!!!", ToastLength.Short).Show();
                        return;
                    }

                }
                catch
                {
                    Toast.MakeText(this, "Unable to process at this moment!!!", ToastLength.Short).Show();
                }

            };

        }






        private void Search_Click(object sender, EventArgs e)
        {

          /*  var txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch);
            txtSearch.Text = "London";
            string URL = (strAutoCompleteGoogleApi + txtSearch.Text + "&key=" + strGoogleApiKey);

            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);

            inputManager.HideSoftInputFromWindow(txtSearch.WindowToken, HideSoftInputFlags.NotAlways);
            fnDownloadString(URL);*/

            var txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch);
            Intent myIntent = new Intent(this, typeof(MainActivity));
            myIntent.PutExtra("city", txtSearch.Text);
            SetResult(Result.Ok, myIntent);
            Finish();
        }


        async Task<string> fnDownloadString(string strUri)
        { var strResultData = "";

            try
            {


                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(strUri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<GoogleInfo>(content);
                    var strPredictiveText = new string[data.predictions.Count];
                    var index = 0;
                    foreach (Prediction objPred in data.predictions)
                    {
                        strPredictiveText[index] = objPred.description;
                        index++;
                    }
                    /*  ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, strPredictiveText);
                      txtSearch.Adapter = adapter;*/

                    var wordlist = new String[] { "Hello", "Hey", "Heja", "Hi", "Hola", "Bonjour", "Gday" };
                    ArrayAdapter autoCompleteAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, strPredictiveText);
                    var txtSearch = FindViewById<AutoCompleteTextView>(Resource.Id.txtTextSearch);
                    txtSearch.Adapter = autoCompleteAdapter;
                }
            }
            catch (Exception ex)
            {
                //ToDo Give errormessage to user and possibly log error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return strResultData;
        }

        async void AutoCompleteOption_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
           

            /*  InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);

             inputManager.HideSoftInputFromWindow(txtSearch.WindowToken, HideSoftInputFlags.NotAlways);

             if (txtSearch.Text != string.Empty)
             {
                 var sb = new StringBuilder();
                 sb.Append(strGeoCodingUrl);
                 sb.Append("?address=").Append(txtSearch.Text);*/


            /*    string strResult = await fnDownloadString(sb.ToString());
                if (strResult == "Exception")
                {
                    Toast.MakeText(this, "Unable to connect to server!!!", ToastLength.Short).Show();
                    //private api_key = "AIzaSyAqB61v3YI6H7Q-jhx3HVSPNBDvX-dr_yY";

                }*/
        
    }

    }
}