using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    Transform player;//이러면 게임오브젝트가 아닌 트랜스폼만 쓰는거임 의미적으로 더 맞다.//이걸 퍼블릭으로해서 밖에서 넣어줘도 됨 많이 쓰는 방식이라함
                     //퍼블릭으로 하면 애 카메라 봤다 재 카메라봤다 바꿔줄수도 있어서 범용성이 올라감

    public PostProcessVolume volume;
    private Vignette vignette;

    float maxintensity = 0.4f;

    float bloommaxintensity = 0.5f;

    void Start()
    {
        volume.profile.TryGetSettings(out vignette);
        //    if (GameManager.instance.scenenum != 0)
        //        player = GameObject.Find("Player").transform;// 뒤에 pos를 붙여서 GameObject.Find("cat").transform.pos;를 하면 깊은 복사가 된다 그래서 안에 내용물만 바꾸는데 
        //                                              //이렇게 트랜스폼만 가져오면 아래서 바뀌면 여기도 바뀌는 참조가 된다 그래서 트랜스폼만 가져온다.
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.slowmode)
        {
            //volume.profile.TryGetSettings(out vignette);
            if(vignette.intensity <= maxintensity)
            {
                vignette.intensity.value += 0.002f;
            }

        }
        else
        {
            if (vignette.intensity >= 0)
            {
                vignette.intensity.value -= 0.001f;
            }
        }


        if (GameManager.instance.scenenum == 6)
        {
            transform.position = new Vector3(0, 0, -10);
            GetComponent<Camera>().orthographicSize = 5;
        }
        else if (GameManager.instance.scenenum != 0 && GameManager.instance.scenenum != GameManager.instance.scenestartnum && GameManager.instance.scenenum != GameManager.instance.sceneendnum)
        {
            if (player != null)
            {
                Vector3 playerPos = player.transform.position;
                transform.position = new Vector3(playerPos.x, playerPos.y + 1f, transform.position.z);
            }
            else
            {
                FindPlayer();
            }
        }
        
        else
        {
            GetComponent<Camera>().orthographicSize = 7;
            gameObject.SetActive(true);
            transform.position = new Vector3(0, 0, -10);
        }
        
        
    }
    public void FindPlayer()
    {
        GetComponent<Camera>().orthographicSize = 7;
        player = GameObject.Find("Player").transform;
        gameObject.SetActive(true);

        //player = GameObject.Find("Player").transform;
        //if (player != null)
        //{
        //    Vector3 playerPos = player.transform.position;
        //    transform.position = new Vector3(playerPos.x, playerPos.y + 1f, transform.position.z);//의미가 중요하니깐 x,z에 0 넣지말고 변하지 않는다는 의미로 기본 좌표를 넣자.
        //}
        //else
        //{
        //    player = GameObject.Find("Player").transform;
        //}
    }
}
