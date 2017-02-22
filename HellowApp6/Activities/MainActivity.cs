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
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            GetPatientList();
            
            //Patients.Add(new Patient("1", "Bob Rob", "4001", "Robert Guy", 45, "Here is a description for you"));
            //Patients.Add(new Patient("3", "Angela", "B12", "Emergency Contact", 88, "This guy is old as fuck"));
            //Patients.Add(new Patient("888", "Connie Smith", "F13", "Contact #1", 76, ""));
            //Patients.Add(new Patient("12", "Daniel Thomson", "MAIN", "", 66, "We should probably provide a description for this guy. He's old as fuck"));
            //Patients.Add(new Patient("9", "STEVE STEVE", "Right here", "NONE", 80, "Some extra information"));
            //Patients.Add(new Patient("100", "Judy", "LOCATION", "N/A", 71, ""));
            //Patients.Add(new Patient("32", "Person Name", "N/A", "403-112-3434", 92, "Bites."));
            //Patients.Add(new Patient("44", "Richard Little", "dunno", "587-999-1234", 66, "Dick for short"));
        }
        private async void GetPatientList()
        {
            var lvPatientList = (ListView)FindViewById(Resource.Id.lv_patient_list);
            HttpClient htc = new HttpClient();
            string response = await htc.GetStringAsync("https://aa798a67.ngrok.io/api/Patients");
            Patients = JsonConvert.DeserializeObject<List<Patient>>(response);
            pa = new PatientAdapter(this, Patients);
            
            lvPatientList.Adapter = pa;
        }
    }
}

