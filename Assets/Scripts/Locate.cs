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
    public Button _buttonClick;
    public Button _buttonHide;
    public Button _buttonRotate;

    public Camera _camera;
    public GameObject _gameObject1;  //fixed for each click
    public GameObject _tiger1, _tiger2, _tiger3, _tiger4, _tiger5, _tiger6, _tiger7, _tiger8, _tiger9;

    public ARSessionOrigin arSessionOrigin;
    public Vector3d gps_device;
    private int model_num;
    private Vector3d[] gps_tiger = new Vector3d[9];  //modify the number of the models
    public float gps_accuracy;
    public double angleToNorth;
    public double bearing;
    public double dist;
    private double rotation_degree;
    private Vector3 origin_position = new Vector3(0.0f, 0.0f, 0.0f);
    private Quaternion origin_rotation = Quaternion.Euler(0,0,0);

    public void setDevice(Vector3d gps_coord){
        gps_device = gps_coord;
    }

    public void setAngleToNorth(double angle){
        angleToNorth = angle;
    }

    public void setGPSAccuracy(float accuracy){
        gps_accuracy = accuracy;
    }

    private double getBearing(Vector3d loc_a, Vector3d loc_b){ //input: the GPS location of device and models
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
        _buttonHide.onClick.AddListener(TaskHide);
        _buttonRotate.onClick.AddListener(TaskRotate);

        //hardcode Latitude, Longtitude, Height
        gps_tiger[5] = new Vector3d(22.337750368767, 114.2630499323, 0); //piazza
        gps_tiger[0] = new Vector3d(22.3378280163879, 114.264188693905, 0); //green field next to LG7
        gps_tiger[2] = new Vector3d(22.3336070432467, 114.263200065615, 0); //shaw building
        gps_tiger[4] = new Vector3d(1, 1, 1); //north gate
        gps_tiger[7] = new Vector3d(22.337805342394, 114.268795309812, 0); //beach promenade
        gps_tiger[1] = new Vector3d(22.3334290432487, 114.26478407823, 0); //business building entrance
        gps_tiger[3] = new Vector3d(22.3361895084094, 114.26409054065, 0); //lovers' walk
        gps_tiger[6] = new Vector3d(22.3370086564405, 114.266969178984, 0); //pond behind hall4
        gps_tiger[8] = new Vector3d(1, 1, 1); //bridge link
    
        _gameObject1.SetActive(false);
    }

    private void Update()
    {   
        //computation for bearing and distance
        for(model_num=0; model_num<9; model_num++){
            dist = getDistance(gps_device, gps_tiger[model_num]);
            if (dist < 25){
                bearing = getBearing(gps_device, gps_tiger[model_num]);
                rotation_degree = (bearing - angleToNorth) % 360;
                if (rotation_degree < 0){
                    rotation_degree += 360;
                    }
                break;
            }
        }

        //GUI output
        _log.text = "Intermediate variables: "+ "\n";
        _log.text += "angle to north: "+ angleToNorth.ToString()+ "\n";
        _log.text += "latitude: "+ gps_device.x.ToString()+ " lontitude: "+ gps_device.y.ToString()+ "\n";
        _log.text += "accuracy: "+ gps_accuracy.ToString()+ "\n";
        if(dist < 25){
            _log.text += "distance: "+ dist.ToString()+ "\n";
        }
        else{
            _log.text += "distance: closest distance bigger than 25m"+ "\n";
        }
        _log.text += "rotation degree: "+ rotation_degree.ToString()+ "\n";
    }

    void TaskOnClick()
    {
        _gameObject1.SetActive(true);
        GameObject[] tigers = {_tiger1, _tiger2, _tiger3, _tiger4, _tiger5, _tiger6, _tiger7, _tiger8, _tiger9};
        for(int i=0; i<9; i++){
            tigers[i].GetComponent<Renderer>().enabled = false;
        }
        if(dist < 25){
            tigers[model_num].GetComponent<Renderer>().enabled = true;
            //rotation and translation
            _gameObject1.transform.rotation = Quaternion.Euler(0, (float)rotation_degree, 0);
            double delta_dist = dist - tigers[model_num].transform.position.z;
            tigers[model_num].transform.Translate(0.0f, 0.0f, (float)delta_dist, Space.Self);
            tigers[model_num].transform.Rotate(0, -(float)angleToNorth, 0);
        }



        arSessionOrigin.transform.position = origin_position;
        arSessionOrigin.transform.rotation = origin_rotation;
    }

    void TaskHide()
    {
        GameObject[] tigers = {_tiger1, _tiger2, _tiger3, _tiger4, _tiger5, _tiger6, _tiger7, _tiger8, _tiger9};
        tigers[model_num].GetComponent<Renderer>().enabled = false;
    }

    void TaskRotate()
    {
        GameObject[] tigers = {_tiger1, _tiger2, _tiger3, _tiger4, _tiger5, _tiger6, _tiger7, _tiger8, _tiger9};
        tigers[model_num].transform.Rotate(0, 45, 0);
    }
}