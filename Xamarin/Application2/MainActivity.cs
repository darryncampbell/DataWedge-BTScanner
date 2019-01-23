using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace Application2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //  Instance used to communicate from broadcast receiver back to main activity
        public static MainActivity Instance;
        myBroadcastReceiver receiver;
        public static String DW_ACTION = "com.dwbt.app2.ACTION";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            MainActivity.Instance = this;
            receiver = new myBroadcastReceiver();

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

        }

        protected override void OnResume()
        {
            base.OnResume();
            //  Register the broadcast receiver dynamically
            RegisterReceiver(receiver, new IntentFilter(DW_ACTION));
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(receiver);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void DisplayResult(Intent intent)
        {
            //  Output the scanned barcode on the screen.  Bear in mind older JB devices will use the legacy DW parameters on unbranded devices.
            String decodedData = intent.GetStringExtra("com.symbol.datawedge.data_string");
            TextView scanDataTxt = FindViewById<TextView>(Resource.Id.txtScanData);
            scanDataTxt.Text = "" + decodedData;
        }
    }

    //  Broadcast receiver to receive our scanned data from Datawedge
    [BroadcastReceiver(Enabled = true)]
    public class myBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;
            if (action.Equals(MainActivity.DW_ACTION))
            {
                //  A barcode has been scanned
                MainActivity.Instance.RunOnUiThread(() => MainActivity.Instance.DisplayResult(intent));
            }
        }

    }
}

