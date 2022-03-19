using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGPS : MonoBehaviour
{
    public Vector3 gps_device;
    public Vector3 gps_target;

    void Start()
    {
        //Latituded, Longtitude, Height
        gps_target = new Vector3(22.33456f, 114.2648f, 126.0496f);
        //hardcode the gps of device here
        gps_device = new Vector3(22.33462f, 114.2649f, 126.1958f);

        //BroadcastMessage("setDevice", gps_device);    // send to child scripts
        GameObject.Find("GameObject_1").GetComponent<Locate>().SendMessage("setDevice", gps_device);
        GameObject.Find("GameObject_1").GetComponent<Locate>().SendMessage("setTarget", gps_target);
    }

    
    void Update()
    {
        //update every frame for gps of your device
        
    }
}
