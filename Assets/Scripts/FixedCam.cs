using UnityEngine;
using System.Collections;

public class FixedCam : MonoBehaviour {
	public Camera cam;
	public Vector3 offset = new Vector3(4.3f,11.1f,-21.8f);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		cam.transform.position = transform.position + offset;
		//cam.transform.LookAt(transform.position);
	}
}
