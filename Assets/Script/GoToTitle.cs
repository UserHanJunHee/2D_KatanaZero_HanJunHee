using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToTitle : MonoBehaviour
{
    public void Title()
    {
        GameManager.instance.scenenum = GameManager.instance.scenestartnum;
        SceneManager.LoadScene(GameManager.instance.scenenum);
    }
}
