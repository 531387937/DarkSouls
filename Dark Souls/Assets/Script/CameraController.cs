using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public PlayerInput pi;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject cam;

    private Vector3 cameraDampVelocity;
	// Use this for initialization
	void Awake () {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        model = playerHandle.GetComponent<ActorCotronller>().model;
        cam = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 tempModelEuler = model.transform.eulerAngles;
        playerHandle.transform.Rotate(Vector3.up,pi.Jright*100.0f*Time.fixedDeltaTime);
        
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, transform.position, ref cameraDampVelocity, 0.2f);
        //cam.transform.eulerAngles = transform.eulerAngles;
        cam.transform.LookAt(cameraHandle.transform);

        tempEulerX -= pi.Jup * 80.0f * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
        model.transform.eulerAngles = tempModelEuler;
        //cam.transform.position = Vector3.Lerp(cam.transform.position,transform.position,0.2f);
        
    }
}
