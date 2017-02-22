using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

using HellowApp6.Entities;

using Newtonsoft.Json;

namespace HellowApp6
{
    [Activity(Label = "NextActivity")]
    public class NextActivity : Activity
    {
        Service1Connection democon;
        int bpm = 0;
        protected override void OnStart()
        {
            base.OnStart();
            democon = new Service1Connection();
            var demoServiceIntent = new Intent(this, typeof(Service1));
            BindService(demoServiceIntent, democon, Bind.AutoCreate);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PatientDetails);

            string jsonpatient = "";
            jsonpatient = Intent.Extras.GetString("details");
            Patient patient = JsonConvert.DeserializeObject<Patient>(jsonpatient);

            var tv_id = (TextView)FindViewById(Resource.Id.tv_id);
            var tv_name = (TextView)FindViewById(Resource.Id.tv_name);
            var tv_age = (TextView)FindViewById(Resource.Id.tv_age);
            var tv_created = (TextView)FindViewById(Resource.Id.tv_created);
            var tv_location = (TextView)FindViewById(Resource.Id.tv_location);

            tv_id.Text = patient.id;
            tv_name.Text = patient.name;
            tv_age.Text = "" + patient.age;
            tv_location.Text = "Lon.: "+patient.longitude + "\nLat.: " + patient.latitude;
            tv_created.Text = patient.created;

            tv_location.Click += delegate
            {
                ShowGoogleMapsLocation(patient);
            };
            //var timer = new System.Threading.Timer(e => blah(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }
        protected void ShowGoogleMapsLocation(Patient p)
        {
            //LocationManager lm = (LocationManager)GetSystemService(LocationService);
            //Criteria crit = new Criteria { Accuracy = Accuracy.Fine };
            //Location loc = lm.GetLastKnownLocation(lm.GetBestProvider(crit, true));
            string coord = p.latitude + "," + p.longitude;
            var geoUri = Android.Net.Uri.Parse("geo:" + coord + "?q=" + coord + "(" + p.name + ")");
            var mapIntent = new Intent(Intent.ActionView, geoUri);
            StartActivity(mapIntent);
        }
        protected void blah()
        {
            if (democon.Binder != null && democon!=null)
            {
                bpm = democon.Binder.GetBPM();   //Gets BPM from service
            }
            //UI threading problem!!
            //tv_desc.Text = ""+bpm;
        }
    }
}