using UnityEngine;
using System.Collections;

public class AnimatedProjector : MonoBehaviour
{
    public float fps = 30.0f;
    public Texture2D[] frames;

    private int frameIndex;
    private Projector projector;
	public GameObject cam;

    void Start()
    {
		transform.position = new Vector3 (cam.transform.position.x, (cam.transform.position.y+100), cam.transform.position.z);
        projector = GetComponent<Projector>();
        NextFrame();
        InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    void NextFrame()
    {
        projector.material.SetTexture("_ShadowTex", frames[frameIndex]);
        frameIndex = (frameIndex + 1) % frames.Length;
    }
	void Update(){
		transform.position = new Vector3 (cam.transform.position.x, (cam.transform.position.y+100), cam.transform.position.z);
	}
}
