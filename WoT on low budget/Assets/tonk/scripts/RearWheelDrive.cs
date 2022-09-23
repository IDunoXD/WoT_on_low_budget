using UnityEngine;
using System.Collections;

public class RearWheelDrive : MonoBehaviour {

	private WheelCollider[] wheels;
	public float torgue;
	public float maxAngle = 30;
	public float maxTorque = 300;
	[Tooltip("meters per minute")]
	public float maxSpeed = 2000; //meters per minute
	private float maxRpm;
	private float TurnRadius, whFB, whRL, whFar, whClose;
	public float FastWheelStiffness=0.3f , SlowWheelStiffness=0.9f;
	private float temp;
	public GameObject wheelShape;
	private float BrakeRecoverTime = 0;
	// here we find all the WheelColliders down in the hierarchy
	public void Start()
	{
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];
			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;

				if (wheel.transform.localPosition.x < 0f)
				{
					ws.transform.localScale = new Vector3(ws.transform.localScale.x*-1f,ws.transform.localScale.y,ws.transform.localScale.z);
				}
			}
		}	
		maxRpm = maxSpeed/(2*Mathf.PI*wheels[0].radius);
		whFB=(wheels[0].transform.localPosition.z-wheels[wheels.Length-2].transform.localPosition.z)/2f; //x
		whRL=(wheels[1].transform.localPosition.x-wheels[0].transform.localPosition.x); //y
		TurnRadius = whFB/Mathf.Tan(Mathf.Deg2Rad * maxAngle); //z
		whFar = Mathf.Rad2Deg * Mathf.Atan2(whFB,TurnRadius+whRL);
		whClose = Mathf.Rad2Deg * Mathf.Atan2(whFB,TurnRadius);
	}

	// this is a really simple approach to updating wheels
	// here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
	// this helps us to figure our which wheels are front ones and which are rear
	public void Update()
	{
		float SteerInput = Input.GetAxis("Horizontal");
		float torque = maxTorque * Input.GetAxis("Vertical");
		bool SpacebarHold = Input.GetKey(KeyCode.Space);
		bool SpacebarUp = Input.GetKeyUp(KeyCode.Space);

		foreach (WheelCollider wheel in wheels)
		{
			//passive brakes
			if (torque==0 && wheel.rpm!=0){
				wheel.brakeTorque = Mathf.Abs(wheel.rpm)*3;
			}

			//Ackermann Steering
			if (SteerInput > 0){
				if (wheel.transform.localPosition.z > 2){	//Front wheels
					if (wheel.transform.localPosition.x < 0)
						wheel.steerAngle = whFar * SteerInput; //LF wheel turn left
					if (wheel.transform.localPosition.x > 0)
						wheel.steerAngle = whClose * SteerInput; //RF wheel turn left
				}
				if (wheel.transform.localPosition.z < -2){	//Back wheels
					if (wheel.transform.localPosition.x < 0)
						wheel.steerAngle = whFar * -SteerInput; //LB wheel turn left
					if (wheel.transform.localPosition.x > 0)
						wheel.steerAngle = whClose * -SteerInput; //RB wheel turn left
				}
			} else if (SteerInput < 0){
				if (wheel.transform.localPosition.z > 2){	//Front wheels
					if (wheel.transform.localPosition.x < 0)
						wheel.steerAngle = whClose * SteerInput; //LF wheel turn right
					if (wheel.transform.localPosition.x > 0)
						wheel.steerAngle = whFar * SteerInput; //RF wheel turn right
				}
				if (wheel.transform.localPosition.z < -2){	//Back wheels
					if (wheel.transform.localPosition.x < 0)
						wheel.steerAngle = whClose * -SteerInput; //LB wheel turn right
					if (wheel.transform.localPosition.x > 0)
						wheel.steerAngle = whFar * -SteerInput; //RB wheel turn right
				}
			} else {	
				if (wheel.transform.localPosition.z > 2){	//Front wheels
					if (wheel.transform.localPosition.x < 0)
						wheel.steerAngle = 0; //LF
					if (wheel.transform.localPosition.x > 0)
						wheel.steerAngle = 0; //RF
				}
				if (wheel.transform.localPosition.z < -2){	//Back wheels
					if (wheel.transform.localPosition.x < 0)
						wheel.steerAngle = 0; //LB
					if (wheel.transform.localPosition.x > 0)
						wheel.steerAngle = 0; //RB
				}
			}

			//controllable torgue dependant from wheel rpm
			if (torque>0){
				wheel.brakeTorque = 0;
				wheel.motorTorque = torque * (1-Mathf.Pow(Mathf.InverseLerp(0,maxRpm,wheel.rpm),2));
			}else if(torque<0){
				wheel.brakeTorque = 0;
				wheel.motorTorque = torque * (1-Mathf.Pow(Mathf.InverseLerp(0,-maxRpm,wheel.rpm),2));
			}else
				wheel.motorTorque = 0;

			//wheel stiffness dependant from wheel rpm
			temp=Mathf.Lerp(FastWheelStiffness,SlowWheelStiffness,Mathf.InverseLerp(maxRpm,0,Mathf.Abs(wheel.rpm)));
			SetWheelStiffness(wheel,temp,temp);

			//rpm limit
			if (Mathf.Abs(wheel.rpm)>maxRpm)
				wheel.brakeTorque = wheel.rpm;

			//brakes with drift
			if (SpacebarHold){
				SetWheelStiffness(wheel,temp,0.2f);
				BrakeRecoverTime=1;
				if (wheel.transform.localPosition.z > 2){
					wheel.brakeTorque = 1000;
					wheel.motorTorque = 0;
				}
			}
			if(SpacebarUp){
				wheel.brakeTorque = 0;
				wheel.motorTorque = 1;
				BrakeRecoverTime = 1;
			}
			
			if(BrakeRecoverTime>0){
				SetWheelStiffness(wheel,temp,Mathf.Lerp(temp,0.2f,BrakeRecoverTime));
			}
			if(BrakeRecoverTime<0){
				SetWheelStiffness(wheel,temp,temp);
			}
			
			//Debug.Log("BrakeRecoverTime " + BrakeRecoverTime);
			//Debug.Log("delta " + Time.deltaTime);
			//Debug.Log("torgue " + torque * (1 - Mathf.Pow(Mathf.InverseLerp(0, maxRpm, Mathf.Abs(wheel.rpm)), 2)));
			//Debug.Log("inv lerp F " + (1 - Mathf.Pow(Mathf.InverseLerp(0, maxRpm, wheel.rpm), 2)));
			//Debug.Log("inv lerp S " + (1 - Mathf.Pow(Mathf.InverseLerp(0, -maxRpm, wheel.rpm), 2)));

			// update visual wheels if any
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);
				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}
		if(BrakeRecoverTime>0)
			BrakeRecoverTime-=Time.deltaTime;
		else
			BrakeRecoverTime = 0;
		
		//Debug.Log("stiffness " + wheels[0].forwardFriction.stiffness);
		//Debug.Log("maxrpm " + maxRpm);
		//Debug.Log("rpm " + wheels[0].rpm);
		//Debug.Log("mt " + wheels[0].motorTorque);
		//Debug.Log("bt " + wheels[0].brakeTorque);
		//Debug.Log("FL " + wheels[0].steerAngle);
		//Debug.Log("FR " + wheels[1].steerAngle);
		SetTorgue(torque);
	}
	public void SetWheelStiffness(WheelCollider wheel, float ForwardStiffness, float SidewaysStiffness)
	{
		WheelFrictionCurve _forawFoFardFriction,_sidewaysFriction;
		_forawFoFardFriction = wheel.forwardFriction;
		_sidewaysFriction = wheel.sidewaysFriction;
		_forawFoFardFriction.stiffness = ForwardStiffness;
		_sidewaysFriction.stiffness = SidewaysStiffness;
		wheel.forwardFriction = _forawFoFardFriction;
		wheel.sidewaysFriction = _sidewaysFriction;
	}
	public void SetTorgue(float t){
		torgue = t;
	}
}
