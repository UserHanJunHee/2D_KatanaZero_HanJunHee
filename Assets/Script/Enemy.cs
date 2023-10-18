using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool battlemode = false;
    public bool patrol = false;
    public bool patrollookleft = false;
    bool attack = false;
    bool standing = false;
    bool isdead = false;
    public float speed = 1f;
    float attackdelay = 0.5f;
    float attackdelystart = 0f;

    public AudioClip[] enemysound;
    AudioSource audioSource;
    bool attacksound = false;

    Transform playerpos;
    Animator animator;
    GameObject attackbox;

    // Start is called before the first frame update
    void Start()
    {
        playerpos = GameObject.Find("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        attackbox = transform.GetChild(1).gameObject;

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
        Debug.DrawRay(transform.position, new Vector3(-1, -2, 0) * 0.5f, Color.red);
        Debug.DrawRay(transform.position, new Vector3(1, -2, 0) * 0.5f, Color.red);

        if(GameManager.instance.playerdie && !standing)
        {           
            if (!patrol)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    attack = false;
                    standing = true;
                }
                animator.SetBool("Standing", standing);
            }
            else//만약 패트롤이 켜져있으면 움직인다.
            {
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
        else if(!GameManager.instance.playerdie)
        {
            if(battlemode)
            {
                patrol = false;
                //공격
                if (attack)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    {
                        attackbox.SetActive(false);
                        attackdelystart += Time.deltaTime;
                        if (attackdelystart > attackdelay)
                        {
                            attacksound = false;
                            attack = false;
                            attackdelystart = 0f;
                        }
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                    {
                        if (!attacksound)
                        {
                            attacksound = true;
                            audioSource.clip = enemysound[4];
                            audioSource.Play();
                        }
                        attackbox.SetActive(true);
                    }
                }
                else if (Mathf.Abs(transform.position.x - playerpos.position.x) < 0.8f)
                {
                    standing = true;
                    if (transform.position.y - playerpos.position.y  >= -0.1f && transform.position.y - playerpos.position.y  < 1f)
                    {
                        standing = false;
                        attack = true;
                        if (playerpos.position.x - transform.position.x > 0)
                        {
                            transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        else
                        {
                            transform.localScale = new Vector3(-1f, 1f, 1f);
                        }
                        
                    }
                }
                //추적
                else if (playerpos.position.x > transform.position.x && !attack)
                {
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(1, -2, 0),1.8f,layerMask);

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
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(-1, -2, 0),1.8f,layerMask);


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
            else if(patrol && !battlemode)
            {
                if (!patrollookleft)//오른쪽
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    int layerMask = 1 << LayerMask.NameToLayer("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(1f, -2f, 0f), 1.8f,layerMask);

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
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(-1f, -2f, 0f), 1.8f,layerMask);

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CounterBullet" && !isdead)
        {
            audioSource.clip = enemysound[0];
            audioSource.Play();

            animator.SetTrigger("Die");
            Destroy(collision.gameObject);
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);
            patrol = false;
            isdead = true;
        }
        else if (collision.tag == "Attack" && !isdead)
        {
            int randomsound = Random.Range(1, 4);
            audioSource.clip = enemysound[randomsound];
            audioSource.Play();

            collision.GetComponent<Slash>().slash.enabled = false;
            animator.SetTrigger("Die");
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);
            patrol = false;
            isdead = true;

            GameManager.instance.CameraShake();

        }
    }
}
