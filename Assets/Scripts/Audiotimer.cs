using UnityEngine;
using System.Collections;

public class Audiotimer : MonoBehaviour {
	int index =0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying && index <=20)
			Application.LoadLevel("ChaseScene");


		if(index <20)
			index++;
	}
}
