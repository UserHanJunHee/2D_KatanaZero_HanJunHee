using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RangeUnit : MonoBehaviour
{

    public bool battlemode = false;
    public bool patrol = false;
    public bool patrollookleft = false;
    public bool attack = false;
    bool standing = false;
    bool isdead = false;
    public float speed = 1f;

    public AudioClip[] rangeunitsound;
    AudioSource audioSource;

    //float attackdelay = 0.5f;//후딜레이
    //float attackdelystart = 0f;

    float reroad;//사격 딜레이.
    float shoot = 0.8f;

    public GameObject bulletPrefab;

    GameObject arm;
    Transform playerpos;
    Transform firepoint;


    Animator animator;

    Vector3 dir;

    Vector3 standingpos;

    
    // Start is called before the first frame update.

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerpos = GameObject.Find("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        firepoint = transform.GetChild(0).GetChild(0);
        arm = transform.GetChild(0).gameObject;


        standingpos = transform.position;

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
        if (GameManager.instance.playerdie && !standing)
        {
            if (!patrol)
            {
                attack = false;
                arm.SetActive(false);
                standing = true;
                animator.SetBool("Standing", standing);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, standingpos.y - 0.2f, transform.position.y);
                if (!patrollookleft)//오른쪽
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(1f, -2f, 0f), 1.8f, layerMask);

                    if (hit.collider == null)
                    {
                        patrollookleft = true;
                    }


                }
                else if (patrollookleft)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(-1f, -2f, 0f), 1.8f, layerMask);

                    if (hit.collider == null)
                    {
                        patrollookleft = false;
                    }

                }
            }
        }

        else if (isdead) return;

        else if (!GameManager.instance.playerdie && !isdead)
        {    
            if (battlemode)
            {
                patrol = false;
                dir = (playerpos.position - transform.position).normalized;


                Debug.DrawRay(transform.position, dir * 30, Color.red);

                int layerMaskToPlayer = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Player"));
                RaycastHit2D playerhit = Physics2D.Raycast(transform.position, dir, 15f, layerMaskToPlayer);
                //사이 벽 체크
                if (playerhit.collider.tag != null)
                {
                    if (playerhit.collider.tag == "Player")
                    {
                        transform.position = new Vector3(transform.position.x, standingpos.y, transform.position.y);
                        attack = true;
                        arm.SetActive(true);
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x, standingpos.y - 0.2f, transform.position.y);
                        attack = false;
                        reroad = 0f;
                        arm.SetActive(false);
                    }
                }

                //공격
                if (attack)
                {
                    if(playerpos.position.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        arm.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        arm.transform.localScale = new Vector3(-1f, -1f, 1f);
                    }
                    reroad += Time.deltaTime;

                    if(reroad > shoot)
                    {
                        audioSource.clip = rangeunitsound[4];
                        audioSource.Play();
                        reroad = 0f;

                        Instantiate(bulletPrefab, firepoint.transform.position, Quaternion.identity);
                        //GameObject go = Instantiate(bulletPrefab, firepoint.transform.position, Quaternion.identity);
                        //go.GetComponent<BulletController>().dir =  (playerpos.position - transform.position).normalized;
                        attack = false;
                    }
                    
                }

                else if (Mathf.Abs(transform.position.x - playerpos.position.x) < 0.8f)
                {
                    standing = true;
                    
                    if (playerpos.position.x - transform.position.x > 0)
                    {
                        transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                    }     
                }

                //추적
                else if (playerpos.position.x > transform.position.x && !attack)
                {
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(1, -2, 0), 1.8f, layerMask);

                    transform.localScale = new Vector3(1f, 1f, 1f);
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag == "Ground")
                        {
                            standing = false;
                            transform.Translate(Vector3.right * speed * Time.deltaTime);
                        }
                    }
                    else
                    {
                        standing = true;
                    }
                }
                else if (playerpos.position.x < transform.position.x && !attack)
                {
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(-1, -2, 0), 1.8f, layerMask);


                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag == "Ground")
                        {
                            standing = false;
                            transform.Translate(Vector3.left * speed * Time.deltaTime);

                        }
                    }
                    else
                    {
                        standing = true;
                    }
                }
            }
            //정찰
            else if (patrol && !battlemode)
            {
                transform.position = new Vector3(transform.position.x, standingpos.y - 0.2f, transform.position.y);
                if (!patrollookleft)//오른쪽
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(1f, -2f, 0f), 1.8f, layerMask);

                    if (hit.collider == null)
                    {
                        patrollookleft = true;
                    }


                }
                else if (patrollookleft)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(-1f, -2f, 0f), 1.8f, layerMask);

                    if (hit.collider == null)
                    {
                        patrollookleft = false;
                    }

                }
            }
        }

        animator.SetBool("Battlemode", battlemode);
        animator.SetBool("Patrol", patrol);
        animator.SetBool("Attack", attack);
        animator.SetBool("Standing", standing);

        Debug.DrawRay(transform.position, new Vector3(-1, -2, 0) * 0.5f, Color.red);
        Debug.DrawRay(transform.position, new Vector3(1, -2, 0) * 0.5f, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CounterBullet" && !isdead)
        {
            audioSource.clip = rangeunitsound[0];
            audioSource.Play();

            animator.SetTrigger("Die");
            transform.position = new Vector3(transform.position.x, standingpos.y - 0.4f, transform.position.y);
            Destroy(collision.gameObject);
            arm.SetActive(false);
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);
            patrol = false;
            isdead = true;
        }
        else if (collision.tag == "Attack" && !isdead)
        {
            int randomsound = Random.Range(1, 4);
            audioSource.clip = rangeunitsound[randomsound];
            audioSource.Play();
            collision.GetComponent<Slash>().slash.enabled = false;
            animator.SetTrigger("Die");
            transform.position = new Vector3(transform.position.x, standingpos.y - 0.4f, transform.position.y);
            arm.SetActive(false);
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);
            patrol = false;
            isdead = true;
            GameManager.instance.TimeAction();
        }
    }
}
