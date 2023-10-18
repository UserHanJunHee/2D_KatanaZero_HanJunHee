using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHunter : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 6;//hp;
    float hitForce = 100f;//공격 맞고 밀려나가는 범위
    float attackdelay = 1f;
    int attackpattern = -1;
    bool isattack = false;
    float delta = 0f; // 피격후 에어스트라이크 공격 빈도
    int airstrike = 5;//피격후 에어스트라이크 공격 횟수


    bool takedamage = false;
    public bool isdead = false;//에어스트라이크 그라운드 스트라이크 죽었을때 제거용도 
    bool roll = false;

    //맞았을때 날라가기 위한 용도
    bool hitpos = false;
    //피격 폭발 생성
    bool makehitexplosion = false;

    //대쉬 공격
    bool dashattack = false;
    bool dashattackend = false;
    bool dashattacksound = false;
    //페스트 건
    bool takeoutgun = false;
    bool putbackgun = false;
    //공중제비
    public GameObject bulletairprefab;
    int shootbulet = 20;
    bool jumpattack = false;
    bool jumpland = false;
    bool jumppos = false;
    bool airshoot = false;
    bool rightjump = false;

    Transform playerpos;
    private Animator animator;
    private GameObject attackbox;
    private Rigidbody2D rb2d;

    public AudioClip[] bosssound;
    AudioSource audioSource;

    Vector3 pos;

    public GameObject sweepPrefab;
    public GameObject groundstrikePrefab;
    public GameObject airstrikePrefab;
    public GameObject bulletPrefab;
    public GameObject hitexplosion;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerpos = GameObject.Find("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        attackbox = transform.GetChild(0).gameObject;
        rb2d = GetComponent<Rigidbody2D>();


        if (GameManager.instance.enemynum > 100)
        {
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum = 1);
        }
        else
        {
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum++);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Random r = new Random(0, 3); 
        if (isdead) return;//사망

        else
        {
            if (!takedamage)
            {
                //공격 패턴 뽑기
                if (!isattack && !GameManager.instance.playerdie)
                    attackdelay += Time.deltaTime;

                if (attackdelay > 0.6f && !GameManager.instance.playerdie)
                {
                    isattack = true;
                    attackdelay = 0f;
                    attackpattern = Random.Range(0, 3);
                    //attackpattern = 1;

                    switch (attackpattern)
                    {
                        case 0:
                            dashattack = true;
                            break;
                        case 1:
                            takeoutgun = true;
                            break;
                        case 2:
                            jumpattack = true;
                            break;
                    }
                }

                //플레이어 쳐다보기
                if (transform.position.x < playerpos.transform.position.x && !jumpattack)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (transform.position.x > playerpos.transform.position.x && !jumpattack)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }

                //대쉬 어택
                if (dashattack)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        dashattack = false;
                        dashattackend = true;
                        if (transform.position.x < playerpos.transform.position.x)//플레이어가 오른쪽
                        {
                            Vector3 tel = new Vector3(playerpos.position.x - 0.3f, transform.position.y, transform.position.z);
                            transform.position = tel;
                        }
                        else//플레이어가 왼쪽
                        {
                            Vector3 tel = new Vector3(playerpos.position.x + 0.3f, transform.position.y, transform.position.z);
                            transform.position = tel;
                        }
                    }
                }
                else if (dashattackend)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        dashattackend = false;
                        isattack = false;
                        dashattacksound = false;
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
                    {
                        attackbox.SetActive(false);
                    }
                    else
                    {
                        if(!dashattacksound)
                        {
                            if (audioSource.clip != bosssound[2])
                            {
                                audioSource.clip = bosssound[2];
                            }
                            audioSource.Play();
                            dashattacksound = true;
                        }
                        
                        attackbox.SetActive(true);
                    }
                }  
                

                //권총사격
                if(takeoutgun)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        takeoutgun = false;
                        putbackgun = true;

                        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    }
                }
                else if(putbackgun)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        putbackgun = false;
                        isattack = false;
                    }
                }
                //점프 공격

                if (jumpattack && !jumpland)
                {
                    if(transform.position.x > 0 && !jumppos)//가운대 기점으로 우측에 있을때 좌측으로 점프
                    {
                        rb2d.AddForce(new Vector2(-300f, 300f));
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        rightjump = false;
                        jumppos = true;

                        if (audioSource.clip != bosssound[0])
                        {
                            audioSource.clip = bosssound[0];
                        }
                        audioSource.Play();
                    }
                    else if (transform.position.x <= 0 && !jumppos)//가운대 기점으로 좌측에 있을때 우측으로 점프
                    {
                        rb2d.AddForce(new Vector2(300f, 300f));
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        rightjump = true;
                        jumppos = true;

                        if (audioSource.clip != bosssound[0])
                        {
                            audioSource.clip = bosssound[0];
                        }
                        audioSource.Play();
                    }

                    
                    

                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && airshoot)//점프 애니메이션을 끝까지 재생하면 착지 모션으로 바꿈
                    {
                        jumpland = true;             
                    }
                    else if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !airshoot)
                    {
                        airshoot = true;
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f && !airshoot)//애니메이션 60퍼 진행하면 총알을 쏨
                    {
                        if (audioSource.clip != bosssound[6])
                        {
                            audioSource.clip = bosssound[6];
                        }
                        audioSource.Play();
                        StartCoroutine("AirShot");
                        //Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        //airshoot = true; 
                        airshoot = true;
                        
                    }
                }
                else if (jumpland)
                {
                    if(jumppos)
                    {
                        rb2d.velocity = Vector2.zero;
                        if (rightjump)
                            rb2d.AddForce(new Vector2(200, -390f));
                        else
                            rb2d.AddForce(new Vector2(-200, -390f));
                        jumppos = false;
                    }
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        if (audioSource.clip != bosssound[1])
                        {
                            audioSource.clip = bosssound[1];
                        }
                        audioSource.Play();


                        jumpland = false;
                        jumpattack = false;
                        airshoot = false;
                        isattack = false;
                        rb2d.velocity = Vector2.zero;
                        transform.position = new Vector2(transform.position.x, -3.08f);
                    }
                }

            }
            else//데미지를 받았다!
            {           
                if (transform.localScale.x == -1 && !hitpos)
                {
                    hitpos = true;      
                    rb2d.AddForce(new Vector2(hitForce, 0));
                }
                else if (transform.localScale.x == 1 && !hitpos)
                {
                    hitpos = true;           
                    rb2d.AddForce(new Vector2(-hitForce, 0));
                }

                if(hitpos)
                {
                    if (transform.position.x > 7)
                    {
                        rb2d.velocity = Vector2.zero;
                    }
                    else if(transform.position.x < -7)
                    {
                        rb2d.velocity = Vector2.zero;
                    }
                }
                

                
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {

                    if(!makehitexplosion)//피격시 폭발
                    {
                        GameObject expl = Instantiate(hitexplosion);
                        expl.transform.position = pos;
                        if(transform.localScale.x == 1)
                        {
                            expl.transform.localScale = new Vector3(-1f, 1f, 1f);
                        }

                        makehitexplosion = true;
                    }
                    else
                    {
                        transform.position = new Vector2(50, 50);
                    }
                    
                    //에어스트라이크

                    if (airstrike > 0)
                    {
                        delta += Time.deltaTime;

                        if (delta > 0.7f)
                        {
                            Instantiate(airstrikePrefab);
                            delta = 0f;
                            airstrike--;
                        }
                    }
                    else
                    {
                        takedamage = false;
                        hitpos = false;
                        airstrike = 5;
                        animator.ResetTrigger("Hit");
                        animator.SetBool("Hurt", takedamage); 
                        makehitexplosion = false;
                        transform.position = new Vector2(0.5f, -3.07f);

                        if(hp <3)
                        {
                            attackdelay = 3;
                            Instantiate(sweepPrefab);
                        }
                        else if(hp < 5)
                        {
                            attackdelay = 3;
                            Instantiate(groundstrikePrefab);
                        }
                        else
                        {
                            attackdelay = 3;
                        }
                            
                    }
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    rb2d.velocity = Vector2.zero;
                    pos = transform.position;
                    //pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);

                }
            }

            animator.SetBool("WallJump", jumpattack);
            animator.SetBool("WallJumpLand", jumpland);
            animator.SetBool("TakeOutGun", takeoutgun);
            animator.SetBool("PutBackGun", putbackgun);
            animator.SetBool("DashAttack", dashattack);
            animator.SetBool("DashAttackEnd", dashattackend);
            animator.SetBool("Hurt", takedamage);


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CounterBullet" && !takedamage && !roll && !jumpattack &&!isdead)
        {
            //대쉬 공격
            dashattack = false;
            dashattackend = false;
            //페스트 건
            takeoutgun = false;
            putbackgun = false;
            //공중제비
            jumpattack = false;
            jumpland = false;
            jumppos = false;
            airshoot = false;
            rightjump = false;

            int randomsound = Random.Range(3, 6);
            audioSource.clip = bosssound[randomsound];
            audioSource.Play();

            hp--;
            attackdelay = 0f;
            attackbox.SetActive(false);
            if (hp <= 0)
            {
                isdead = true;
                animator.SetTrigger("Die");
                PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);

                if (transform.position.x > playerpos.position.x)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                animator.SetTrigger("Hit");
                takedamage = true;
            }
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Attack" && !takedamage && !roll && !jumpattack && !isdead)
        {

            //대쉬 공격
            dashattack = false;
            dashattackend = false;
            //페스트 건
            takeoutgun = false;
            putbackgun = false;
            //공중제비
            jumpattack = false;
            jumpland = false;
            jumppos = false;
            airshoot = false;
            rightjump = false;

            int randomsound = Random.Range(3, 6);
            audioSource.clip = bosssound[randomsound];
            audioSource.Play();

            hp--;
            GameManager.instance.CameraShake();
            attackdelay = 0f;
            attackbox.SetActive(false);
            if (hp <= 0)
            {
                isdead = true;
                PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);
                animator.SetTrigger("Die");
                if(transform.position.x > playerpos.position.x)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                animator.SetTrigger("Hit");
                takedamage = true;               
            }
            collision.GetComponent<Slash>().slash.enabled = false;
        }
    }
    public IEnumerator AirShot()
    {
        //float angle = 360 / shootbulet;
        int speed = 500;
        for (int i = 0; i < shootbulet; i++)
        {
            GameObject obj = Instantiate(bulletairprefab, transform.position, Quaternion.identity);

            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / shootbulet), speed * Mathf.Sin(Mathf.PI * i * 2 / shootbulet)));

            obj.transform.Rotate(new Vector3(0f, 0f, 360 * i / shootbulet));

               
        }
        yield return new WaitForSeconds(1f);
    }

    //private void AirStrike()
    //{
    //    Debug.Log("함수 들어옴");
    //    float delta=0;
    //    int num = 5;

    //    while(true)
    //    {
    //        Debug.Log("포문 들어옴");
    //        if (num <= 0) break;

    //        delta += Time.deltaTime;
    //        if (delta > 1f)
    //        {
    //            delta = 0f;
    //            //Instantiate(, firepoint.transform.position, Quaternion.identity);
    //            Instantiate(airstrikePrefab);
    //            num--;
    //        }
    //    }
    //}
}
