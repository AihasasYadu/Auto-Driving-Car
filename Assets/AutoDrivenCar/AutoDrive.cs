using UnityEngine;
using System.Collections;
using System.IO.Ports;
using Uniduino;

public class AutoDrive : MonoBehaviour 
{
	public TestingArdu test;
	int motorPin1= 9;
	int motorPin2= 10;
	int motorPin3= 6;
	int motorPin4= 5;
	float angle = 0.2f;
	public float sensorLen = 100f;
	public float speed = 0.25f;
	//public float sensorSP = 5.5f;
	//public float sensorAngle = 30f;
	public GameObject car;
	int maxDist = 0;
	bool check = true;
	int left = 0;
	float i = 0;
	Component T;
	public GameObject LR;
	public GameObject RR;
	public Arduino ar;
	void Start () 
	{
		test.spp.ReadTimeout = 1;
		ar = Arduino.global;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(check == false && i < 1.2)
		{
			Backward();
			i += Time.deltaTime;
		}
		else
		{
			i = 0;
			check = true;
			Forward();
			Sensors();
		}
	}
	
	void Sensors()
	{
		try{
			int fd = test.FrontDist;
			int ld = test.LeftDist;
			int rd = test.RightDist;
			int[] distances = new int[]{fd, ld, rd};
			maxDist = Mathf.Max(distances);
			int F = 1,L = 1,R = 1;
			RaycastHit r;
			Vector3 FsensorPos = transform.position;
			//Front Sensor
			if(Physics.Raycast(FsensorPos, transform.TransformDirection(Vector3.forward) , out r, sensorLen))
			{
				Debug.DrawLine(FsensorPos, r.point);
				//Debug.Log("Ray Done" + r.distance);
				if(r.collider.gameObject.CompareTag("Player"))
				{
					//transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,50,0), 1 * Time.deltaTime);
					//sp.WriteLine("In Front");
					//Debug.Log("Front 0");
					transform.Rotate(0, 0.2f, 0);
					if(r.distance < 80)
						F = 0;
				}
			}
			//Left Sensor
			/*if(Physics.Raycast(sensorPos, transform.TransformDirection(Vector3.forward) , out r, sensorLen))
			{
				Debug.DrawLine(sensorPos, r.point);
				if(r.collider.gameObject.CompareTag("Player"))
				{
					//Steer(2);
					//sp.WriteLine("In Left");
				}
			}*/
			Vector3 LsensorPos = LR.transform.position;
			//Left Angular Sensor
			if(Physics.Raycast(LsensorPos, LR.transform.TransformDirection(Vector3.forward) , out r, sensorLen-5))
			{
				Debug.DrawLine(LsensorPos, r.point);
				if(r.collider.gameObject.CompareTag("Player"))
				{
					//Steer(3);
					//Debug.Log("Left 0");
					if(r.distance < 80)
						L = 0;
				}
			}
			
			Vector3 RsensorPos = RR.transform.position;
			//Right Sensor
			/*if(Physics.Raycast(sensorPos, transform.TransformDirection(Vector3.forward) , out r, sensorLen))
			{
				Debug.DrawLine(sensorPos, r.point);
				if(r.collider.gameObject.CompareTag("Player"))
				{
				
					//Steer(2);
					//sp.WriteLine("In Right");
				}
			}*/
			
			//Right Angular Sensor
			//Quaternion.AngleAxis(-sensorAngle , transform.up) * transform.forward
			if(Physics.Raycast(RsensorPos, RR.transform.TransformDirection(Vector3.forward), out r, sensorLen-5))
			{
				Debug.DrawLine(RsensorPos, r.point);
				if(r.collider.gameObject.CompareTag("Player"))
				{
					//Steer(3);
					//Debug.Log("Right 0");
					if(r.distance < 80) 
						R = 0;
				}
			}
			
			if (F == 0) 
			{
				if (L == 0 && R == 0) 
				{
					check = false;
					if(maxDist == test.LeftDist)
					{
						left = 1;
						Steer(3f);
						Backward();
						//test.spp.WriteLine("BLX");
						ar.analogWrite(motorPin1, 0);
						ar.analogWrite(motorPin2, 255);
						ar.analogWrite(motorPin3, 0);
						ar.analogWrite(motorPin4, 255);
					}
					else if(maxDist == test.RightDist)
					{
						left = 2;
						SteerL (3f);
						Backward();
						//test.spp.WriteLine("BRX");
						ar.analogWrite(motorPin1, 255);
						ar.analogWrite(motorPin2, 0);
						ar.analogWrite(motorPin3, 0);
						ar.analogWrite(motorPin4, 255);
					}
				}
				if (L != 0 && R == 0) 
				{
					SteerL (3f);
					Forward();
					//test.spp.WriteLine ("FLX");
					ar.analogWrite(motorPin1, 0);
					ar.analogWrite(motorPin2, 255);
					ar.analogWrite(motorPin3, 255);
					ar.analogWrite(motorPin4, 0);
					
				}
				if (L == 0 && R != 0) 
				{
					Steer (3f);
					Forward();
					//test.spp.WriteLine ("FRX");
					ar.analogWrite(motorPin1, 255);
					ar.analogWrite(motorPin2, 0);
					ar.analogWrite(motorPin3, 255);
					ar.analogWrite(motorPin4, 0);
				}
			}
			else
			{
				if (L != 0 && R == 0) 
				{
					SteerL (3f);
					Forward ();
					//test.spp.WriteLine ("FLX");
					ar.analogWrite(motorPin1, 0);
					ar.analogWrite(motorPin2, 255);
					ar.analogWrite(motorPin3, 255);
					ar.analogWrite(motorPin4, 0);
				}
				if (L == 0 && R != 0) 
				{
					Steer(3f);
					Forward();
					//test.spp.WriteLine ("FRX");
					ar.analogWrite(motorPin1, 0);
					ar.analogWrite(motorPin2, 255);
					ar.analogWrite(motorPin3, 255);
					ar.analogWrite(motorPin4, 0);
				}
				if(L == 0 && R == 0)
				{
					check = false;
					if(maxDist == test.LeftDist)
					{
						left = 1;
						Steer(3f);
						Backward();
						//test.spp.WriteLine ("BLX");
						ar.analogWrite(motorPin1, 0);
						ar.analogWrite(motorPin2, 255);
						ar.analogWrite(motorPin3, 0);
						ar.analogWrite(motorPin4, 255);
					}
					else if(maxDist == test.RightDist)
					{
						left = 2;
						SteerL (3f);
						Backward();
						//test.spp.WriteLine ("BRX");
						ar.analogWrite(motorPin1, 255);
						ar.analogWrite(motorPin2, 0);
						ar.analogWrite(motorPin3, 0);
						ar.analogWrite(motorPin4, 255);
					}
				}
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Exception : " + e);
		}
	}
	
	void Backward()
	{
		transform.Translate(0, 0, -speed);
		//Debug.Log("Backward");
		if(left == 1)
			Steer(3);
		else if(left == 2)
			SteerL(3);
	}
	
	void Forward()
	{
		transform.Translate(0, 0, speed);
		//Debug.Log("Forward");
	}
	
	void Steer(float x)
	{
		angle += x * Time.deltaTime/10000000;
		transform.Rotate (0, angle, 0);
		//sp.WriteLine ("100R");
	}
	
	void SteerL(float x)
	{
		angle += x * Time.deltaTime/10000000;
		transform.Rotate (0, -angle, 0);
	}
}