using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class U_EndMenu : MonoBehaviour
{
    public void RestartButton()
    {
        SceneManager.LoadScene("U_Game");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
