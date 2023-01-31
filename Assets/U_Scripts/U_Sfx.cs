using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Sfx : MonoBehaviour
{
    static AudioSource audioSource;
    public static AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioClip = Resources.Load<AudioClip>("Apple") as AudioClip; // Resources 폴더 안의 "Apple"효과음을 audioClip 변수에 저장
    }
   
    // Update is called once per frame
    void Update()
    {

    }
    
}
