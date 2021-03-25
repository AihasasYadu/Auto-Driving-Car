using UnityEngine;
using System.Collections;
using System.IO.Ports;
using Uniduino;

public class Ultra_Ardu : MonoBehaviour 
{
	// Use this for initialization
	public Arduino arduino;
	int trig = 9;
	int echo = 10;
	long duration;
	int distance;
	SerialPort ser = new SerialPort("COM5",9600);
	void Start () 
	{
		arduino = Arduino.global;
		arduino.Setup (ConfigurePins);
		StartCoroutine (Ultra ());
	}

	void ConfigurePins ()
	{
		arduino.pinMode (trig, PinMode.OUTPUT);
		arduino.pinMode (echo, PinMode.INPUT);
	}

	IEnumerator Ultra()
	{
		arduino.digitalWrite(trig, Arduino.LOW);
		yield return new WaitForSeconds(2);
		// Sets the trigPin on HIGH state for 10 micro seconds
		arduino.digitalWrite(trig, Arduino.HIGH);
		yield return new WaitForSeconds(2);
		arduino.digitalWrite(trig, Arduino.LOW);
		// Reads the echoPin, returns the sound wave travel time in microseconds
		//duration = Arduino.pulseIn(echo, Arduino.HIGH);
		// Calculating the distance
		//distance= duration*0.034/2;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
