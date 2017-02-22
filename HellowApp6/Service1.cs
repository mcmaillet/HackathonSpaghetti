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

namespace HellowApp6
{
    [Service(Name ="com.xamarin.test.testname")]
    public class Service1 : Service
    {
        private int i;
        public override IBinder OnBind(Intent intent)
        {
            return new Service1Binder(this);
        }
        public int GetBPM()
        {
            i = 143;

            return i;
        }
    }
}