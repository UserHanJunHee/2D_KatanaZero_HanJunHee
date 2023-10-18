using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public float enemynum;
    public int scenenum = 0;//디버그 씬 하나 끼어 넣어서 1부터 시작
    public int scenestartnum = 1;
    public int sceneendnum = 7;//엔딩 씬 넘버


    public bool slowmode = false;


    float delta = 0; // 다음 스테이지 혹은 다시하기할때 바로 안넘어가게 할려고 시간 제한 주기 위한 용도.


    public bool playerdie = false;
    
    public GameObject centerText;
    public GameObject timegauge;//시간 가속
    public GameObject topUI;
    public Image timer;//시간제한


    // 시간 액션

    private bool timeaction = false;


    void Start()
    {
        enemynum = 101f;
        PlayerPrefs.SetFloat("EnemyNum",enemynum);
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scenenum == 0)
        {
            SceneManager.LoadScene(++scenenum);
            //이 부분은 다시 활성화 시켜줘야 하는 부분입니다. 
            //테스트 용으로 비활성화.
        }

        //if(timer.fillAmount <=0)
        //{
        //    playerdie = true;
        //}

        if (scenenum != scenestartnum && scenenum != sceneendnum)
        {
            topUI.SetActive(true);
            if (!playerdie && enemynum >0)
            {
                //시간 흐르는곳
                //if (scenenum == 6)
                //{
                //    timer.fillAmount -= 0.0001f;
                //}
                //else
                //{
                //    timer.fillAmount -= 0.0003f;
                //}
            }

            if (slowmode && !timeaction)
            {
                if(playerdie)
                {
                    slowmode = false;
                }
                else if(timegauge.GetComponent<Image>().fillAmount <= 0f)
                {
                    slowmode = false;
                }
                else if(Input.GetMouseButtonUp(1))
                {
                    slowmode = false;
                }
                timegauge.GetComponent<Image>().fillAmount -= 0.004f;
                Time.timeScale = 0.2f;
            }
            else if(!slowmode && !timeaction)
            {
                Time.timeScale = 1f;
                if(Input.GetMouseButtonDown(1) && timegauge.GetComponent<Image>().fillAmount >= 0.1f && !playerdie)
                {
                    slowmode = true;
                }
                else if(timegauge.GetComponent<Image>().fillAmount < 1f && !playerdie)
                {
                    timegauge.GetComponent<Image>().fillAmount += 0.002f;
                }
            }

            //if(Input.GetMouseButton(1) && timegauge.GetComponent<Image>().fillAmount <= 0)
            //{
            //    Time.timeScale = 1f;
            //}
            //else if(Input.GetMouseButton(1)&&timegauge.GetComponent<Image>().fillAmount >= 0)
            //{
            //    timegauge.SetActive(true);
            //    timegauge.GetComponent<Image>().fillAmount -= 0.005f;
            //    Time.timeScale = 0.2f;
            //}
            //else if (Input.GetMouseButtonUp(1))
            //{
            //    Time.timeScale = 1f;             
            //}

            //if (Input.GetMouseButtonUp(1) && timegauge.GetComponent<Image>().fillAmount >= 1)
            //{
            //    timegauge.SetActive(false);
            //}
            //else if(Input.GetMouseButtonUp(1) && timegauge.GetComponent<Image>().fillAmount <= 1)
            //{
            //    timegauge.GetComponent<Image>().fillAmount += 0.001f;
            //}
        }
        else
        {
            topUI.SetActive(false);
            Time.timeScale = 1f;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            NextScenes();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            scenenum = 5;
            NextScenes();
        }


        if (enemynum <= 0 && scenenum != scenestartnum && !playerdie)
        {
            centerText.GetComponent<Text>().text = "그래 이렇게 하면 되겠지.";
            centerText.SetActive(true);

            delta += Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && delta > 0.5f)
            {
                enemynum = 101;
                centerText.SetActive(false);
                delta = 0;
                NextScenes();
            }
        }
        else if (playerdie)
        {
            
            if (timer.GetComponent<Image>().fillAmount <= 0f)
            {
                centerText.GetComponent<Text>().text = "오래도 걸리는군.";
            }
            else
            {
                centerText.GetComponent<Text>().text = "아니 통하지 않을꺼야.";
            }
            centerText.SetActive(true);
            delta += Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && delta > 0.5f)
            {
                enemynum = 101;
                delta = 0;
                playerdie = false;
                centerText.SetActive(false);
                timer.fillAmount = 1f;
                timegauge.GetComponent<Image>().fillAmount = 1f;
                slowmode = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);               
            }
        }
    }




    public void NextScenes()
    {
        //GameObject.Find("Main Camera").GetComponent<CameraController>().FindPlayer();
        enemynum = 101;
        timegauge.GetComponent<Image>().fillAmount = 1f;
        timer.fillAmount = 1f;
        slowmode = false;

        SceneManager.LoadScene(++scenenum);
        
    }

    public void EndGame()
    {
        Application.Quit();
    }
    public void Title()
    {
        scenenum = scenestartnum;
        SceneManager.LoadScene(scenenum);
    }
    public void PlayerDie()
    {
        playerdie = true;
    }

    public void TimeAction()
    {
        timeaction = true;
        Invoke("TimeRecovery", 0.01f);
        Time.timeScale = 0.03f;
    }
    public void TimeRecovery()
    {
        timeaction = false;
        Time.timeScale = 1f;  
    }
    
}
