using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControlScript : MonoBehaviour
{

    PlayerControlScript playerControll;

    public GameObject player;
    public GameObject stick;
    private const float MaxSwipeDistance = 0.4f;
    private bool Swiping = false;
    private float PositionRate;
    private float firstSwipe;
    public bool IsSwiping { set { Swiping = value; } }




    private void Awake()
    {
        playerControll = this.GetComponent<PlayerControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
        SwipeControll();
    }

    private void SwipeControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Swiping = true;
            firstSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
            if (player.transform.position.z > stick.transform.position.z)
            {
                playerControll.FallAndFly();
            }
                      
        }
        if (Swiping)
        {
            if (Input.GetMouseButton(0))
            {
                SetPositionRate();
                playerControll.TurnHorizontal(GetPositionRate());
            }
            if (Input.GetMouseButtonUp(0))
            {
                Swiping = false;
                if (player.transform.position.z > stick.transform.position.z)
                {
                    playerControll.FallAndFly();
                }
               
            }
        }


    }

    private void SetPositionRate() 
    {
        float distance = CheckSwipeDistance();

        if (Mathf.Abs(distance) >= MaxSwipeDistance)
        {
            PositionRate = 1f;
            PositionRate *= Mathf.Sign(distance);
        }
        else
        {
            PositionRate = distance / MaxSwipeDistance;
        }
    }
    public float GetPositionRate()
    {
        return PositionRate;
    }

    public float CheckSwipeDistance() 
    {
        float lastPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
        return Camera.main.ScreenToViewportPoint(Input.mousePosition).x - 0.5f; 
    }

    public void setSwiping(bool state)
    {
        Swiping = state;
    }
}
