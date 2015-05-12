using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RalphoffsetPursueAI : MonoBehaviour {
	public Vector3  defaultpos = new Vector3(0,0,0);
	private List<Vector3> paths = new List<Vector3>();
	private List<Vector3> Feelers = new List<Vector3>();

	public Vector3 curTarget;
	public Vector3 velocity;
	public Vector3 acceleration;
	public Vector3 force;
	public float mass;
	public float maxSpeed;
	public int Index=0;
	public float defaultRadius = 5.0f;
	public GameObject leader;
	public bool RandomTarget = true;
	public bool PathFollow = false;


	public bool OffsetPursueEnabled=false;

	
	// Use this for initialization
	void Start () {

		mass = 1;
		velocity = Vector3.one;
		force = Vector3.one;
		acceleration = Vector3.one;
		maxSpeed = 60.0f;
		OffsetPursueEnabled=true;

	}
	
	// Update is called once per frame
	void Update () {

		if(OffsetPursueEnabled){
			force += OffsetPursuit (new Vector3(0.0f,0.0f,-0.0f));
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

	Vector3 generatePos(){
		System.Random xyz = new System.Random();
		
		System.Random p_m = new System.Random();
		int xx = xyz.Next (1000);
		int yy = xyz.Next (520);
		int zz = xyz.Next (1000);
		
		//Random minus number generator
		if (p_m.Next (2) == 0) {
			xx = xx *(-1);
		}
		if (p_m.Next (2) == 0) {
			zz = zz *(-1);
		}
		
		return new Vector3(xx,yy,zz);
	}
	Vector3 getPathTarget(){
		Vector3 temp = paths [Index];
		Index++;
		return temp;
		
	}
	void PopulatePathList(){
		paths.Add(new Vector3(0,200,-300));	//1
		paths.Add(new Vector3(500,250,-500));	//2
		paths.Add(new Vector3(500,250,-100));	//3
		paths.Add(new Vector3(250,250,100));	//4
		paths.Add(new Vector3(600,300,150));	//5
		paths.Add(new Vector3(700,300,400));	//6
		paths.Add(new Vector3(200,350,800));	//7
		paths.Add(new Vector3(0,450,950));	//8
		paths.Add(new Vector3(-300,400,800));	//9
		paths.Add(new Vector3(-200,300,300));	//10
		paths.Add(new Vector3(-150,250,-50));	//11
		paths.Add(new Vector3(-400,100,-400));	//12
		
		
	}

	public Vector3 Arrive(Vector3 target)
	{
		Vector3 toTarget = target - transform.position;
		
		float slowingDistance = 8.0f;
		float distance = toTarget.magnitude;
		if (distance == 0.0f)
		{
			return Vector3.zero;
		}
		const float DecelerationTweaker = 10.3f;
		float ramped = maxSpeed * (distance / (slowingDistance * DecelerationTweaker));
		
		float clamped = Math.Min(ramped, maxSpeed);
		Vector3 desired = clamped * (toTarget / distance);
		
		
		return desired - velocity;
	}
	Vector3 OffsetPursuit(Vector3 offset)
	{
		Vector3 target = leader.transform.position - offset;
		transform.forward = leader.transform.forward;
		float dist = (target - transform.position).magnitude;
		
		float lookAhead = (dist / maxSpeed);
		
		target = target + (lookAhead * velocity);
		

		return Arrive(target);
	}
	private float GetRadius()
	{
		Renderer r = GetComponent<Renderer>();
		if (r == null)
		{
			return defaultRadius;
		}
		else
		{
			return r.bounds.extents.magnitude;
		}
	}
}
