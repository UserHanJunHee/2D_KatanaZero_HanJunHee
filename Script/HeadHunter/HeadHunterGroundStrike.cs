using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHunterGroundStrike : MonoBehaviour
{
    GameObject laser;
    GameObject headhunter;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        laser = transform.GetChild(0).gameObject;
        headhunter = GameObject.Find("HeadHunter");
        animator = GetComponent<Animator>();


        int random = Random.Range(0, 2);

        if(random == 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.position = new Vector2(-8, -3.24f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.position = new Vector2(8, -3.24f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(headhunter.GetComponent<HeadHunter>().isdead)
        {
            Destroy(gameObject);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
        {
            laser.SetActive(true);
        }
    }
}
