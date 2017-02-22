using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using HellowApp6;
using HellowApp6.Entities;

using Java.Lang;

using Newtonsoft.Json;

namespace HellowApp6.Adapters
{
    public class PatientAdapter : BaseAdapter
    {
        List<Patient> Patients;
        Activity A;
        Switch sw_monitor;
        public PatientAdapter(Activity a, List<Patient> p)
        {
            Patients = p;
            A = a;
        }
        public override int Count
        {
            get
            {
                return Patients.Count;
            }
        }
        public override Java.Lang.Object GetItem(int i)
        {
            return null;
        }
        public override long GetItemId(int i)
        {
            return (long)i;
        }
        public override View GetView(int i, View convertView, ViewGroup parent)
        {
            var view = convertView ?? A.LayoutInflater.Inflate(Resource.Layout.lviPatient, parent, false);
            var tv_name = view.FindViewById<TextView>(Resource.Id.tv_name);
            var iv_info = view.FindViewById(Resource.Id.iv_info);
            sw_monitor = view.FindViewById<Switch>(Resource.Id.sw_monitor);

            sw_monitor.Click += delegate
            {
                UpdatePatient(Patients[i]);
            };

            iv_info.Click += delegate
            {
                var patient = Patients[i];
                Intent I = new Intent(A, typeof(NextActivity));
                I.PutExtra("details", JsonConvert.SerializeObject(patient));
                A.StartActivity(I);
            };

            tv_name.Text = Patients[i].name;

            return view;
        }
        private async void UpdatePatient(Patient PATupdate)
        {
            var updatePatient = PATupdate;
            updatePatient.monitor = sw_monitor.Checked;
            var JSONpatient = JsonConvert.SerializeObject(updatePatient);

            HttpClient htc = new HttpClient();
            htc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Post, "");
            hrm.Content = new StringContent(JSONpatient,Encoding.UTF8,"application/json");

            HttpResponseMessage response = await htc.PostAsync("https://aa798a67.ngrok.io/api/Patients",hrm.Content);
        }
    }
}