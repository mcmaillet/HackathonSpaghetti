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

namespace HellowApp6
{
    public interface IGetBPM
    {
        int GetBPM();
        void setMonitoredPatient(Patient patient);
    }
}