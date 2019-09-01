using UnityEngine;
using System.Collections;


// reference: http://forum.unity3d.com/threads/fly-cam-simple-cam-script.67042/


public class FlyingCamera : MonoBehaviour
{

    /**************** public attributes ****************/
    public float speed = 100.0f;                //speed
    public float camSensitivity = 0.25f;        //How sensitive it with mouse
    private float totalRun = 1.0f;
    private bool isRotating;                    // Determines if the camera is rotating
    private float speedMultiplier;              // Angryboy: Used by Y axis to match the velocity on X/Z axis

    [Range(0.0f, 1.0f)]
    public float mouseSensitivity = 0.5f;       // Mouse rotation sensitivity.
    /***************************************************/

    /**
     * Move using WASD
     * Hold left click to rotate camera
     * Q/E to raise/lower Y plane
     */

    private float rotationY;


    void Update()
    {
        // Handles Rotation of camera
        // Assing isRotating when left click is being pressed
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
        }

        // Assing isRotating to false when left click isnt being pressed
        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        if (isRotating)
        { 
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
        }

        //Keyboard commands
        Vector3 position = GetNewPosition();

        // Move using WASD
        totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        position *= speed;
        speedMultiplier = speed * Time.deltaTime;
        position *= Time.deltaTime;
        transform.Translate(position);


        // To raise or lower the Y Plane
        Vector3 newPosition = transform.position; 
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;
        if (Input.GetKey(KeyCode.Q))
        {
            newPosition.y -= speedMultiplier;
        }

        if (Input.GetKey(KeyCode.E))
        {
            newPosition.y += speedMultiplier;
        }

        transform.position = newPosition;
    }


    private Vector3 GetNewPosition()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return(new Vector3(0, 0, 1));
        }

        if (Input.GetKey(KeyCode.S))
        {
            return (new Vector3(0, 0, -1));
        }

        if (Input.GetKey(KeyCode.A))
        {
            return (new Vector3(-1, 0, 0));
        }

        if (Input.GetKey(KeyCode.D))
        {
            return (new Vector3(1, 0, 0));
        }

        // If no input
        return new Vector3();
    }
}
