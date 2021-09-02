using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{

  

    SwipeControlScript swipePlayerScript;
    GameManager gameManagerScript;
    public GameObject player;
    public GameObject BackMusic;
    public GameObject JumpMusic;
    public GameObject TryAgainMusic;
    public GameObject LevelCompletedMusic;
    public Animator playerAnimator; 
    public Transform body; 
    private Rigidbody playerRigidbody;
    private Vector3 forceVelocity;

    private bool Fly = false;
    private bool Fall = false;
    private bool WingAngle = true;

    private const float Gravity= 0.2f;
    private const float ThrowingSpeed = 15f;
    private const float HRotationSpeed = 45;
    private const float FRotationSpeed = 800f;
    private float cubeSpeed = 30f;
    private float cylinderSpeed = 15f;
    private const float wingAngle = 40f;
    private const float BodyAngle = 90f;
    private const float MaxWingAngle = 45f;




    IEnumerator musicActive()
    {
        JumpMusic.SetActive(true);
        yield return new WaitForSeconds(1f);
        JumpMusic.SetActive(false);
    }

    private void Start()
    {
        BackMusic.SetActive(true);
        JumpMusic.SetActive(false);
        TryAgainMusic.SetActive(false);
        LevelCompletedMusic.SetActive(false);
    }



    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        swipePlayerScript = GetComponent<SwipeControlScript>();
        gameManagerScript = Camera.main.GetComponent<GameManager>();
       

    }

    
    void Update()
    {
        RotateBody();
    }

    public void PlayerVelocity(float rate) 
    {
        float radiantAngle = wingAngle * Mathf.Deg2Rad;
        float z = Mathf.Cos(radiantAngle) * ThrowingSpeed * rate;
        float y = Mathf.Sin(radiantAngle) * ThrowingSpeed * rate;

        forceVelocity = new Vector3(0, y, z);

        Invoke("TPlayer", 0.2f);    
    }
    private void TPlayer()
    {
        this.transform.parent = null;
        transform.rotation = Quaternion.identity;
        playerRigidbody.velocity = forceVelocity;
        FallAndFly();
        playerAnimator.SetBool("Rotating", true);
      Camera.main.GetComponent<CamScript>().ReadyFollow(); 
        swipePlayerScript.enabled = true;
    }
    public void FallAndFly() 
    {
        if (!Fall)
        {
            
            playerRigidbody.useGravity = true;
            Fly = false;
            Fall = true;
            playerAnimator.SetBool("Flying", false);
            playerAnimator.SetBool("Rotating", true);
        }
        else
        {
            
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Physics.gravity.y * Gravity, playerRigidbody.velocity.z);
            Fall = false;
            Fly = true;
            playerAnimator.SetBool("Rotating", false);
            playerAnimator.SetBool("Flying", true);
            WingAngle = false; 
        }
    }


    public void TurnHorizontal(float rate)
    {
        transform.Rotate(Vector3.up * rate * Time.deltaTime * HRotationSpeed);

       
        Vector3 normalizedVelocity = transform.forward;
        float x = normalizedVelocity.x * forceVelocity.z;
        float z = normalizedVelocity.z * forceVelocity.z;

        playerRigidbody.velocity = new Vector3(x, playerRigidbody.velocity.y, z);

    }

  
    private void RotatePlatform()
    { 
        swipePlayerScript.IsSwiping = false;
        if (Fly)
        {
            FallAndFly();
        }
    }

    private void RotateBody()
    {
        if (Fall)
        {
            if (!WingAngle)
            {
                body.localRotation = Quaternion.Lerp(body.localRotation, Quaternion.identity, FRotationSpeed * Time.deltaTime);
                float angleThreshold = Quaternion.Angle(body.localRotation, Quaternion.identity);
                if (angleThreshold < 0.001f)
                {
                    WingAngle = true;
                }
            }
            else 
            {
                body.Rotate(FRotationSpeed * Time.deltaTime, 0, 0, Space.Self);
            }
        }
        else if (Fly) 
        {
            float wingAngle = swipePlayerScript.GetPositionRate() * MaxWingAngle;
            Vector3 targetRotation = new Vector3(BodyAngle + wingAngle, BodyAngle, BodyAngle);
            body.localRotation = Quaternion.Lerp(body.localRotation, Quaternion.Euler(targetRotation), FRotationSpeed * Time.deltaTime);
        }
    }
    
    private void BouncePlayer(float bounceSpeed)
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, bounceSpeed, playerRigidbody.velocity.z);
    }
    
    private void BounceOnCube()
    {
        BouncePlayer(cubeSpeed);
        



    }
    private void BounceOnCylinder()
    {    
        BouncePlayer(cylinderSpeed);
       



    }
    private void BounceFan()
    {
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);

       

    }
    private void BounceMagnet()
    {
        player.GetComponent<Rigidbody>().AddForce(Vector3.down*50f);
       
    }
   
    private void OnTriggerEnter(Collider other)
    {
        RotatePlatform();
        if (other.CompareTag("CubeJump"))
        {
            BounceOnCube();
            StartCoroutine(musicActive());


        }
        else if (other.CompareTag("CylinderJump"))
        {
            BounceOnCylinder();
            StartCoroutine(musicActive());
        }
        else if (other.CompareTag("fan"))
        {
            BounceFan();
            StartCoroutine(musicActive());

        }
        else if (other.CompareTag("Magnet"))
        {
            BounceMagnet();

        }

    }
    private void OnCollisionEnter(Collision other)
    {
        playerAnimator.SetBool("Flying", false);
        playerRigidbody.useGravity = true;
        swipePlayerScript.enabled = false;
        Fall = false;
        Fly = false;

        if (other.gameObject.CompareTag("Platform"))
        {
            other.transform.parent.GetChild(0).gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
             gameManagerScript.GameOver();
            playerRigidbody.velocity = Vector3.zero;
            BackMusic.SetActive(false);
            TryAgainMusic.SetActive(true);

        }
        else if (other.gameObject.CompareTag("finish"))
        {
            gameManagerScript.EndGame();
            LevelCompletedMusic.SetActive(true);
            BackMusic.SetActive(false);
        }

    }
}

