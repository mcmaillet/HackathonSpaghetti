using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Locations;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;

using HellowApp6.Entities;
using HellowApp6.Adapters;
using System;
using HellowApp6.NetworkUtils;
using System.Net.Http.Headers;
using System.Text;

namespace HellowApp6
{
    [Activity(Label = "MainActivity", MainLauncher =true,Icon ="@drawable/Icon")]
    public class MainActivity : Activity
    {
        List<Patient> Patients;
        PatientAdapter pa;
        ListView lvPatientList;
        bool patientadded = false;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            GetPatientList();

            var btn_addpatient = (Button)FindViewById(Resource.Id.btn_addpatient);

            btn_addpatient.Click += delegate
            {
                View view = LayoutInflater.Inflate(Resource.Layout.register_patient_layout, null);
                AlertDialog builder = new AlertDialog.Builder(this).Create();
                builder.SetView(view);
                builder.SetCanceledOnTouchOutside(false);
                var et_patient_name = (EditText)view.FindViewById(Resource.Id.et_patient_name);
                var et_patient_age = (EditText)view.FindViewById(Resource.Id.et_age);
                var et_patient_emerg = (EditText)view.FindViewById(Resource.Id.et_emergPhone);
                var btn_register = (Button)view.FindViewById(Resource.Id.btn_register_patient);
                btn_register.Click += delegate
                  {
                      LocationManager lm = (LocationManager)GetSystemService(LocationService);
                      Criteria crit = new Criteria { Accuracy = Accuracy.Fine };
                      Location loc = lm.GetLastKnownLocation(lm.GetBestProvider(crit,true));

                      Patient p = new Patient()
                      {
                          name = et_patient_name.Text,
                          age = Convert.ToInt32(et_patient_age.Text),
                          emergPhone = et_patient_emerg.Text,
                          longitude = "" + loc.Longitude,
                          latitude = "" + loc.Latitude,
                          monitor = true
                      };
                      RegisterPatient(builder,p);
                  };
                builder.Show();
            };
        }
        private async void RegisterPatient(AlertDialog ad,Patient patRegister)
        {
            string uri = PatientsNetworkUtils.CONTROLLER_BASE_ADDRESS + "/";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(uri);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
            string patientJson = JsonConvert.SerializeObject(patRegister);
            request.Content = new StringContent(patientJson,
                Encoding.UTF8,
                "application/json");
            var response = httpClient.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            patientadded = true;
            GetPatientList();
            ad.Dismiss();
        }
        private async void GetPatientList()
        {
            lvPatientList = (ListView)FindViewById(Resource.Id.lv_patient_list);
            using (HttpClient htc = new HttpClient())
            {
                string response = await htc.GetStringAsync("http://32d40c6f.ngrok.io/api/Patients");
                Patients = JsonConvert.DeserializeObject<List<Patient>>(response);
                pa = new PatientAdapter(this, Patients);
            }
            lvPatientList.Adapter = pa;
        }
        protected override void OnResume()
        {
            base.OnResume();
            GetPatientList();
        }
    }
}

