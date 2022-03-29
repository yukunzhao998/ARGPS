using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Locate : MonoBehaviour
{
    public const float PI = 3.1415926f;
    public Text _log;
    public Button _button;
    public Camera _camera;
    public GameObject _gameObject;
    public GameObject _cube;

    public Vector3 gps_device;
    public Vector3 gps_target;
    public float angleToNorth;
    public float bearing;
    public float dist;
    public float delta_angle;
    public float delta_dist;
    public float rotation_degree;

    private float previous_dist = 0.0f;
    private float previous_angles = 0.0f;

    public void setDevice(Vector3 gps_location){
        gps_device = gps_location;
    }

    public void setTarget(Vector3 gps_location){
        gps_target = gps_location;
    }

    public void setAngleToNorth(float angle){
        angleToNorth = angle;
    }

    private float getBearing(Vector3 loc_a, Vector3 loc_b){ //input: the GPS location of device and target
        float lat_a = loc_a.x * Mathf.Deg2Rad;
        float lon_a = loc_a.y * Mathf.Deg2Rad;
        float lat_b = loc_b.x * Mathf.Deg2Rad;
        float lon_b = loc_b.y * Mathf.Deg2Rad;
        
        float x = Mathf.Sin(lon_b-lon_a) * Mathf.Cos(lat_b);
        float y = Mathf.Cos(lat_a)*Mathf.Sin(lat_b) - Mathf.Sin(lat_a)*Mathf.Cos(lat_b)*Mathf.Cos(lon_b-lon_a);
        float bearing = Mathf.Atan2(x,y) * Mathf.Rad2Deg;     //bearing in the form of degree
        return bearing;
    }

    private float getDistance(Vector3 loc_a, Vector3 loc_b){
        const float EARTH_RADIUS = 6378.137f;
        float lon_a = loc_a.y * Mathf.Deg2Rad;
        float lat_a = loc_a.x * Mathf.Deg2Rad;
        float lon_b = loc_b.y * Mathf.Deg2Rad;
        float lat_b = loc_b.x * Mathf.Deg2Rad;
        float x = Mathf.Sin((lat_a - lat_b)/2.0f);
        float y = Mathf.Sin((lon_a - lon_b)/2.0f);

        float dist = EARTH_RADIUS * 1000 * 2 *Mathf.Asin(Mathf.Sqrt( x*x + Mathf.Cos(lat_a)*Mathf.Cos(lat_b)*y*y));
        //float dist = EARTH_RADIUS * 1000 * Mathf.Acos(Mathf.Sin(lat_a)*Mathf.Sin(lat_b) + Mathf.Cos(lat_a)*Mathf.Cos(lat_b)*Mathf.Cos(lon_a-lon_b));
        return dist;
    }

    private void Start()
    {
        _button.onClick.AddListener(TaskOnClick);
        _gameObject.SetActive(false);
        
    }

    private void Update()
    {   
        _log.text = "angle to north: "+angleToNorth.ToString()+"\n";
        
        bearing = getBearing(gps_device, gps_target);
        //Debug.Log("bearing: "+bearing.ToString());
        dist = getDistance(gps_device, gps_target);
        //dist = 2;  unline this to check only the bearing
        //Debug.Log("distance: "+ dist.ToString());
        rotation_degree = (bearing - angleToNorth) % 360;

        //first rotate GameObject, then translate cube
        //1. GameObject rotation
        delta_angle = (rotation_degree - previous_angles) % 360;
        
        //2. cube translation
        delta_dist = dist - previous_dist;
        
        //another way of translation, but in world space
        //Vector3 cube_pos = _cube.transform.position;
        //cube_pos.z += delta_dist;
        //_cube.transform.position = cube_pos;

    }

    void TaskOnClick()
    {
        _gameObject.SetActive(true);
        
        _gameObject.transform.Rotate(0.0f, delta_angle, 0.0f, Space.World);
        previous_angles = rotation_degree;
        _cube.transform.Translate(0.0f, 0.0f, delta_dist, Space.Self);
        previous_dist = dist;
        //_gameObject.transform.eulerAngles= new Vector3(0, -_deltaAngle, 0);
    }
    
}
