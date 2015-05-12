using UnityEngine;
using System.Collections;

public class FishGrabberBehavior : MonoBehaviour {
	public float dragFactor = 1f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// respond to changes in the GrabberController
		// specifically drag vector
		if (FishController.Instance.dragVector != Vector3.zero)
		{
			GameObject obj = FishController.Instance.cursorObject;
			if (obj != null)
			{
				Rigidbody rb = obj.rigidbody;
				if (rb != null)
				{
					// move the object in the direction of the drag
					// could affect velocity instead
					//rb.velocity = GrabberController.Instance.dragVector * dragFactor;
					rb.velocity = Vector3.zero;
					rb.transform.position = FishController.Instance.cursorPosition;
				}
			}
		}
	}
}