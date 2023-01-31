using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class U_PlayerMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;

    [SerializeField]
    private Animator anim;

    private bool isJumping;
    private bool isWalking;
    private bool isMoving;
    private bool isDying;
    private bool isInDeathZone;

    [SerializeField]
    private LayerMask FloorMask;

    [SerializeField]
    private Transform FeetTransform;

    [SerializeField]
    private Transform PlayerCamera;

    [SerializeField]
    private Rigidbody PlayerBody;

    [Space]

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float Sensitivity;

    [SerializeField]
    private float Jumpforce;

    [SerializeField]
    AudioSource feetSteps;

    [SerializeField]
    AudioClip Apple;
    

    [SerializeField]
    private Transform deathZone;

    public string targetObjectName;
    GameObject smObject;
   
    U_ScoreManager sm;

    GameObject gmObject;
    U_GameManager gm;

   

    //목표 오브젝트 이름은 인스펙터에서 지정
    void Start()
    {
        Time.timeScale = 1;
        smObject = GameObject.Find("ScoreManager");
        sm = smObject.GetComponent<U_ScoreManager>();

        gmObject = GameObject.Find("GameManager");
        gm = gmObject.GetComponent<U_GameManager>();

    }
    private void OnCollisionEnter(Collision collision)//사과 20개 다 모으고 FinishLine에 닿으면 움직임 멈춤
    {
        //GameObject smObject = GameObject.Find("U_ScoreManager");
        //U_ScoreManager sm = smObject.GetComponent<U_ScoreManager>();
        smObject = GameObject.Find("ScoreManager");
        sm = smObject.GetComponent<U_ScoreManager>();

        
        if (collision.gameObject.name == "Apple")
        {
            

            sm.appleAmount += 1;
            sm.appleAmountUI.text = "획득 사과: " + sm.appleAmount;

            Destroy(collision.gameObject);//사과에 닿으면 사과 삭제

            feetSteps.PlayOneShot(Apple);//사과 획득 시 효과음 발생
        }
        //if (collision.gameObject.name == targetObjectName && sm.appleAmount == 20)
        if(collision.gameObject.name == targetObjectName)
        {
            print(targetObjectName);

            if(sm.appleAmount == 15)
            {
                Time.timeScale = 0;

                SceneManager.LoadScene("U_Clear");
            }
        }
        
    }
   
    // Update is called once per frame
    void Update()
    {
        
        if (U_GameManager.gm.gState != U_GameManager.GameState.Run)
        {
            return;
        }

        if (isDying)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        PlayerMovementInput = new Vector3(h, 0f, v);
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        CheckJumping();
        CheckMoving();
        MovePlayer();
        MovePlayerCamera();

        isWalking = (h != 0 || v != 0);
        anim.SetBool("isWalking", isWalking);

        if (isWalking && !isJumping && !feetSteps.isPlaying)
        {
            feetSteps.loop = true;
            feetSteps.Play(0);
        }
        else
        {
            feetSteps.loop = false;
        }

        CheckDeathTime();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
                PlayerBody.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
        }
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void CheckJumping()
    {
        isJumping = !Physics.CheckSphere(FeetTransform.position, 0.1f, FloorMask);
        anim.SetBool("isJumping", isJumping);
    }

    private void CheckMoving()
    {
        isMoving = isJumping || isWalking;
    }

    private void CheckDeathTime()
    {
        if (U_GameManager.headTime && isMoving || U_GameManager.headTimeFinish)
        {
            if (!isInDeathZone)
                return;

            isDying = true;
            anim.SetBool("isDying", true);
            feetSteps.Stop();
            //shoot.Play(0); //+game over 창 띄움
            SceneManager.LoadScene("U_EndScene");
            
        }
    }

  private void OnTriggerEnter(Collider other)
    {
        isInDeathZone = other.transform == deathZone;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == deathZone)
            isInDeathZone = false;
    }
   
}
