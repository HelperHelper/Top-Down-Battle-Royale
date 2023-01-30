using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownScript : MonoBehaviour
{

    public Transform target;
    public float distance = 5;
    public float angle = 60;
    public float height = 9.5f;
    public float smoothness = 0.6f;
    private Vector3 referenceVelocity;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void TopDCam()
    {
        Vector3 worldPos = (Vector3.forward * -distance) + (Vector3.up * height); // alejar la camara del objetivo 
        Vector3 angleVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPos; // el angulo desde donde se vera 
        Vector3 flatPos = target.position;
        flatPos.y = 0;
        Vector3 finalPos = flatPos + angleVector; // posición final

        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref referenceVelocity, smoothness); // ahora movemos la camara a la posición establecida anteriomente
        transform.LookAt(flatPos); // hacer que la cámara enfoque al objetivo
    }

    private void Update()
    {
        TopDCam();
    }

}
