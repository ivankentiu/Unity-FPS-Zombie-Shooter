using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraMount : MonoBehaviour
{
    public Transform Mount = null;
    public float Speed = 5.0f;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Mount.position, Speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Mount.rotation, Speed * Time.deltaTime);
    }
}
