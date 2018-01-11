using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBillboard : MonoBehaviour
{
    //public GameObject headJoint;
    //public Vector3 offset;
    public Transform targetCamera;

    // Use this for initialization
    void Start ()
    {
        //this.transform.position = headJoint.transform.position;
        transform.LookAt(targetCamera);

    }
	
	// Update is called once per frame
	void Update ()
    {
        //this.transform.position = headJoint.transform.position;
        transform.LookAt(targetCamera);
        //this.transform.position = headJoint.transform.position 
        //                            + (headJoint.transform.forward * offset.z)
        //                            + (headJoint.transform.up * offset.y)
        //                            + (headJoint.transform.right * offset.x);
        //transform.LookAt(targetCamera);
    }
}
