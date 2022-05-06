using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using DoubleComputation;

public class Locate : MonoBehaviour
{
    public Text _log;
    public Button _buttonFrame;
    public Button _buttonFrameHide;
    public Button _buttonClick;
    public Button _buttonClickHide;
    public Camera _camera;
    public GameObject _gameObject1;
    public GameObject _gameObject2;
    public GameObject _tiger1;  //fixed for each click
    public GameObject _tiger2;  //every frame update
    public ARSessionOrigin arSessionOrigin;

    public Vector3d gps_device;
    public Vector3d gps_target;
    public float gps_accuracy;
    public double angleToNorth;
    public double bearing;
    public double dist;

    private double delta_angle1; //1 for tiger1, 2 for tiger 2
    private double delta_dist1;
    private double delta_angle2;
    private double delta_dist2;
    private double rotation_degree;
    private double previous_dist1 = 0.0;
    private double previous_angles1 = 0.0;
    private double previous_dist2 = 0.0;
    private double previous_angles2 = 0.0;
    private Vector3 origin_position = new Vector3(0.0f, 0.0f, 0.0f);
    private Quaternion origin_rotation = Quaternion.Euler(0,0,0);
    private bool model2_appear = false;

    public void setDevice(Vector3d gps_location){
        gps_device = gps_location;
    }

    public void setTarget(Vector3d gps_location){
        gps_target = gps_location;
    }

    public void setAngleToNorth(double angle){
        angleToNorth = angle;
    }

    public void setGPSAccuracy(float accuracy){
        gps_accuracy = accuracy;
    }

    private double getBearing(Vector3d loc_a, Vector3d loc_b){ //input: the GPS location of device and target
        double lat_a = loc_a.x * Mathd.Deg2Rad;
        double lon_a = loc_a.y * Mathd.Deg2Rad;
        double lat_b = loc_b.x * Mathd.Deg2Rad;
        double lon_b = loc_b.y * Mathd.Deg2Rad;
        
        double x = Math.Sin(lon_b-lon_a) * Math.Cos(lat_b);
        double y = Math.Cos(lat_a)*Math.Sin(lat_b) - Math.Sin(lat_a)*Math.Cos(lat_b)*Math.Cos(lon_b-lon_a);
        double bearing = Math.Atan2(x,y) * Mathd.Rad2Deg;     //bearing in the form of degree
        return bearing;
    }

    private double getDistance(Vector3d loc_a, Vector3d loc_b){
        const double EARTH_RADIUS = 6378.137;
        double lon_a = loc_a.y * Mathd.Deg2Rad;
        double lat_a = loc_a.x * Mathd.Deg2Rad;
        double lon_b = loc_b.y * Mathd.Deg2Rad;
        double lat_b = loc_b.x * Mathd.Deg2Rad;
        double x = Math.Sin((lat_a - lat_b)/2.0);
        double y = Math.Sin((lon_a - lon_b)/2.0);

        double dist = EARTH_RADIUS * 1000 * 2 *Math.Asin(Math.Sqrt(x*x + Math.Cos(lat_a)*Math.Cos(lat_b)*y*y));
        //float dist = EARTH_RADIUS * 1000 * Mathf.Acos(Mathf.Sin(lat_a)*Mathf.Sin(lat_b) + Mathf.Cos(lat_a)*Mathf.Cos(lat_b)*Mathf.Cos(lon_a-lon_b));
        return dist;
    }

    private void Start()
    {
        _buttonClick.onClick.AddListener(TaskOnClick);
        _buttonClickHide.onClick.AddListener(TaskOnClickHide);
        _buttonFrame.onClick.AddListener(TaskFrame);
        _buttonFrameHide.onClick.AddListener(TaskFrameHide);

        _gameObject1.SetActive(false);
        _gameObject2.SetActive(false);
    }

    private void Update()
    {   
        _log.text = "Some intermediate variables: "+"\n";
        _log.text += "angle to north: "+angleToNorth.ToString()+"\n";
        _log.text += "latitude: "+gps_device.x.ToString()+" lontitude: "+gps_device.y.ToString()+"\n";
        _log.text += "accrracy: "+gps_accuracy.ToString()+"\n";
        bearing = getBearing(gps_device, gps_target);
        //Debug.Log("bearing: "+bearing.ToString());
        dist = getDistance(gps_device, gps_target);
        _log.text += "distance: "+ dist.ToString()+"\n";
        rotation_degree = (bearing - angleToNorth) % 360;
        if (rotation_degree < 0){
            rotation_degree += 360;
        }
        _log.text += "rotation degree: "+ rotation_degree.ToString() + "\n";

        //first rotate GameObject, then translate (for tiger 1)
        //1. GameObject rotation
        delta_angle1 = (rotation_degree - previous_angles1) % 360;
        //2. cube translation
        delta_dist1 = dist - previous_dist1;
        
        //for tiger 2
        delta_angle2 = (rotation_degree - previous_angles2) % 360;
        delta_dist2 = dist - previous_dist2;
        
        _gameObject2.SetActive(true);
        if(model2_appear == false){
            _tiger2.GetComponent<Renderer>().enabled = false;
        }
        else{
            _tiger2.GetComponent<Renderer>().enabled = true;
        }
        _gameObject2.transform.Rotate(0.0f, (float)delta_angle2, 0.0f, Space.World);
        previous_angles2 = rotation_degree;
        _tiger2.transform.Translate(0.0f, 0.0f, (float)delta_dist2, Space.Self);
        previous_dist2 = dist;

        //another way of translation, but in world space
        //Vector3 cube_pos = _cube.transform.position;
        //cube_pos.z += delta_dist;
        //_cube.transform.position = cube_pos;

    }

    void TaskOnClick()
    {
        _gameObject1.SetActive(true);
        if(dist == 0.0){
            _tiger1.GetComponent<Renderer>().enabled = false;
        }
        else{
            _tiger1.GetComponent<Renderer>().enabled = true;
        }

        _gameObject1.transform.Rotate(0.0f, (float)delta_angle1, 0.0f, Space.World);
        previous_angles1 = rotation_degree;
        _tiger1.transform.Translate(0.0f, 0.0f, (float)delta_dist1, Space.Self);
        previous_dist1 = dist;

        arSessionOrigin.transform.position = origin_position;
        arSessionOrigin.transform.rotation = origin_rotation;
    }

    void TaskOnClickHide()
    {
        _tiger1.GetComponent<Renderer>().enabled = false;
    }
    
    void TaskFrame()
    {
        model2_appear = true;
        //translation and rotation done in update()
        
    }

    void TaskFrameHide()
    {
        model2_appear = false;
    }
}
