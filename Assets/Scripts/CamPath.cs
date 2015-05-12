using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CamPath : MonoBehaviour {
	public Vector3  defaultpos = new Vector3(0,0,0);
	private List<Vector3> paths = new List<Vector3>();
	public GameObject pathcam;
	public Vector3 curTarget;
	public Vector3 velocity;
	public Vector3 acceleration;
	public Vector3 force;
	public float mass;
	public float maxSpeed;
	public int Index=0;
	// Use this for initialization
	void Start () {
		PopulatePathList();
		curTarget = getPathTarget ();

		mass = 1;
		velocity = Vector3.one;
		force = Vector3.one;
		acceleration = Vector3.one;
		maxSpeed = 40.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Index >= paths.Count)
			Index = 0;

		Vector3 toTarget = curTarget - pathcam.transform.position;
		float dist = toTarget.magnitude;
		
		//Target generation - Path following or Random Generation


		if (dist <10 ) {
			curTarget = getPathTarget();
		}


		force += seek (curTarget);

		acceleration =  force / mass;
		velocity += acceleration * Time.deltaTime;
		Vector3.ClampMagnitude(velocity, maxSpeed);
		pathcam.transform.position +=  Time.deltaTime * velocity;
		
		if (velocity.magnitude > float.Epsilon)
		{
			pathcam.transform.forward = velocity.normalized;
			velocity *= 0.99f;
		}
		if (pathcam.transform.position.y < 0)
			pathcam.transform.position = new Vector3(pathcam.transform.position.x,0,pathcam.transform.position.z);
		if (pathcam.transform.position.y > 531)
			pathcam.transform.position = new Vector3(pathcam.transform.position.x,531,pathcam.transform.position.z);
		force = Vector3.zero;
	}

	Vector3 seek(Vector3 seekTarget){
		Vector3 desired = seekTarget - pathcam.transform.position;
		desired.Normalize();
		desired *= maxSpeed;
		
		return desired - velocity;
	}
	Vector3 getPathTarget(){
		Vector3 temp = paths [Index];
		Index++;
		return temp;
		
	}
	void PopulatePathList(){
		paths.Add(new Vector3(0,300,-300));	//1
		paths.Add(new Vector3(450,250,-450));	//2
		paths.Add(new Vector3(500,250,-100));	//3
		paths.Add(new Vector3(250,250,100));	//4
		paths.Add(new Vector3(600,300,150));	//5
		paths.Add(new Vector3(700,300,400));	//6
		paths.Add(new Vector3(200,350,740));	//7
		paths.Add(new Vector3(0,440,740));	//8
		paths.Add(new Vector3(-300,400,600));	//9
		paths.Add(new Vector3(-200,300,300));	//10
		paths.Add(new Vector3(-150,250,-50));	//11
		paths.Add(new Vector3(-400,150,-400));	//12
		
		
	}
}
