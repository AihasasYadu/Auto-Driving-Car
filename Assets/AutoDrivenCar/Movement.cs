using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

	public float maxSteerAngle = 45f;
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;
	public float maxMotorTorque = 80f;
	public float maxBrakeTorque = 150f;
	public float currentSpeed;
	public float maxSpeed = 100f;
	public Vector3 centerOfMass;
	public bool isBraking = false;
	bool avoiding;
	
	[Header("Sensors")]
	public float sensorLength = 3f;
	public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
	public float frontSideSensorPosition = 0.2f;
	public float frontSensorAngle = 30f;

	
	private void FixedUpdate() {
		Sensors();
		Drive();
		//Braking();
	}
	
	private void Sensors() {
		RaycastHit hit;
		Vector3 sensorStartPos = transform.position;
		sensorStartPos += transform.forward * frontSensorPosition.z;
		sensorStartPos += transform.up * frontSensorPosition.y;
		float avoidMultiplier = 0;
		avoiding = false;
		
		//front right sensor
		sensorStartPos += transform.right * frontSideSensorPosition;
		if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag("Cube")) {
				Debug.DrawRay(sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier -= 1f;
			}
		}
		
		//front right angle sensor
		else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag("Cube")) {
				Debug.DrawRay(sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier -= 0.5f;
			}
		}
		
		//front left sensor
		sensorStartPos -= transform.right * frontSideSensorPosition * 2;
		if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag("Cube")) {
				Debug.DrawRay(sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier += 1f;
			}
		}
		
		//front left angle sensor
		else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
			if (!hit.collider.CompareTag("Cube")) {
				Debug.DrawRay(sensorStartPos, hit.point);
				avoiding = true;
				avoidMultiplier += 0.5f;
			}
		}
		
		//front center sensor
		if (avoidMultiplier == 0) {
			if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
				if (!hit.collider.CompareTag("Cube")) {
					Debug.DrawRay(sensorStartPos, hit.point);
					avoiding = true;
					if (hit.normal.x < 0) {
						avoidMultiplier = -1;
					} else {
						avoidMultiplier = 1;
					}
				}
			}
		}
		
		if (avoiding) {
			wheelFL.steerAngle = maxSteerAngle * avoidMultiplier;
			wheelFR.steerAngle = maxSteerAngle * avoidMultiplier;
		}
		
	}
	
	private void Drive() {
		currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
		
		if (currentSpeed < maxSpeed && !isBraking) {
			wheelFL.motorTorque = maxMotorTorque;
			wheelFR.motorTorque = maxMotorTorque;
		} else {
			wheelFL.motorTorque = 0;
			wheelFR.motorTorque = 0;
		}
	}

	
	/*private void Braking() {
		if (isBraking) {;
			wheelRL.brakeTorque = maxBrakeTorque;
			wheelRR.brakeTorque = maxBrakeTorque;
		} else {
			wheelRL.brakeTorque = 0;
			wheelRR.brakeTorque = 0;
		}
	}*/
}
