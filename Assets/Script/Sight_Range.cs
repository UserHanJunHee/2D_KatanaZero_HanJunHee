using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight_Range : MonoBehaviour
{
    Vector3 dir; // 플레이어 방향
    Transform playerpos;
    private void Start()
    {
        playerpos = GameObject.Find("Player").GetComponent<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GetComponentInParent<Enemy_RangeUnit>().battlemode&&!GameManager.instance.playerdie)
        {
            if (collision.tag == "Player")
            {
                dir = (playerpos.position - transform.position).normalized;

                int layerMaskToPlayer = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Player"));
                RaycastHit2D playerhit = Physics2D.Raycast(transform.position, dir, 20f, layerMaskToPlayer);
                if (playerhit.collider.tag == "Player")
                {
                    GetComponentInParent<Enemy_RangeUnit>().battlemode = true;
                    GetComponentInParent<Enemy_RangeUnit>().speed = 5f;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GetComponentInParent<Enemy_RangeUnit>().battlemode && !GameManager.instance.playerdie)
        {
            if (collision.tag == "Player")
            {
                dir = (playerpos.position - transform.position).normalized;

                int layerMaskToPlayer = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Player"));
                RaycastHit2D playerhit = Physics2D.Raycast(transform.position, dir, 20f, layerMaskToPlayer);
                if (playerhit.collider.tag == "Player")
                {
                    GetComponentInParent<Enemy_RangeUnit>().battlemode = true;
                    GetComponentInParent<Enemy_RangeUnit>().speed = 5f;
                }
            }
        }
    }
}
