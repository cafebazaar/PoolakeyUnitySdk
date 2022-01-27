package com.sample.android.trivialdrivesample;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

import com.farsitel.bazaar.PoolakeyBridge;

public class MainActivity extends AppCompatActivity {

    private PoolakeyBridge bridge;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        bridge = new PoolakeyBridge();
    }
}