using UnityEngine;
using System.Collections;

public class FishController : MonoBehaviour {
	public Vector3 dragVector;
	public GameObject cursorObject;
	public Vector3 cursorPosition;
	public Vector3 prevCursorPosition;
	
	private static FishController instance;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// track cursor position
		prevCursorPosition = cursorPosition;
		cursorPosition = GetMousePositionYZero();
		
		// reset drag vector
		dragVector = Vector3.zero;
		

		// listen for mouse down events
		 if (Input.GetMouseButton(0))
		{
			// holding
			dragVector = cursorPosition - prevCursorPosition;
		}
	}
	
	/// <summary>
	/// Singleton instance
	/// </summary>
	public static FishController Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject container = new GameObject();
				container.name = "FishController";
				instance = container.AddComponent<FishController>();			
			}
			
			return instance;
		}
	}
	

	
	public static Vector3 GetMousePositionYZero()
	{
		if (Camera.mainCamera != null)
		{
			Vector3 pos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(
				Input.mousePosition.x,
				Input.mousePosition.y, 
				Camera.mainCamera.transform.position.y
				));
			return new Vector3(pos.x, 0f, pos.z);
		}
		else {
			return Vector3.zero;
		}
	}
}