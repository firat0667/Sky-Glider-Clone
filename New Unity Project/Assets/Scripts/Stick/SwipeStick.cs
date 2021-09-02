using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeStick : MonoBehaviour
{
    
    StickControll stickcontrollerScript;
    private const float MinDistance = 0.22f;
    private const float MaxDistance = 0.44f;

    

    private float cPositionRate;  
    private bool Swipable = true;
    private float firstPosition;





    private void Awake()
    {
        stickcontrollerScript = this.GetComponent<StickControll>();
        stickcontrollerScript = GameObject.FindGameObjectWithTag("Stick").GetComponent<StickControll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Swipable)
        {
            CheckSwipe();
        }

    }

    private void CheckSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
            cPositionRate = 0;
            stickcontrollerScript.Pull();
        }
        else if (Input.GetMouseButton(0))
        {
            SetPositionRate();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetPositionRate();
            Swipe();
        }

    }

    private void SetPositionRate() 
    {
        float distance = CheckSwipeDistance();
        if (distance >= MaxDistance)
            cPositionRate = 1f;
        else if (distance <= 0)
            cPositionRate = 0f;
        else
        {
            cPositionRate = distance / MaxDistance;
        }
    }
    public float PositionRate()
    {
        return cPositionRate;
    }

    public float CheckSwipeDistance() 
    {
        float lastPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
        return firstPosition - lastPosition;
    }
    private void Swipe() 
    {
        if (CheckSwipeDistance() >= MinDistance)
        {
            Swipable = false;
            stickcontrollerScript.Release();
        }
        else
        {
            stickcontrollerScript.Reverse();
        }
    }
     public void Exit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stickcontrollerScript.ReleaseFinished();
    }
}
