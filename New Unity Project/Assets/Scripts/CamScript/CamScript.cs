using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    
    public Transform tCamPosition;
    public Transform targetLookPosition;
    public Transform lookPosition; 
    

    private float followSpeed = 3f;

    private bool  Following = false;

    private bool  CameraPositionX = false; 
    private bool  LookAtPositionX = false; 

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(lookPosition); 
    }


    void FixedUpdate()
    {
        if (Following)
        {
            Follow();
        }

    }
   
    private void Follow()
    {
        transform.LookAt(lookPosition);

        if (!CameraPositionX)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * followSpeed);
            if (Vector3.Distance(transform.localPosition, Vector3.zero) < 0.001f)
            {
                transform.localPosition = Vector3.zero;
                CameraPositionX = true;
            }
        }
        if (!LookAtPositionX)
        {
            lookPosition.localPosition = Vector3.Lerp(lookPosition.localPosition, targetLookPosition.localPosition, Time.deltaTime * followSpeed);
            if (Vector3.Distance(lookPosition.localPosition, targetLookPosition.localPosition) < 0.001f)
            {
                lookPosition.localPosition = targetLookPosition.localPosition;
                LookAtPositionX = true;
            }
        }
        if (CameraPositionX && LookAtPositionX)
        {
            this.enabled = false; 
        }
    }
    public void ReadyFollow()
    {
        this.transform.parent = tCamPosition;
        Following = true;
    }
}
