using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGPS : MonoBehaviour
{
    public Vector3 gps_device;
    public Vector3 gps_target;

    void Start()
    {
        Input.location.Start(10.0f, 10.0f);
        
        //Latituded, Longtitude, Height
        //hardcode the gps of target here
        gps_target = new Vector3(22.33765f, 114.2631f, 143.434f);
        
    }

    
    void Update()
    {
        //update every frame for gps of your device
        //the gps of device changges dynamically
        gps_device = new Vector3(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.altitude);
        //BroadcastMessage("setDevice", gps_device);    // send to child scripts
        GameObject.Find("GameObject_1").GetComponent<Locate>().SendMessage("setDevice", gps_device);
        GameObject.Find("GameObject_1").GetComponent<Locate>().SendMessage("setTarget", gps_target);
    }
}
