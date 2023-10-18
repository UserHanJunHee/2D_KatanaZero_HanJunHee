using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    Transform playerpos;
    Vector3  dir;

    private void Start()
    {
        playerpos = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GetComponentInParent<Enemy>().battlemode && !GameManager.instance.playerdie)
        {
            if (collision.tag == "Player")
            {
                dir = (playerpos.position - transform.position).normalized;

                int layerMaskToPlayer = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Player"));
                RaycastHit2D playerhit = Physics2D.Raycast(transform.position, dir, 20f, layerMaskToPlayer);
                if (playerhit.collider.tag == "Player")
                {
                    GetComponentInParent<Enemy>().battlemode = true;
                    GetComponentInParent<Enemy>().speed = 5f;
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GetComponentInParent<Enemy>().battlemode && !GameManager.instance.playerdie)
        {
            if (collision.tag == "Player")

            {
                dir = (playerpos.position - transform.position).normalized;

                int layerMaskToPlayer = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Player"));
                RaycastHit2D playerhit = Physics2D.Raycast(transform.position, dir, 20f, layerMaskToPlayer);
                if (playerhit.collider.tag == "Player")
                {
                    GetComponentInParent<Enemy>().battlemode = true;
                    GetComponentInParent<Enemy>().speed = 5f;
                }
            }
        }
    }
}
