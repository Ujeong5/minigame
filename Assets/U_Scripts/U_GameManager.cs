using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class U_GameManager : MonoBehaviour
{
    public static U_GameManager gm;

    [SerializeField]
    private int minutes;
    private float timeValue;
    private float lastTimeToHead;

    [SerializeField]
    private Transform Head;
    [SerializeField]
    private int headTimer;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private AudioSource dollSing;
    [SerializeField]
    private AudioSource dollHeadOff;
    [SerializeField]
    private AudioSource dollHeadOn;
    [SerializeField]
    AudioSource feetSteps;

    public static bool headTime;
    public static bool headTimeFinish;

    public enum GameState
    {
        Ready,
        Run
    }
    public GameState gState;
    public GameObject gameLabel;
    Text gameText;


    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "3..";
        StartCoroutine(ReadyToStart());


        headTime = false;
        timeValue = minutes * 120;

    }
    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(1f);

        gameText.text = "2..";

        yield return new WaitForSeconds(1f);
        gameText.text = "1..";

        yield return new WaitForSeconds(1f);
        gameText.text = "시작!";

        yield return new WaitForSeconds(1f);
        gameLabel.SetActive(false);

        gState = GameState.Run;

        {
            dollSing.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gState != U_GameManager.GameState.Run)
        {
            return;
        }
        CountDown();

    }

    private void CountDown()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            SceneManager.LoadScene("U_TimeOut");
        }

        DisplayTime(timeValue);

        
    }


    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
            timeToDisplay = 0;

        float mins = Mathf.FloorToInt(timeToDisplay / 60);
        float secs = Mathf.FloorToInt(timeToDisplay % 60);
        HeadTime(secs);

        timeText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }

    private void HeadTime(float secs)
    {
        if (gState != U_GameManager.GameState.Run)
        {
            return;
        }

        print("HeadTime");
        
        if (timeValue <= 0)
        {
            headTimeFinish = true;
            RotHead(180);
            return;
        }

        if (secs % headTimer == 0 && secs != lastTimeToHead)
        {
            lastTimeToHead = secs;
            headTime = !headTime;

            if (headTime)
            {
                dollHeadOn.Play();
            }
            else
            {
                if (!dollHeadOff.isPlaying)
                    dollHeadOff.Play();

                if (!dollSing.isPlaying)
                   dollSing.Play();
            }
        }

        if (headTime)
            RotHead(180);
        else
            RotHead(0);
    }

    private void RotHead(int deg)
    {
        Vector3 direction = new Vector3(Head.rotation.eulerAngles.x, deg, Head.rotation.eulerAngles.z);
        Quaternion targetRotation = Quaternion.Euler(direction);
        Head.rotation = Quaternion.Lerp(Head.rotation, targetRotation, Time.deltaTime * 3);
    }

    public void StopSounds()
    {
        dollSing.Stop();
        dollHeadOff.Stop();
        dollHeadOn.Stop();
        feetSteps.Stop();
    }
}

