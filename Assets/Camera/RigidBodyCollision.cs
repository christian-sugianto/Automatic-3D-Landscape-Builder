using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCollision : MonoBehaviour
{   
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // to ensure the object doesnt move after collision
    private void OnCollisionExit(Collision collision)
    {
        rb.velocity = new Vector3();
        rb.freezeRotation = true;
    }
}
