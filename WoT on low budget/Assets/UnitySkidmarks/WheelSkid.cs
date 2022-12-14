using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example skid script. Put this on a WheelCollider.
// Copyright 2017 Nition, BSD licence (see LICENCE file). http://nition.co
[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour {

	// INSPECTOR SETTINGS

	[SerializeField]
	Rigidbody rb;
	[SerializeField]
	Skidmarks skidmarksController;

	// END INSPECTOR SETTINGS

	WheelCollider wheelCollider;
	WheelHit wheelHitInfo;
	AudioSource source;
	public GameObject smoke;
	const float SKID_FX_SPEED = 3f; // Min side slip speed in m/s to start showing a skid
	const float MAX_SKID_INTENSITY = 10.0f; // m/s where skid opacity is at full intensity
	const float WHEEL_SLIP_MULTIPLIER = 0.1f; // For wheelspin. Adjust how much skids show
	int lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
	float lastFixedUpdateTime;
	float BrakeSkidSize = 0.2f;
	// #### UNITY INTERNAL METHODS ####

	protected void Awake() {
		wheelCollider = GetComponent<WheelCollider>();
		source = GetComponent<AudioSource>();
	}
	protected void FixedUpdate() {
		lastFixedUpdateTime = Time.time;
	}

	protected void Update() {
		//Debug.Log(wheelHitInfo.forwardSlip);
		if (wheelCollider.GetGroundHit(out wheelHitInfo))
		{
			// Check sideways speed

			// Gives velocity with +z being the car's forward axis
			Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
			float skidTotal = Mathf.Abs(localVelocity.x);

			// Check wheel spin as well

			float wheelAngularVelocity = wheelCollider.rpm * ((2 * Mathf.PI * wheelCollider.radius) / 60);
			float carForwardVel = Vector3.Dot(rb.velocity, transform.forward);
			float SpeedDifferencePercentage = Mathf.Abs((wheelAngularVelocity) - (carForwardVel));
			float wheelSpin = SpeedDifferencePercentage * Mathf.Abs(wheelHitInfo.forwardSlip) * 1f;
			//Debug.Log(wheelHitInfo.forwardSlip + " " + wheelSpin);

			// NOTE: This extra line should not be needed and you can take it out if you have decent wheel physics
			// The built-in Unity demo car is actually skidding its wheels the ENTIRE time you're accelerating,
			// so this fades out the wheelspin-based skid as speed increases to make it look almost OK
			//if(wheelAngularVelocity!=0)
			wheelSpin = Mathf.Max(0, wheelSpin * (18f - Mathf.Abs(wheelAngularVelocity)));

			//skidTotal += wheelSpin;
			skidTotal+=wheelSpin;

			// Skid if we should
			if (skidTotal >= SKID_FX_SPEED) {
				float intensity = Mathf.Clamp01(skidTotal / MAX_SKID_INTENSITY);
				source.volume=Mathf.Lerp(0.1f,0.2f,Mathf.InverseLerp(3,30,skidTotal));
				if(!source.isPlaying)
					source.Play();
				// Account for further movement since the last FixedUpdate
				Vector3 skidPoint = wheelHitInfo.point + (rb.velocity * (Time.time - lastFixedUpdateTime)); // + rb.velocity.normalized * 0.15f
				//Debug.Log(carForwardVel);
				var flash = Instantiate(smoke, transform);
				Destroy(flash,1);
				if (Vector3.Magnitude(rb.velocity)<0.01f){
					skidPoint += (transform.forward * BrakeSkidSize);
					//Debug.Log(transform.forward);
					BrakeSkidSize = -BrakeSkidSize;
				}
				lastSkid = skidmarksController.AddSkidMark(skidPoint, wheelHitInfo.normal, intensity, lastSkid, Mathf.Abs(carForwardVel));
			}
			else {
				lastSkid = -1;
				source.volume=0;
			}
		}
		else {
			lastSkid = -1;
		}
	}

	// #### PUBLIC METHODS ####

	// #### PROTECTED/PRIVATE METHODS ####


}
