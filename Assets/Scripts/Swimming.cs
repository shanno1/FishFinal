using UnityEngine;
using System.Collections;

public class Swimming : MonoBehaviour {
	CharacterController cc;
	CharacterMotor cm;
	public Camera cam;
	public float Speed = 50.0f;
	public float clockwise = 1000.0f;
	public float counterClockwise = -5.0f;
	// Use this for initialization
	void Start () {
		cc = gameObject.GetComponent<CharacterController> ();
		cm = gameObject.GetComponent<CharacterMotor> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isUnderwater ()) {

			cm.movement.maxForwardSpeed =60;
			cm.movement.maxSidewaysSpeed = 60;
			cm.movement.maxBackwardsSpeed = 60;
			cm.movement.maxFallSpeed = 60;
			if (transform.position.y < 0)
				transform.position = new Vector3(transform.position.x,0,transform.position.z);
			if (transform.position.y > 531)
				transform.position = new Vector3(transform.position.x,531,transform.position.z);

			if(Input.GetKey (KeyCode.O)){
				cm.SetVelocity(new Vector3(cc.velocity.x,20,cc.velocity.z));
			}
			else if(Input.GetKey (KeyCode.L)){
				cm.SetVelocity(new Vector3(cc.velocity.x,-20,cc.velocity.z));
			}
			else{
				cm.SetVelocity(new Vector3(cc.velocity.x,0,cc.velocity.z));
			}


			/*
			if(Input.GetKey (KeyCode.UpArrow)){
				cm.SetVelocity(new Vector3(cc.velocity.x,cc.velocity.y,40));
			}
			else if(Input.GetKey (KeyCode.DownArrow)){
				cm.SetVelocity(new Vector3(cc.velocity.x,cc.velocity.y,-40));
			}
			else{
				cm.SetVelocity(new Vector3(cc.velocity.x,cc.velocity.y,0));
			}

			if(Input.GetKey (KeyCode.LeftArrow)){
				cm.SetVelocity(new Vector3(-40,cc.velocity.y,cc.velocity.z));
			}
			else if(Input.GetKey (KeyCode.RightArrow)){
				cm.SetVelocity(new Vector3(40,cc.velocity.y,cc.velocity.z));
			}
			else{
				cm.SetVelocity(new Vector3(0,cc.velocity.y,cc.velocity.z));
			} */
		}

	}
	bool isUnderwater(){
		return transform.position.y < 530.0f;
	}
}
