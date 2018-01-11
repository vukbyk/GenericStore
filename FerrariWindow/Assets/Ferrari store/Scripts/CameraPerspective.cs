using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPerspective : MonoBehaviour
{

    public GameObject referenceCam;

    public float rotSpeed = 0.01f;
    public float posSpeed = 0.01f;

    KinectManager kinectManager;

    Camera cam;
    Vector3 playingPos;
    Quaternion playingRot;
    Vector3 moviePos;
    Quaternion movieRot;


    public bool isPlaying=false;
    public float timer = 0.0f;
    
    //bool lastPlaying = false;
    // Use this for initialization
    void Start ()
    {
        
        cam = Camera.main;
        playingPos = cam.transform.position;
        playingRot = cam.transform.rotation;

        moviePos = referenceCam.transform.position;
        movieRot = referenceCam.transform.rotation;

        cam.orthographic = true;
        cam.transform.position = moviePos;
        cam.transform.rotation = movieRot;

        kinectManager = KinectManager.Instance;
        
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if(timer<=60)
            timer += Time.deltaTime;
        if (kinectManager.GetUsersCount() >= 1)
        {
            if (!isPlaying)
                timer = 0;
            isPlaying = true;
        }
        else
        {
            if (isPlaying)
                timer = 0;
            isPlaying = false;
        }

        if (isPlaying)
        {
            //Debug.Log("Playing " + playingPos.position);
            cam.orthographic = false;
            cam.transform.position = Vector3.Lerp(cam.transform.position, playingPos, timer * posSpeed);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, playingRot, timer * rotSpeed);
        }
        else
        {
            //Debug.Log("NOT playing");
            cam.transform.position = Vector3.Lerp(cam.transform.position, moviePos, Time.time * posSpeed);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, movieRot, Time.time * rotSpeed);
            if (timer >= 2)
                cam.orthographic = true;
        }
    }
}
