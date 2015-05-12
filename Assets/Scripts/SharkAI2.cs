using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SharkAI2 : MonoBehaviour {
	public Vector3 Target,TarFish, defaultpos = new Vector3(0,0,0);
	public GameObject TargetFish;
	public bool RandomTarget = false, isAttackScene , PathFollow = false;
	public bool IdleState=true, AttackingState=false;
	private List<Vector3> paths = new List<Vector3>();
	LoadLevel level;
	public Vector3 velocity;
	public Vector3 acceleration;
	public Vector3 force;
	public float mass;
	public float maxSpeed;
	public int Index=0;
	// Use this for initialization

	void Start () {


		PopulatePathList();
		if(PathFollow)
			Target = getPathTarget ();
		if (RandomTarget)
			Target = generatePos ();
		mass = 1;
		velocity = Vector3.one;
		force = Vector3.one;
		acceleration = Vector3.one;
		maxSpeed = 30.0f;
		if(isAttackScene)
			TargetFish = GameObject.FindGameObjectWithTag ("Jeff");
		else 
			TargetFish = GameObject.FindGameObjectWithTag ("FishIndividual");
	}
	
	// Update is called once per frame
	void Update () {
		if (Index > paths.Count)
			Index = 0;
	 	//if the attacking state activates automatically, idle state is off.
		if (AttackingState) {
			IdleState = false;

		} else {
			IdleState = true;
		}
		Vector3 toTarget3 = Target - transform.position;
		float distance2 = toTarget3.magnitude;
		if (IdleState) {
			//Random Path Generation
			if (RandomTarget) {
				if (transform.position == Target) {
					Target =  defaultpos;
				}
				if (distance2 <10 || Target == defaultpos)
					Target = generatePos ();
			}

			if (PathFollow) {
				if (transform.position == Target) {
					Target =  defaultpos;
				}
				if (distance2 <10) {
					Target = getPathTarget();
				}
			}

		}

		Vector3 toTarget = TargetFish.transform.position - transform.position;
		float distance = toTarget.magnitude;
		//if fish in range it attacks
		if (distance <=130) {
			force += seek (TargetFish.transform.position);
			TarFish = TargetFish.transform.position;
			audio.Play();
			maxSpeed = 80.0f;
			AttackingState = true;

			if(distance <= 10 &&isAttackScene )
				Application.LoadLevel("GameOver");
		} else {
			force += seek (Target);
			TarFish = new Vector3(0,0,0);
			maxSpeed = 30.0f;
			AttackingState = false;
			//dundun.Play = false;
		}


		if (transform.position.y < 0)
			transform.position = new Vector3(transform.position.x,0,transform.position.z);
		if (transform.position.y > 531)
			transform.position = new Vector3(transform.position.x,531,transform.position.z);

		acceleration =  force / mass;
		velocity += acceleration * Time.deltaTime ;
		Vector3.ClampMagnitude(velocity, maxSpeed);
		transform.position +=  Time.deltaTime * velocity;
		
		if (velocity.magnitude > float.Epsilon)
		{
			transform.forward = velocity.normalized;
			velocity *= 0.99f;
		}
		
		force = Vector3.zero;
	}

	Vector3 generatePos(){
		System.Random xyz = new System.Random();
		
		System.Random p_m = new System.Random();
		int xx = xyz.Next (-700,700);
		int yy = xyz.Next (120,500);
		int zz = xyz.Next (-700,700);

		return new Vector3(xx,yy,zz);
	}
	Vector3 getPathTarget(){
		Vector3 temp = paths [Index];
		Index++;
		return temp;
	
	}
	void PopulatePathList(){
		paths.Add(new Vector3(0,200,-300));		//1
		paths.Add(new Vector3(500,250,-500));	//2
		paths.Add(new Vector3(500,250,-100));	//3
		paths.Add(new Vector3(250,250,100));		//4
		paths.Add(new Vector3(600,300,150));		//5
		paths.Add(new Vector3(700,300,400));		//6
		paths.Add(new Vector3(200,350,800));		//7
		paths.Add(new Vector3(0,450,950));			//8
		paths.Add(new Vector3(-300,400,800));	//9
		paths.Add(new Vector3(-200,300,300));	//10
		paths.Add(new Vector3(-150,250,-50));	//11
		paths.Add(new Vector3(-400,100,-400));	//12


	}
	Vector3 seek(Vector3 seekTarget){
		Vector3 desired = seekTarget - transform.position;
		desired.Normalize();
		desired *= maxSpeed;

		return desired - velocity;
	}


}
