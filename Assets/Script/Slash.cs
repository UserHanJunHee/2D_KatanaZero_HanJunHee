using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    Vector3 mousePos;//마우스 위치

    Transform player;
    public AudioClip[] slashsound;
    AudioSource audioSource;
    public BoxCollider2D slash;

    //float speed = 100f;//누른방향으로 베는 이팩트가 살짝 앞으로 가기 위해서
    int sound;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();//플레리어 위치 가져옴
        audioSource = GetComponent<AudioSource>();
        slash = GetComponent<BoxCollider2D>();

        Atk();
    }

    private void OnEnable()
    {
        Atk();
        
        if(!slash.enabled)
            slash.enabled = true;//칼이나 총알은 베면 콜라이더가 꺼지게 해놔서 다시 켜준다 1회공격에 1회만 베게끔 해준 용도
    }

    private void Atk()
    {
        if (player == null) return;

        audioSource.clip = slashsound[sound = Random.Range(0, 3)];
        audioSource.Play();

        transform.position = player.position;//다시 활성화할때 본래 자리로 되돌려 놓기 위해서(0,0,0)

        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);//마우스 누른 지점

       
        Vector3 dir = (mousePos - transform.position).normalized;//마우스 클릭 방향
        //transform.Translate(Vector3.forward * Time.deltaTime);


        if (dir.x < 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);//오른쪽을보고 있어도 왼쪽을 공격하면 회전해서 때리게 넣어줌.
            transform.localScale = new Vector3(-1f, -1f, 1f);//이팩트 방향
        }
        else
        {
            player.transform.localScale = new Vector3(1, 1, 1);//위와 같은 이유
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        // 타겟 방향으로 회전함
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // 타겟 방향으로 다가감
        //dir.z = -1f;
        //transform.position += dir * speed * Time.deltaTime;//누른 방향으로 살짝 앞으로 이동해서 공격 모션이 나옴

        
    }

}
