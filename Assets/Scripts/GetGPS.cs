using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using DoubleComputation;

public class GetGPS : MonoBehaviour
{
    bool locationIsReady = false;
    bool locationGrantedAndroid = false;
    
    public Vector3d gps_device;

    void Start()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        else
        {
            locationGrantedAndroid = true;
            locationIsReady = NativeGPSPlugin.StartLocation();
        }

        #elif PLATFORM_IOS

        locationIsReady = NativeGPSPlugin.StartLocation();
    
        #endif
    }

    
    void Update()
    {
        //update for gps of your device
        gps_device = new Vector3d(NativeGPSPlugin.GetLatitude(), NativeGPSPlugin.GetLongitude(), NativeGPSPlugin.GetAltitude());

        GameObject.Find("GameObject").GetComponent<Locate>().SendMessage("setDevice", gps_device);
    }
}
