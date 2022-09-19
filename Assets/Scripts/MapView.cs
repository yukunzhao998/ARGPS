#if UNITY_2018_3_OR_NEWER && PLATFORM_ANDROID
	using UnityEngine.Android;
#endif

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NinevaStudios.GoogleMaps.Internal;
using UnityEngine;
using UnityEngine.UI;
using NinevaStudios.GoogleMaps;
using UnityEngine.SceneManagement;
using DoubleComputation;

public class MapView : MonoBehaviour
{
    GoogleMapsView _map;
    public RectTransform rect;
	Marker _marker;
	public Texture2D icon;

    // Camera position
    public float camPosLat = 22.33719118f;
	public float camPosLng = 114.264811f;
	public float camPosZoom = 15f;
	public float camPosTilt = 0f;
	public float camPosBearing = 0f;

	public double dist;
	private int model_num;
	private Vector3d[] gps_tiger = new Vector3d[10];
	public Vector3d gps_device;

    CameraPosition CameraPosition => new CameraPosition(
        new LatLng(camPosLat, camPosLng),
		camPosZoom,
		camPosTilt,
		camPosBearing);

    void Start()
	{
		// Show the map when the demo starts
		//_map.IsVisible = true;
		OnShow();

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
	}
    

    public void OnShow()
	{
		Dismiss();

		GoogleMapsView.CreateAndShow(CreateMapViewOptions(), RectTransformToScreenSpace(rect), OnMapReady);
	}

	GoogleMapsOptions CreateMapViewOptions()
	{
		var options = new GoogleMapsOptions();

		// Camera position
		options.Camera(CameraPosition);
        
        /*
		options.MapType((GoogleMapType)mapType.value);

		// Bounds
		if (boundsToggle.isOn)
		{
			options.LatLngBoundsForCameraTarget(Bounds);
		}

		options.AmbientEnabled(ambientToggle.isOn);
		options.CompassEnabled(compassToggle.isOn);
		options.LiteMode(liteModeToggle.isOn);
		options.MapToolbarEnabled(mapToolbarToggle.isOn);
		options.RotateGesturesEnabled(rotateGesturesToggle.isOn);
		options.ScrollGesturesEnabled(scrollGesturesToggle.isOn);
		options.TiltGesturesEnabled(tiltGesturesToggle.isOn);
		options.ZoomGesturesEnabled(zoomGesturesToggle.isOn);
		options.ZoomControlsEnabled(zoomControlsToggle.isOn);

		options.MinZoomPreference(float.Parse(minZoom.text));
		options.MaxZoomPreference(float.Parse(maxZoom.text));
        */
		return options;
	}

    static Rect RectTransformToScreenSpace(RectTransform transform)
	{
		Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
		Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
		rect.x -= transform.pivot.x * size.x;
		rect.y -= (1.0f - transform.pivot.y) * size.y;
		rect.x = Mathf.CeilToInt(rect.x);
		rect.y = Mathf.CeilToInt(rect.y);
		return rect;
	}

    void OnMapReady(GoogleMapsView googleMapsView)
	{
		_map = googleMapsView;
		_map.SetPadding(0, 0, 0, 0);

        /*
		var isStyleUpdateSuccess = _map.SetMapStyle(customStyleJson.text);
        */
        EnableAllSettings(_map.UiSettings, true);
        _map.IsMyLocationEnabled = true;
		_map.UiSettings.IsMyLocationButtonEnabled = true;
		_map.OnOrientationChange += () => { _map.SetRect(RectTransformToScreenSpace(rect)); };
		
		AddMarker();
    }

