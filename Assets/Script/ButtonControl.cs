using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    public void Title()
    {
        GameManager.instance.Title();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(++GameManager.instance.scenenum);
    }

    public void QuitGame()
    {
        GameManager.instance.EndGame();
    }

    public void GotoDebugScean()
    {
        GameManager.instance.scenenum = 99;//디버그
        SceneManager.LoadScene("DebugScene");
    }
}
