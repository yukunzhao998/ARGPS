using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleComputation;

public class GetGPS : MonoBehaviour
{
    bool locationIsReady = false;
    bool locationGrantedAndroid = false;
    
    public Vector3d gps_device;
    public Vector3d gps_target;

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

        //Latituded, Longtitude, Height
        //hardcode the gps of target here
        gps_target = new Vector3d(22.337750368767, 114.2630499323, 126);
        
    }

    
    void Update()
    {
        //update every frame for gps of your device
        //the gps of device changges dynamically
        gps_device = new Vector3d(NativeGPSPlugin.GetLatitude(), NativeGPSPlugin.GetLongitude(), NativeGPSPlugin.GetAltitude());
        //gps_device = new Vector3d(22.3375, 114.2629);    //for simple test
        //BroadcastMessage("setDevice", gps_device);    // send to child scripts
        GameObject.Find("GameObject_1").GetComponent<Locate>().SendMessage("setDevice", gps_device);
        GameObject.Find("GameObject_1").GetComponent<Locate>().SendMessage("setTarget", gps_target);
    }
}
