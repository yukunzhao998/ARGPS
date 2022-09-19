using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using DoubleComputation;
using UnityEngine.SceneManagement;

public class Locate : MonoBehaviour
{
    public Camera _camera;
    public ARSessionOrigin arSessionOrigin;
    public GameObject _gameObject1;  //parent of tigers, fixed for each click
    public GameObject _tiger1, _tiger2, _tiger3, _tiger4, _tiger5, _tiger6, _tiger7, _tiger8, _tiger9, _tiger10;

    public Button _buttonClick;

    public Vector3d gps_device;
    private int model_num;
    private Vector3d[] gps_tiger = new Vector3d[10];  //modify the number of the models
    public float angleToNorth;
    public double bearing;
    public double dist;
    private double rotation_degree;

    public void setDevice(Vector3d gps_coord){
        gps_device = gps_coord;
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
        Input.location.Start();
        Input.compass.enabled = true;

        //hardcode Latitude, Longtitude, Height
        gps_tiger[5] = new Vector3d(22.337544, 114.263170, 0); //piazza
        gps_tiger[0] = new Vector3d(22.3377718, 114.2643658, 0); //green field next to LG7
        gps_tiger[9] = new Vector3d(22.33575835013311, 114.2641477387218, 0); //terrace at fifth floor
        gps_tiger[2] = new Vector3d(22.3342641763769, 114.2636490802, 0); //shaw building
        gps_tiger[4] = new Vector3d(22.3384411, 114.2623102, 0); //north gate
        gps_tiger[7] = new Vector3d(22.337760, 114.268816, 0); //beach promenade
        gps_tiger[1] = new Vector3d(22.3335102992291, 114.264752715932, 0); //business building entrance
        gps_tiger[3] = new Vector3d(22.3371832, 114.2647986, 0); //lovers' walk
        gps_tiger[6] = new Vector3d(22.3370043, 114.2670028, 0); //pond behind hall4
        gps_tiger[8] = new Vector3d(22.3374853, 114.2655530, 0); //bridge link
    
        _gameObject1.SetActive(false);
    }

    private void Update()
    {   
        angleToNorth = Input.compass.trueHeading;

        //computation for bearing and distance
        for(model_num=0; model_num<10; model_num++){
            dist = getDistance(gps_device, gps_tiger[model_num]);
            if (dist < 25){
                bearing = getBearing(gps_device, gps_tiger[model_num]);
                rotation_degree = bearing - angleToNorth;
                break;
            }
        }

    }

    void TaskOnClick()
    {
        _gameObject1.SetActive(true);
        GameObject[] tigers = {_tiger1, _tiger2, _tiger3, _tiger4, _tiger5, _tiger6, _tiger7, _tiger8, _tiger9, _tiger10};

        for(int i=0; i<10; i++){
            tigers[i].SetActive(false);
            //tigers[i].GetComponent<Renderer>().enabled = false;
        }

        if(dist < 25){
            tigers[model_num].SetActive(true);

            //rotation and translation
            _gameObject1.transform.rotation = Quaternion.Euler(0, (float)rotation_degree+_camera.transform.eulerAngles.y, 0);
            tigers[model_num].transform.localPosition = new Vector3(0.0f, 0.0f, (float)dist);

            //rotation for each model to face towards north
            tigers[model_num].transform.localRotation = Quaternion.Euler(0, -(float)bearing, 0);
        }
        
        arSessionOrigin.transform.position = new Vector3(arSessionOrigin.transform.position.x-_camera.transform.position.x, 0, arSessionOrigin.transform.position.z-_camera.transform.position.z);

    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}