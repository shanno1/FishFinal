using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FishAI : MonoBehaviour {
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

	public bool RandomTarget = true;
	public bool PathFollow = false;
	public bool isChasing;
	public bool SeekEnabled=true;
	public bool EvadeEnabled=false;
	public bool FleeEnabled=false;
	public bool ObstacleAvoidanceEnabled=false;
	
	// Use this for initialization
	void Start () {
		PopulatePathList();
		if(PathFollow)
			curTarget = getPathTarget ();
		if (RandomTarget)
			curTarget = generatePos ();
		mass = 1;
		velocity = Vector3.one;
		force = Vector3.one;
		acceleration = Vector3.one;
	
		maxSpeed = 70.0f;
		EvadeEnabled=false;
		FleeEnabled=false;
		ObstacleAvoidanceEnabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Index > paths.Count)
			Index = 0;
		Vector3 toTarget = curTarget - transform.position;
		float dist = toTarget.magnitude;

		//Target generation - Path following or Random Generation
		if (RandomTarget) {
			if (transform.position == curTarget) {
				curTarget =  defaultpos;
			}
			if (dist<10 || curTarget == defaultpos)
				curTarget = generatePos ();
		}
		if (PathFollow) {
			if (transform.position == curTarget) {
				curTarget =  defaultpos;
			}
			if (dist <10 ) {
				curTarget = getPathTarget();
			}
		}
		if(SeekEnabled){
			force += seek (curTarget);
		}
		if(EvadeEnabled){
			force += Evade ();
		}
		if(FleeEnabled){
			force += Flee (curTarget);
		}
		if(ObstacleAvoidanceEnabled){
			force += ObstacleAvoidance ();
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
	private void makeFeelers()
	{
		Feelers.Clear();
		float feelerDistance = 20.0f;
		// Make the forward feeler
		Vector3 newFeeler = Vector3.forward * feelerDistance;
		newFeeler = transform.TransformPoint(newFeeler);
		Feelers.Add(newFeeler);
		
		newFeeler = Vector3.forward * feelerDistance;
		newFeeler = Quaternion.AngleAxis(45, Vector3.up) * newFeeler;
		newFeeler = transform.TransformPoint(newFeeler);
		Feelers.Add(newFeeler);
		
		newFeeler = Vector3.forward * feelerDistance;
		newFeeler = Quaternion.AngleAxis(-45, Vector3.up) * newFeeler;
		newFeeler = transform.TransformPoint(newFeeler);
		Feelers.Add(newFeeler);
		
		newFeeler = Vector3.forward * feelerDistance;
		newFeeler = Quaternion.AngleAxis(45, Vector3.right) * newFeeler;
		newFeeler = transform.TransformPoint(newFeeler);
		Feelers.Add(newFeeler);
		
		newFeeler = Vector3.forward * feelerDistance;
		newFeeler = Quaternion.AngleAxis(-45, Vector3.right) * newFeeler;
		newFeeler = transform.TransformPoint(newFeeler);
		Feelers.Add(newFeeler);
	}
	
	Vector3 generatePos(){
		System.Random xyz = new System.Random();
		
		System.Random p_m = new System.Random();
		int xx = xyz.Next (-700,700);
		int yy = xyz.Next (120,500);
		int zz = xyz.Next (-700,700);
		
//		//Random minus number generator
//		if (p_m.Next (2) == 0) {
//			xx = xx *(-1);
//		}
//		if (p_m.Next (2) == 0) {
//			zz = zz *(-1);
//		}
		
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
	Vector3 ObstacleAvoidance()
	{
		Vector3 force = Vector3.zero;
		makeFeelers();
		List<GameObject> tagged = new List<GameObject>();
		float minBoxLength = 50.0f;
		float boxLength = minBoxLength + ((velocity.magnitude / maxSpeed) * minBoxLength * 2.0f);
		
		if (float.IsNaN(boxLength))
		{
			System.Console.WriteLine("NAN");
		}
		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("FishIndividual");
		GameObject m = GameObject.FindGameObjectWithTag ("MainCharacter");
		// Matt Bucklands Obstacle avoidance
		// First tag obstacles in range
		if (obstacles.Length == 0)
		{
			return Vector3.zero;
		}
		foreach (GameObject obstacle in obstacles)
		{
			if (obstacle == null)
			{
				Debug.Log("Null object");
				continue;
			}
			
			Vector3 toCentre = transform.position - obstacle.transform.position;
			float dist = toCentre.magnitude;
			if (dist < boxLength)
			{
				tagged.Add(obstacle);
			}
		}
		tagged.Add (m);
		float distToClosestIP = float.MaxValue;
		GameObject closestIntersectingObstacle = null;
		Vector3 localPosOfClosestObstacle = Vector3.zero;
		Vector3 intersection = Vector3.zero;
		
		foreach (GameObject o in tagged)
		{
			Vector3 localPos = transform.InverseTransformPoint(o.transform.position);
			
			// If the local position has a positive Z value then it must lay
			// behind the agent. (in which case it can be ignored)
			if (localPos.z >= 0)
			{
				// If the distance from the x axis to the object's position is less
				// than its radius + half the width of the detection box then there
				// is a potential intersection.
				
				float obstacleRadius = o.transform.localScale.x / 2;
				float expandedRadius = GetRadius() + obstacleRadius;
				if ((Math.Abs(localPos.y) < expandedRadius) && (Math.Abs(localPos.x) < expandedRadius))
				{
					// Now to do a ray/sphere intersection test. The center of the				
					// Create a temp Entity to hold the sphere in local space
					Sphere tempSphere = new Sphere(expandedRadius, localPos);
					
					// Create a ray
					Ray ray = new Ray();
					ray.pos = new Vector3(0, 0, 0);
					ray.look = Vector3.forward;
					
					// Find the point of intersection
					if (tempSphere.closestRayIntersects(ray, Vector3.zero, ref intersection) == false)
					{
						continue;
					}
					
					// Now see if its the closest, there may be other intersecting spheres
					float dist = intersection.magnitude;
					if (dist < distToClosestIP)
					{
						dist = distToClosestIP;
						closestIntersectingObstacle = o;
						localPosOfClosestObstacle = localPos;
					}
				}
			}                
		}
		
		if (closestIntersectingObstacle != null)
		{
			// Now calculate the force
			float multiplier = 1.0f + (boxLength - localPosOfClosestObstacle.z) / boxLength;
			
			//calculate the lateral force
			float obstacleRadius = closestIntersectingObstacle.transform.localScale.x / 2; // closestIntersectingObstacle.GetComponent<Renderer>().bounds.extents.magnitude;
			float expandedRadius = GetRadius() + obstacleRadius;
			force.x = (expandedRadius - Math.Abs(localPosOfClosestObstacle.x)) * multiplier;
			force.y = (expandedRadius - Math.Abs(localPosOfClosestObstacle.y)) * multiplier;
			
			// Generate positive or negative direction so we steer around!
			// Not always in the same direction as in Matt Bucklands book
			if (localPosOfClosestObstacle.x > 0)
			{
				force.x = -force.x;
			}
			
			// If the obstacle is above, steer down
			if (localPosOfClosestObstacle.y > 0)
			{
				force.y = -force.y;
			}
			
			//apply a braking force proportional to the obstacle's distance from
			//the vehicle.
			const float brakingWeight = 0.01f;
			force.z = (expandedRadius -
			           localPosOfClosestObstacle.z) *
				brakingWeight;
			
			//finally, convert the steering vector from local to world space
			// Dont include position!                    
			force = transform.TransformDirection(force);
		}
		
		
		return force;
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
