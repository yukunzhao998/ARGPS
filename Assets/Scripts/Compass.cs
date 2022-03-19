using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    private Gyroscope _gyro;
    public float _deltaAngle;
    public float angleToNorth;

    private void Start()
    {
        //Set up and enable the gyroscope (check your device has one)
        _gyro = Input.gyro;
        _gyro.enabled = true;
    }

    private void Update()
    {
        Vector3 previousAngles = transform.eulerAngles;
        transform.localRotation = Input.gyro.attitude;
        transform.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
        transform.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
        // We now have the angle of the camera to the north as transform.eulerAngles.y

        angleToNorth = transform.eulerAngles.y;
        GameObject.Find("GameObject_1").GetComponent<Locate>().SendMessage("setAngleToNorth", angleToNorth);

        //float deltaAngle2 = Mathf.DeltaAngle(_camera.transform.eulerAngles.y, transform.eulerAngles.y);//%_target.transform.eulerAngles.y);
        transform.eulerAngles = previousAngles;
        //_deltaAngle = deltaAngle2;
        
        //_target.transform.eulerAngles = new Vector3(0, deltaAngle2, 0);


        // This delta angle does something too, not sure what exactly
        //float deltaAngle = Mathf.DeltaAngle(previousAngles.y, transform.eulerAngles.y);//%_target.transform.eulerAngles.y);
    }

}
