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

namespace HellowApp6
{
    [Activity(Label = "MainActivity", MainLauncher =true,Icon ="@drawable/Icon")]
    public class MainActivity : Activity
    {
        List<Patient> Patients;
        PatientAdapter pa;
        ListView lvPatientList;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            GetPatientList();
        }
        private async void GetPatientList()
        {
            lvPatientList = (ListView)FindViewById(Resource.Id.lv_patient_list);
            using (HttpClient htc = new HttpClient())
            {
                pa = null;//does this work
                string response = await htc.GetStringAsync("https://aa798a67.ngrok.io/api/Patients");
                Patients = JsonConvert.DeserializeObject<List<Patient>>(response);
                pa = new PatientAdapter(this, Patients);
            }
            lvPatientList.Adapter = pa;
        }
    }
}

