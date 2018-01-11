using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public float startAngle = 45;
    public float multiplayer = 12;
    public GameObject head;
    public GameObject target;
    // Use this for initialization
    void Start ()
    {	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = head.transform.position;
        transform.LookAt(target.transform);
        Camera.main.fieldOfView = startAngle - (Camera.main.transform.position.z - 2) * multiplayer;

    }
}
