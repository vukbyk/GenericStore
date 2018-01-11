using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
//using UnityEditor;

//#if UNITY_EDITOR
//using UnityEditor;
// #endif
// // other code, class definition blah blah
// #if UNITY_EDITOR
// EditorUtility.InstanceIDToObject();
// #endif

public class TakePhoto : MonoBehaviour
{
    float panelSpeed=0.5f;

    public enum GameMode { movie, playing, posing, showPhotoQR, toAcceptPhoto }
    public /*static*/ GameMode mode;
    public ProgressBar.ProgressRadialBehaviour StartTimeIconBar;
    public ProgressBar.ProgressRadialBehaviour SaveTimeIconBar;

    public float instructionDelayTime = 5.0f;
    public float photoDelayTime = 5.0f;
    public float iconDelayTime = 3.0f;
    public float showPhotoTime = 5.0f;
    public float acceptPhotoTime = 20;

    float faseTime;


    float photoTimer;
    float iconTimer;
    float showTimer;
    float acceptTimer;


    byte[] fileData;
    Texture2D tex;

    public GameObject playing;
    public GameObject posing;
    public GameObject showPhotoQR;
    public GameObject toAcceptPhoto;

    public RawImage photo;
    public GameObject photoCanvas;
    public GameObject ProgressBarIcon;
    public GameObject[] numbersCountdown = new GameObject[6];
    //public float rotationSpeed = .5f;
    Rigidbody rb;

    KinectManager kinectManager;
   
    public GameObject avatar;
    Vector3 startAvatarPosition;
    bool lastFrameActive = false;
    bool activateFix = true;

    PoseActivateScreenShoot pose;

    //enum Direction { waiting, takingPhoto };
    //bool takingPhotoProcess = false;

    float speed = 1.0f;
    Quaternion deltaRotation;
    Renderer rend;

    Vector3 startPosition;
    Vector3 endPosition;

    string screenShot;

    // Use this for initialization
    void Start ()
    {
        if (!System.IO.Directory.Exists("C:/movies/"))
        {
            Directory.CreateDirectory("C:/movies/");
        }
        if (!System.IO.Directory.Exists(Application.dataPath + "/ScreenShots/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/ScreenShots/");
        }
        screenShot = Application.dataPath + "/ScreenShots/photo.png";
        faseTime =0;
        kinectManager = (KinectManager)FindObjectOfType(typeof(KinectManager));
        //KinectManager kinectManager = KinectManager.Instance;
        startAvatarPosition = avatar.transform.position;

        startPosition = new Vector3(playing.transform.position.x, playing.transform.position.y, playing.transform.position.z);
        endPosition = new Vector3(playing.transform.position.x + 535, playing.transform.position.y, playing.transform.position.z);

        mode = GameMode.movie;
        pose = avatar.GetComponent<PoseActivateScreenShoot>();
        photoTimer = 0;
        iconTimer = 0;
        showTimer = 0;
        acceptTimer = 0;

        ProgressBarIcon.SetActive(true);
        //photoCanvas.SetActive(false);

        for (int i = 1; i <= 5; i++)
        {
            numbersCountdown[i].SetActive(false);
        }


        //rend = GetComponent<Renderer>();
        //rb = GetComponent<Rigidbody>();
        //deltaRotation = Quaternion.Euler(new Vector3(rotationSpeed*3, rotationSpeed, 0));
        //rend.material.SetColor("_EmissionColor", new Color(1.0f, 0.0f, 0.0f) );
    }

    //void FixedUpdate()
    //{
    //    //if (rb.position.x <= 1.0f)
    //    {
    //        //Quaternion direction = new Quaternion(1,0,0,1);
    //        //rb.MoveRotation(rb.transform.rotation * deltaRotation);//* Time.fixedDeltaTime);
    //    }
    //}

