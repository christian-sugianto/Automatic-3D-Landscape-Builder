using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraObjectController : MonoBehaviour
{
    /**************** attributes ****************/

    public float mouseSensitivity = 0.5f;   // Mouse rotation sensitivity.
    [Range(0.0f, 35.0f)]
    public float speed;                     // Speed of the camera object.
    
    private bool isRotating;                // Determines if the camera is rotating
    private float rotationY;
    private float rotationX;
    /********************************************/

    /**
     * Move using WASD
     * Hold left click to rotate camera
     * Q/E to raise/lower Y plane
     */


    void Start()
    {
        // To prevent rotation to be set to 0 at first click of mouse
        this.rotationY = -transform.localEulerAngles.x;
        this.rotationX = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    { 
		// Handles Rotation of camera
		// Assing isRotating when left click is being pressed
		if (Input.GetMouseButtonDown(0))
			isRotating = true;
       
		// Assing isRotating to false when left click isnt being pressed
		if (Input.GetMouseButtonUp(0))
			isRotating = false;

		if (isRotating)
        { 
		    rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
		    rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
		    rotationY = Mathf.Clamp(rotationY, -90, 90);
		    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
        }

        // Handles Keyboard input
        Vector3 position = new Vector3();
        if (Input.GetKey(KeyCode.D))
            position.x += 1.0f;
        if (Input.GetKey(KeyCode.A))
            position.x -= 1.0f;
        if (Input.GetKey(KeyCode.W))
            position.z += 1.0f;
        if (Input.GetKey(KeyCode.S))
            position.z -= 1.0f;
        if (Input.GetKey(KeyCode.E))
            position.y -= 1.0f;
        if (Input.GetKey(KeyCode.Q))
            position.y += 1.0f;
        
		position *= speed * Time.deltaTime;
        transform.Translate(position);
    }

}