	void AddMarker()
	{
		var piazza = new LatLng(22.337544f, 114.263170f);
		string title_piazza = "Tiger at piazza";
		var LG7 = new LatLng(22.3377718f, 114.2643658f);
		string title_LG7 = "Tiger at LG7 greenfield";
		var terrace = new LatLng(22.33575835013311f, 114.2641477387218f);
		string title_terrace = "Tiger at terrace";
		var shaw_building = new LatLng(22.3342641763769f, 114.2636490802f);
		string title_shaw_building = "Tiger at Shaw building";
		var north_gate = new LatLng(22.3384411f, 114.2623102f);
		string title_north_gate = "Tiger at north gate";
		var beach_promenade = new LatLng(22.337760f, 114.268816f);
		string title_beach_promenade = "Tiger at beach promenade";
		var business_building = new LatLng(22.3335102992291f, 114.264752715932f);
		string title_business_building = "Tiger at business building";
		var lovers_walk = new LatLng(22.3371832f, 114.2647986f);
		string title_lovers_walk = "Tiger at lovers' walk";
		var pond = new LatLng(22.3370043f, 114.2670028f);
		string title_pond = "Tiger at pond";
		var bridge_link = new LatLng(22.3374853f, 114.2655530f);
		string title_bridge_link = "Tiger at bridge link";

		_map.AddMarker(NewMarkerOptions(piazza, title_piazza));
		_map.AddMarker(NewMarkerOptions(LG7, title_LG7));
		_map.AddMarker(NewMarkerOptions(terrace, title_terrace));
		_map.AddMarker(NewMarkerOptions(shaw_building, title_shaw_building));
		_map.AddMarker(NewMarkerOptions(north_gate, title_north_gate));
		_map.AddMarker(NewMarkerOptions(beach_promenade, title_beach_promenade));
		_map.AddMarker(NewMarkerOptions(business_building, title_business_building));
		_map.AddMarker(NewMarkerOptions(lovers_walk, title_lovers_walk));
		_map.AddMarker(NewMarkerOptions(pond, title_pond));
		_map.AddMarker(NewMarkerOptions(bridge_link, title_bridge_link));

	}

	public MarkerOptions NewMarkerOptions(LatLng position, string title)
	{
		return new MarkerOptions()
			.Position(position)
			//.Icon(ImageDescriptor.FromAsset("map-marker-icon.png"))
			.Icon(ImageDescriptor.FromTexture2D(icon))
			.Alpha(0.8f) // make semi-transparent image
			.Anchor(0.5f, 1f) // anchor point of the image
			.InfoWindowAnchor(0.5f, 1f)
			.Draggable(false)
			.Flat(false)
			.Rotation(0f)
			.Snippet("")
			.Title(title)
			.Visible(true)
			.ZIndex(1f);
	}

    static void EnableAllSettings(UiSettings settings, bool enable)
	{
		// Buttons/other
		settings.IsCompassEnabled = enable;
		settings.IsIndoorLevelPickerEnabled = enable;
		settings.IsMapToolbarEnabled = enable;
		settings.IsMyLocationButtonEnabled = enable;
		settings.IsZoomControlsEnabled = enable;

		// Gestures
		settings.IsRotateGesturesEnabled = enable;
		settings.IsScrollGesturesEnabled = enable;
		settings.IsTiltGesturesEnabled = enable;
		settings.IsZoomGesturesEnabled = enable;
		settings.SetAllGesturesEnabled(enable);
	}

    void Dismiss()
	{
		if (_map != null)
		{
			_map.Dismiss();
			_map = null;
		}
	}
    
	public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
		_map.IsVisible = false;
    }

	public void setDevice(Vector3d gps_coord)
	{ 
		gps_device = gps_coord;
	}

	private double getDistance(Vector3d loc_a, Vector3d loc_b)
	{
        const double EARTH_RADIUS = 6378.137;
        double lon_a = loc_a.y * Mathd.Deg2Rad;
        double lat_a = loc_a.x * Mathd.Deg2Rad;
        double lon_b = loc_b.y * Mathd.Deg2Rad;
        double lat_b = loc_b.x * Mathd.Deg2Rad;
        double x = Math.Sin((lat_a - lat_b)/2.0);
        double y = Math.Sin((lon_a - lon_b)/2.0);

        double dist = EARTH_RADIUS * 1000 * 2 *Math.Asin(Math.Sqrt(x*x + Math.Cos(lat_a)*Math.Cos(lat_b)*y*y));
        return dist;
    }

	private void Update()
    {   
        for(model_num=0; model_num<10; model_num++){
            dist = getDistance(gps_device, gps_tiger[model_num]);
            if (dist < 20){
				_map.IsVisible = false;
				SceneManager.LoadScene("TigerDisplay");
            }
        }
	}
}
