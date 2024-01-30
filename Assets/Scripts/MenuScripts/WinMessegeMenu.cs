using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMessegeMenu : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
