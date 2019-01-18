package com.dwbtscanner.application3;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity {

    private static String DW_ACTION = "com.dwbt.app3.ACTION";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        IntentFilter filter = new IntentFilter();
        filter.addCategory(Intent.CATEGORY_DEFAULT);
        filter.addAction(DW_ACTION);
        registerReceiver(myBroadcastReceiver, filter);
    }

    @Override
    protected void onDestroy()
    {
        super.onDestroy();
        unregisterReceiver(myBroadcastReceiver);
    }

    private BroadcastReceiver myBroadcastReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if (action.equals(DW_ACTION)) {
                //  Received a barcode scan
                try {
                    String decodedData = intent.getStringExtra("com.symbol.datawedge.data_string");
                    final TextView lblScanData = findViewById(R.id.scanData);
                    lblScanData.setText(decodedData);
                } catch (Exception e) {

                }
            }
        }
    };
}
