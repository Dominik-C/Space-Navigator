using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour {

    public float timeVal = .1f;
    public float timeValUP = .1f;
    public float timeValLeft = .1f;

    void Update()
    {
        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(Vector3.right * timeVal * Time.deltaTime/2);

        // ...also rotate around the World's Y axis
        transform.Rotate(Vector3.up * timeValUP * Time.deltaTime / 2);

        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(Vector3.down * timeValLeft * Time.deltaTime / 2);
    }
}
