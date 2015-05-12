using UnityEngine;
using System.Collections;

public class CamSwitcher : MonoBehaviour {
	public bool FPScam,RALPHcam,PATHcam;
	public Camera fpsCam, ralphCam, pathCam;
	// Use this for initialization
	void Start () {		
		if (FPScam && !RALPHcam && !PATHcam) {
			fpsCam.camera.enabled = true;
			ralphCam.camera.enabled = false;
			pathCam.camera.enabled = false;
		} else if (!FPScam && RALPHcam && !PATHcam) {
			fpsCam.camera.enabled = false;
			ralphCam.camera.enabled = true;
			pathCam.camera.enabled = false;
		} else if (!FPScam && !RALPHcam && PATHcam) {
			fpsCam.camera.enabled = false;
			ralphCam.camera.enabled = false;
			pathCam.camera.enabled = true;
		} else {
			fpsCam.camera.enabled = true;
			ralphCam.camera.enabled = false;
			pathCam.camera.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			fpsCam.camera.enabled = true;
			ralphCam.camera.enabled = false;
			pathCam.camera.enabled =false;
		} else if (Input.GetKeyDown ("2")) {
			fpsCam.camera.enabled = false;
			ralphCam.camera.enabled = true;
			pathCam.camera.enabled =false;
		} else if (Input.GetKeyDown ("3")) {
			fpsCam.camera.enabled = false;
			ralphCam.camera.enabled = false;
			pathCam.camera.enabled =true;
		}


	}
}
