using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHunterAirStrike : MonoBehaviour
{
    Transform playerpos;
    GameObject laser;
    GameObject headhunter;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerpos = GameObject.Find("Player").GetComponent<Transform>();
        headhunter = GameObject.Find("HeadHunter");
        laser = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();

        transform.position = new Vector2(playerpos.position.x, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (headhunter.GetComponent<HeadHunter>().isdead)
        {
            Destroy(gameObject);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            laser.SetActive(true);
        }
    }
}
