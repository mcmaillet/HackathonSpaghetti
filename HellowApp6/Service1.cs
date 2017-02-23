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

using HellowApp6.Entities;
using HellowApp6.NetworkUtils;
using System.Net.Http;
using Newtonsoft.Json;

namespace HellowApp6
{
    [Service(Name ="service.getbpm")]
    public class Service1 : Service
    {
        private Patient _patient;
        private List<double> BPM = new List<double>();
        public override IBinder OnBind(Intent intent)
        {
            return new Service1Binder(this);
        }
        public double GetBPM()
        {
            if (BPM.Count == 0)
            {
                return 0;
            }

            return BPM[BPM.Count - 1];
        }
        public void setMonitoredPatient(Patient patient)
        {
            _patient = patient;
            var timer = new System.Threading.Timer(
                e => getBpmFromNetwork(_patient),
                    null,
                    TimeSpan.Zero,
                    TimeSpan.FromSeconds(5));
        }
        public async void getBpmFromNetwork(Patient patient)
        {
            string patientVitalsUrl = PatientsNetworkUtils.CONTROLLER_BASE_ADDRESS + "/Readings?device=" + patient.id;

            HttpClient client = PatientsNetworkUtils.GetClient(patientVitalsUrl);

            string response = await client.GetStringAsync("");

            var vitals = JsonConvert.DeserializeObject<IEnumerable<Vital>>(response).ToList();

            if (vitals.Count == 0)
            {
                return;
            }
            BPM.Add(vitals[0].value);
        }
    }
    public class Vital
    {
        public string meaning { get; set; }
        public double value { get; set; }
        public object received { get; set; }
    }
}