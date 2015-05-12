using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			Application.LoadLevel("Beginning");
		}
	}
	public void NxtLevel(string scene){
		Application.LoadLevel(scene);
	}
}
