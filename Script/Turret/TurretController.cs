using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject bulletPrefab;
    float shoot = 1f;
    float reroad;
    public bool isdead = false;
    public bool battlemode = false;

    Transform firepoint;
    Transform diepos;
    Transform playerpos;
    GameObject tureeteTop;

    AudioSource turret_death;

    private Animator animator;

    Vector3 dir;
    void Start()
    {
        animator = GetComponent<Animator>();
        firepoint = transform.GetChild(0).GetChild(0);
        diepos = transform.GetChild(1);
        tureeteTop = transform.GetChild(0).gameObject;
        playerpos = GameObject.Find("Player").GetComponent<Transform>();

        turret_death = GetComponent<AudioSource>();

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
        if (isdead) return;
     
        else if(battlemode && !GameManager.instance.playerdie)
        {
            reroad += Time.deltaTime;
            if (reroad > shoot)
            {
                reroad = 0f;

                Instantiate(bulletPrefab, firepoint.transform.position, Quaternion.identity);
            }
        }
        else
        {
            reroad = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CounterBullet" && !isdead)
        {
            turret_death.Play();
            transform.position = diepos.position;
            tureeteTop.SetActive(false);
            animator.SetTrigger("Die");
            Destroy(collision.gameObject);
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);
            isdead = true;            
        }
        else if (collision.tag == "Attack" && !isdead)
        {
            turret_death.Play();
            collision.GetComponent<Slash>().slash.enabled = false;
            transform.position = diepos.position;
            tureeteTop.SetActive(false);
            animator.SetTrigger("Die");
            PlayerPrefs.SetFloat("EnemyNum", GameManager.instance.enemynum--);
            isdead = true;
            GameManager.instance.TimeAction();
        }
    }
}
