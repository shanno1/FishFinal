using UnityEngine;
using System.Collections;

public class MenuCamScript : MonoBehaviour {
	public Texture text;
	// Use this for initialization
	void onGUI(){
		GUI.DrawTexture (new Rect(0, 0, 1380, 720),text);
	}
}
