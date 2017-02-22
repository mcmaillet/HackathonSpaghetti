using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

using HellowApp6.Entities;
using HellowApp6.NetworkUtils;

using Newtonsoft.Json;

namespace HellowApp6
{
    [Activity(Label = "NextActivity")]
    public class NextActivity : Activity
    {
        Service1Connection democon;
        int bpm = 0;
        public static TextView patientbpm;
        public static EditText patientinject;
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

            patientbpm = (TextView)FindViewById(Resource.Id.tv_current_heartbeat);

            var ib_device = (ImageButton)FindViewById(Resource.Id.ib_device);
            var et_name = (EditText)FindViewById(Resource.Id.et_name);
            var et_age = (EditText)FindViewById(Resource.Id.et_age);
            var ib_emerg = (ImageButton)FindViewById(Resource.Id.ib_emerg);
            var tv_created = (TextView)FindViewById(Resource.Id.tv_created);
            var sw_monitor_hr = (Switch)FindViewById(Resource.Id.sw_monitor_hr);
            var et_highHr = (EditText)FindViewById(Resource.Id.et_highHr);
            var et_lowHr = (EditText)FindViewById(Resource.Id.et_lowHr);
            patientinject = (EditText)FindViewById(Resource.Id.et_injection);

            var btn_inject = (Button)FindViewById(Resource.Id.btn_inject);

            var ib_location = (ImageButton)FindViewById(Resource.Id.ib_location);

            var ib_edit_name = (ImageButton)FindViewById(Resource.Id.ib_edit_name);
            var ib_save_name = (ImageButton)FindViewById(Resource.Id.ib_save_name);
            ib_save_name.Visibility = ViewStates.Gone;
            var ib_edit_hrrange = (ImageButton)FindViewById(Resource.Id.ib_edit_hrrange);
            var ib_save_hrrange = (ImageButton)FindViewById(Resource.Id.ib_save_hrrange);
            ib_save_hrrange.Visibility = ViewStates.Gone;
            var ib_edit_emerg = (ImageButton)FindViewById(Resource.Id.ib_edit_emerg);
            var ib_save_emerg = (ImageButton)FindViewById(Resource.Id.ib_save_emerg);
            ib_save_emerg.Visibility = ViewStates.Gone;
            var ib_edit_age = (ImageButton)FindViewById(Resource.Id.ib_edit_age);
            var ib_save_age = (ImageButton)FindViewById(Resource.Id.ib_save_age);
            ib_save_age.Visibility = ViewStates.Gone;

            et_name.Text = patient.name;
            et_age.Text = "" + patient.age;
            tv_created.Text = patient.created.Substring(0,patient.created.IndexOf('T'));
            sw_monitor_hr.Checked = patient.monitor;
            et_highHr.Text = ""+patient.highHr;
            et_lowHr.Text = "" + patient.lowHr;

            btn_inject.Click += delegate
              {
                  InjectPatient(patient);
              };

            ib_location.Click += delegate
              {
                  ShowGoogleMapsLocation(patient);
              };

            ib_emerg.Click += delegate
              {
                  var uri = Android.Net.Uri.Parse("tel:" + patient.emergPhone);
                  var intent = new Intent ( Intent.ActionDial, uri );
                  StartActivity ( intent );
              };

            ib_device.Click += delegate
              {
                  View view = LayoutInflater.Inflate(Resource.Layout.dialog_device, null);
                  AlertDialog builder = new AlertDialog.Builder(this).Create();
                  builder.SetView(view);
                  builder.SetCanceledOnTouchOutside(true);
                  TextView tv_device_id = view.FindViewById<TextView>(Resource.Id.tv_device_id);
                  tv_device_id.Text = patient.id;
                  builder.Show();
              };

            ib_save_name.Click += delegate
              {
                  patient.name = et_name.Text;
                  UpdatePatient(patient);

                  et_name.Enabled = false;
                  ib_save_name.Visibility = ViewStates.Gone;
                  ib_edit_name.Visibility = ViewStates.Visible;
              };

            ib_edit_name.Click += delegate
              {
                  ib_save_name.Visibility = ViewStates.Visible;
                  ib_edit_name.Visibility = ViewStates.Gone;
                  et_name.Enabled = true;
              };

            ib_save_hrrange.Click += delegate
              {
                  patient.highHr = Convert.ToInt32(et_highHr.Text);
                  patient.lowHr = Convert.ToInt32(et_lowHr.Text);
                  UpdatePatient(patient);

                  et_highHr.Enabled = false;
                  et_lowHr.Enabled = false;
                  ib_save_hrrange.Visibility = ViewStates.Gone;
                  ib_edit_hrrange.Visibility = ViewStates.Visible;
              };

            ib_edit_hrrange.Click += delegate
              {
                  ib_save_hrrange.Visibility = ViewStates.Visible;
                  ib_edit_hrrange.Visibility = ViewStates.Gone;
                  et_highHr.Enabled = true;
                  et_lowHr.Enabled = true;
              };
            getVitalsAsync(patient);
            //var timer = new System.Threading.Timer(e => blah(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }
        private void InjectPatient(Patient PATinject)
        {
            string uri = PatientsNetworkUtils.CONTROLLER_BASE_ADDRESS + "/";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(uri);
            /*var postContent = new FormUrlEncodedContent ( new []
            {
                new KeyValuePair<string, string>("name", patientName)
                
            } );*/

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
            string jsonInject = "{ \"volume\":\"" + patientinject.Text + "\"}";
            request.Content = new StringContent(jsonInject,
                Encoding.UTF8,
                "application/json");
            //var response = httpClient.SendAsync(request).Result;



            //response.EnsureSuccessStatusCode();


            //string content = await response.Content.ReadAsStringAsync();
        }
        private async void UpdatePatient(Patient PATupdate)
        {
            var JSONpatient = JsonConvert.SerializeObject(PATupdate);

            HttpClient htc = new HttpClient();
            htc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Post, "");
            hrm.Content = new StringContent(JSONpatient, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await htc.PostAsync("https://aa798a67.ngrok.io/api/Patients", hrm.Content);
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
        public async void getVitalsAsync(Patient pat)
        {
            string patientVitalsUrl = PatientsNetworkUtils.CONTROLLER_BASE_ADDRESS + "/Readings?device=" + pat.id;

            HttpClient client = PatientsNetworkUtils.GetClient(patientVitalsUrl);

            string response = await client.GetStringAsync("");

            var vitals = JsonConvert.DeserializeObject<IEnumerable<Vital>>(response).ToList();
            if (vitals.Count == 0)
            {
                patientbpm.Text = "Not Found";
            }
            else
            {
                patientbpm.Text = vitals[0].value.ToString();
            }
        }
        public class Vital
        {
            public string meaning { get; set; }
            public int value { get; set; }
            public object received { get; set; }
        }
    }
}