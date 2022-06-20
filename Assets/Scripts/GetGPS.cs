using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleComputation;

public class GetGPS : MonoBehaviour
{
    bool locationIsReady = false;
    bool locationGrantedAndroid = false;
    
    public Vector3d gps_device;
    public Vector3d[] gps_tiger = new Vector3d[9];
    public float gps_accuracy;

    void Start()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            //dialog = new GameObject();
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
        gps_accuracy = NativeGPSPlugin.GetAccuracy();

        GameObject.Find("GameObject").GetComponent<Locate>().SendMessage("setDevice", gps_device);
        GameObject.Find("GameObject").GetComponent<Locate>().SendMessage("setGPSAccuracy", gps_accuracy);
    }
}