    private void Update()
    {
        faseTime += Time.deltaTime;
        //if (kinectManager != null && kinectManager.IsInitialized() && kinectManager.IsUserDetected() && lastFrameActive == false)
        //{
        //    lastFrameActive = true;
        //    activateFix = true;
        //}
        //if (activateFix)
        //{
        //    Debug.Log("IsPRAVI");
        //    activateFix = false;
        //    Vector3 position = avatar.transform.position;
        //    position.x *= 10;
        //    avatar.transform.position = position;
        //}
        //if (kinectManager != null && kinectManager.IsInitialized() && !kinectManager.IsUserDetected())
        //{
        //    //Debug.Log("false    ");
        //    lastFrameActive = false;
        //}
        //Debug.Log(mode);
        //StartTimeIconBar.SetFillerSizeAsPercentage(iconTimer * 100 / iconDelayTime);

        if (mode != GameMode.movie && kinectManager != null && kinectManager.IsInitialized() && !kinectManager.IsUserDetected() && lastFrameActive == false)
        {
            mode = GameMode.movie;
            acceptTimer = 0;
            showTimer = 0;
            faseTime = 0;

        }


        if (mode == GameMode.movie)
        {
            showPhotoQR.transform.position = Vector3.Lerp(showPhotoQR.transform.position, startPosition, faseTime * panelSpeed);
            playing.transform.position = Vector3.Lerp(playing.transform.position, startPosition, faseTime * panelSpeed);
            posing.transform.position = Vector3.Lerp(posing.transform.position, startPosition, faseTime * panelSpeed);
            toAcceptPhoto.transform.position = Vector3.Lerp(toAcceptPhoto.transform.position, startPosition, faseTime * panelSpeed);
            if (kinectManager != null && kinectManager.IsInitialized() && kinectManager.IsUserDetected() && lastFrameActive == false)
            {
                mode = GameMode.playing;
                acceptTimer = 0;
                showTimer = 0;
                faseTime = 0;
                
            }
        }

        if (mode == GameMode.playing)
        {
            toAcceptPhoto.transform.position = Vector3.Lerp(toAcceptPhoto.transform.position, startPosition, faseTime * panelSpeed );
            if (faseTime > instructionDelayTime)
            {
                playing.transform.position =
                    Vector3.Lerp(playing.transform.position, endPosition, faseTime - instructionDelayTime - Time.deltaTime * panelSpeed);
            }

            ProgressBarIcon.SetActive(true);//can optimise moving to callers else
            StartTimeIconBar.SetFillerSizeAsPercentage(iconTimer * 100 / iconDelayTime);
            if (pose.IsPoseMatched())
            {
                iconTimer += Time.deltaTime;
                if (iconTimer >= iconDelayTime)
                {
                    photoTimer = photoDelayTime;
                    ProgressBarIcon.SetActive(false);
                    iconTimer = 0;
                    mode = GameMode.posing;
                    faseTime = 0;
                }
            }
            else
            {
                iconTimer = 0;
            }
        }

        else if (mode == GameMode.posing)
        {
            playing.transform.position = Vector3.Lerp(playing.transform.position, startPosition, faseTime * panelSpeed);
            posing.SetActive(true);
            posing.transform.position = Vector3.Lerp(posing.transform.position, endPosition, faseTime * panelSpeed );
            if (photoTimer > 0)
            {
                photoTimer -= Time.deltaTime;
                float c = (Mathf.Sin((-0.1f + photoTimer) * Mathf.PI));// + 1) / 2;
                //rend.material.SetColor("_EmissionColor", new Color(c, c, c));
                for (int i = 1; i <= 5; i++)
                {
                    numbersCountdown[i].SetActive(false);
                }
                drawCountdownFrom5();
            }
            else
            {
                //Application.CaptureScreenshot("ScreenShots\\" + System.DateTime.Now.ToString("MM-dd-yy_hh-mm-ss") + ".png");
                //FileUtil.DeleteFileOrDirectory("ScreenShots\\" + "photo.png");
                posing.SetActive(false);
                //if ( System.IO.File.Exists("ScreenShots\\photo.png") )
                    //FileUtil.CopyFileOrDirectory("ScreenShots\\" + "photo.png", "ScreenShots\\" + System.DateTime.Now.ToString("MM - dd - yy_hh - mm - ss") + ".png");
                Application.CaptureScreenshot(screenShot);
                //Application.CaptureScreenshot("ScreenShots\\" + System.DateTime.Now.ToString("MM - dd - yy_hh - mm - ss") + ".png");

                tex = new Texture2D(2, 2);
                photo.texture = tex;

                //tex = new Texture2D(1280, 720, TextureFormat.RGB24, false);
                //tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                //tex.Apply();
                //photo.texture = tex;

                photoTimer = 0;
                acceptTimer = acceptPhotoTime;
                mode = GameMode.toAcceptPhoto;
                

                faseTime = 0;
            }
        }

        else if (mode == GameMode.toAcceptPhoto)
        {
            posing.SetActive(true);
            posing.transform.position = Vector3.Lerp(posing.transform.position, startPosition, faseTime * panelSpeed);
            toAcceptPhoto.transform.position = Vector3.Lerp(toAcceptPhoto.transform.position, endPosition, faseTime * panelSpeed);
            photoCanvas.SetActive(true);//can optimise moving to callers else
            if (System.IO.File.Exists(screenShot) && acceptTimer > 0 && acceptTimer < acceptPhotoTime - 1)
            {
                fileData = File.ReadAllBytes(screenShot);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                photo.texture = tex;
            }
            else
            {
                Debug.Log(screenShot + " does not exist");
            }

            if (acceptTimer > 0 )
            {
                if(iconTimer <= 0 )
                    acceptTimer -= Time.deltaTime;

                SaveTimeIconBar.SetFillerSizeAsPercentage(iconTimer * 100 / iconDelayTime);
                if (pose.IsPoseMatched())
                {
                    iconTimer += Time.deltaTime;
                    if (iconTimer >= iconDelayTime)
                    {
                        photoTimer = photoDelayTime;
                        //ProgressBarIcon.SetActive(false);
                        faseTime = 0;
                        iconTimer = 0;
                        showTimer = showPhotoTime;
                        mode = GameMode.showPhotoQR;
                        
                    }
                }
                else
                {
                    iconTimer = 0;
                }
            }
            else
            {
                //photoCanvas.SetActive(false);


                mode = GameMode.movie;
            }
        }





        else if (mode == GameMode.showPhotoQR)
        {
            toAcceptPhoto.transform.position = Vector3.Lerp(toAcceptPhoto.transform.position, startPosition, faseTime * panelSpeed);
            showPhotoQR.transform.position = Vector3.Lerp(showPhotoQR.transform.position, endPosition, faseTime * panelSpeed);
            //photoCanvas.SetActive(true);//can optimise moving to callers else
            //if (System.IO.File.Exists("ScreenShots\\photo.png") && showTimer > 0 && showTimer < showPhotoTime - 1)
            //{
            //    fileData = File.ReadAllBytes("ScreenShots\\photo.png");
            //    tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            //    photo.texture = tex;
            //}
            //else
            //{
            //    Debug.Log("ScreenShots\\photo.png" + " does not exist");
            //}

            if (showTimer > 0)
            {
                showTimer -= Time.deltaTime;
            }
            else
            {
                //photoCanvas.SetActive(false);                
                showTimer = 0;
                mode = GameMode.movie;
            }
        }
    }

    void drawCountdownFrom5()
    {
        if (photoTimer <= 5 && photoTimer > 4)
        {
            numbersCountdown[5].SetActive(true);
        }
        else if (photoTimer <= 4 && photoTimer > 3)
        {
            numbersCountdown[4].SetActive(true);
        }
        else if (photoTimer <= 3 && photoTimer > 2)
        {
            numbersCountdown[3].SetActive(true);
        }
        else if (photoTimer <= 2 && photoTimer > 1)
        {
            numbersCountdown[2].SetActive(true);
        }
        else if (photoTimer <= 1 && photoTimer > 0)
        {
            numbersCountdown[1].SetActive(true);
        }
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log("OnCollisionEnter");
    //    //if (col.gameObject.name == "Hand")
    //    {
    //        Application.CaptureScreenshot("ScreenShots\\Screenshot.png");
    //    }
    //}
    //void OnTriggerEnter(Collider c)
    //{
    //    Debug.Log("OnTriggerEnter");
    //    //if (col.gameObject.name == "Hand")
    //    if (!takingPhoto)
    //    {
    //        photoTimer = photoDelayTime;
    //        takingPhoto = true;
    //    }
    //}
}
