using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class TestingArdu : MonoBehaviour 
{
	public string DistanceF;
	public string DistanceL;
	public string DistanceR;
	public int FrontDist = 300;
	public int LeftDist = 300;
	public int RightDist = 300;
	public GameObject LeftPositioner;
	public GameObject RightPositioner;
	public GameObject Obstacle;
	GameObject FinstObject;
	GameObject LinstObject;
	GameObject RinstObject;
	GameObject temp;
	Vector3 positioner;
	float time = 0;
	bool timer = false;
	string s;
	bool b = true;
	public Camera cam;
	public SerialPort spp = new SerialPort("COM5", 9600);
	void Start () 
	{
		spp.Open();
		spp.ReadTimeout = 1;
	}
	// Update is called once per frame
	void FixedUpdate() 
	{
		if (!timer && spp.IsOpen)
		{
			time += Time.deltaTime;
			//Debug.Log("Time : " + time);
			if(time > 2.1)
				timer = true;
			return;
		} 
		else 
		{
			if (timer) 
			{
				try {
					while(b == true)
					{
						s = (spp.ReadLine()).ToString();
						Debug.Log("s : " + s);
						if(s.StartsWith("0"))
						{
							b = false;
						}
					}
					if(b == false)
					{
						Debug.Log("00");
						DistanceL = (spp.ReadLine()).ToString();
						Debug.Log("left : " + DistanceL);
						DistanceF = (spp.ReadLine()).ToString();
						Debug.Log("front : " + DistanceF);
						DistanceR = (spp.ReadLine()).ToString();
						Debug.Log("right : " + DistanceR);
						b = true;
					}
					if(DistanceF.EndsWith("F"))
					{
						FrontDist = int.Parse(DistanceF.Remove(DistanceF.Length-1));
						Debug.Log("FD : " + FrontDist);
					}
					if(DistanceL.EndsWith("L"))
					{
						LeftDist = int.Parse(DistanceL.Remove(DistanceL.Length-1));
						Debug.Log("LD : " + LeftDist);
					}
					if(DistanceR.EndsWith("R"))
					{
						RightDist = int.Parse(DistanceR.Remove(DistanceR.Length-1));
						Debug.Log("RD : " + RightDist);
					}
					
					if (FrontDist <= 500) 
					{
						//Debug.Log("Instantiating Front...");
						positioner = transform.TransformPoint(Vector3.forward * FrontDist);
						FinstObject = (GameObject)Instantiate (Obstacle, positioner, transform.rotation);
						timer = false;
						time = 0;
					}
					if (LeftDist <= 500) 
					{
						//Debug.Log("Instantiating Left...");
						positioner = LeftPositioner.transform.TransformPoint(Vector3.forward * LeftDist);
						LinstObject = (GameObject)Instantiate (Obstacle, positioner, LeftPositioner.transform.rotation);
						timer = false;
						time = 0;
					}
					if (RightDist <= 500) 
					{
						//Debug.Log("Instantiating Right...");
						//RP.transform = LP.transform.forward * RightDist;
						positioner = RightPositioner.transform.TransformPoint(Vector3.forward * RightDist);
						RinstObject = (GameObject)Instantiate (Obstacle, positioner, RightPositioner.transform.rotation);
						timer = false;
						time = 0;
					}
				} 
				catch (System.Exception) {
					
				}
			}
		}
	}
	
}
