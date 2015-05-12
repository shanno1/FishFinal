using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class JeffAI : MonoBehaviour {
	private Vector3  defaultpos = new Vector3(0,0,0);
	private List<Vector3> paths = new List<Vector3>();
	private List<Vector3> Feelers = new List<Vector3>();
	public GameObject Ralph, sharks; 
	LoadLevel Level;
	public bool isChaseScene;
	public Vector3 curTarget;
	public Vector3 velocity;
	public Vector3 acceleration;
	public Vector3 force;
	public float mass;
	public float maxSpeed;
	public float defaultRadius = 5.0f;
	public bool isSharkAttackScene;
	public bool SeekEnabled=true;
	public bool EvadeEnabled=false;
	public bool FleeEnabled=false;

	
	// Use this for initialization
	void Start () {
		mass = 1;
		sharks = GameObject.FindGameObjectWithTag ("Shark");
		velocity = Vector3.one;
		force = Vector3.one;
		acceleration = Vector3.one;
		maxSpeed = 40.0f;
		EvadeEnabled=false;
		FleeEnabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		curTarget = Ralph.transform.position;

		Vector3 toTarget = curTarget - transform.position;
		float dist = toTarget.magnitude;


		if (isSharkAttackScene && !isChaseScene) {
			force += seek (curTarget);
			if((curTarget - transform.position).magnitude < 15)
				Application.LoadLevel("TheEnd");
		}
		else {
			Debug.Log(dist);
	
			force += seek (curTarget);
			if (EvadeEnabled) {
				force += Evade ();
			} else if (FleeEnabled) {
				force += Flee (curTarget);
			}
		}

		acceleration =  force / mass;
		velocity += acceleration * Time.deltaTime;
		Vector3.ClampMagnitude(velocity, maxSpeed);
		transform.position +=  Time.deltaTime * velocity;
		
		if (velocity.magnitude > float.Epsilon)
		{
			transform.forward = velocity.normalized;
			velocity *= 0.99f;
		}

		if (transform.position.y < 0)
			transform.position = new Vector3(transform.position.x,0,transform.position.z);
		if (transform.position.y > 531)
			transform.position = new Vector3(transform.position.x,531,transform.position.z);
		force = Vector3.zero;
	}

	Vector3 seek(Vector3 seekTarget){
		Vector3 desired = seekTarget - transform.position;
		desired.Normalize();
		desired *= maxSpeed;
		
		return desired - velocity;
	}
	Vector3 Evade()
	{
		float dist = (curTarget - transform.position).magnitude;
		float lookAhead = maxSpeed;
		
		Vector3 targetPos = curTarget+ (lookAhead * velocity);
		return Flee(targetPos);
	}

	Vector3 Flee(Vector3 targetPos)
	{
		float panicDistance = 100.0f;
		Vector3 desiredVelocity;
		desiredVelocity = transform.position - curTarget;
		if (desiredVelocity.magnitude > panicDistance)
		{
			//return Vector3.zero;
		}
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		return (desiredVelocity - velocity);
	}

}
