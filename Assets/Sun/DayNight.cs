using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public float speed;
    private float rotationX;

    void Start()
    {
        this.rotationX = transform.localEulerAngles.x;
    }
    // Update is called once per frame
    void Update()
    {
        this.rotationX += speed * Time.deltaTime;
        this.rotationX %= 360;
        transform.localEulerAngles = new Vector3(rotationX, 0.0f, 0.0f);
    }
}
