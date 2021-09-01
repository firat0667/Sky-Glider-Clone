using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickControll : MonoBehaviour
{
    private Animator stickAnim;
    private SwipeStick swipeStick;
    private PlayerControlScript playerControll;
    private int stateHash;
    private bool StickPull = false;





    // Start is called before the first frame update
    private void Awake()
    {
        stickAnim= this.GetComponent<Animator>();

        swipeStick = this.GetComponent<SwipeStick>();
        playerControll = GameObject.FindGameObjectWithTag("Rocketman").GetComponent<PlayerControlScript>();

    }
    void Start()
    {
        SetStickSpeed(0f);
        stateHash = Animator.StringToHash("Base Layer.Armature|Bend_Stick");
    }

    // Update is called once per frame
    void Update()
    {
        if (StickPull)
        {
            stickAnim.Play(stateHash, 0, swipeStick.PositionRate());
        }

    }

    #region Stick states
    public void Pull()
    {
        SetStickSpeed(0f);
        StickPull = true;
    }
    public void Reverse() 
    {
        StickPull = false;
        SetStickSpeed(-1f);
    }
    public void Release()
    {
       StickPull = false;
        stickAnim.SetTrigger("Release");
        SetStickSpeed(1f);
        playerControll.PlayerVelocity(swipeStick.PositionRate());
    }
    public void ReleaseFinished()
    {
        DisableStick();
    }
    public void SetStickSpeed(float value) 
    {
        stickAnim.SetFloat("SpeedMultiplier", value);
    }
    #endregion
    public void DisableStick()
    {
        swipeStick.enabled = false;
        this.GetComponent<Animator>().enabled = false;
        this.enabled = false;
    }
}
