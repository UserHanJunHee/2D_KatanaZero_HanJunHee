using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSight : MonoBehaviour
{

    Vector3 dir;
    Transform playerpos;
    AudioSource turret_act;

    bool actplay = false;

    private void Start()
    {
        playerpos = GameObject.Find("Player").GetComponent<Transform>();
        turret_act = GetComponent<AudioSource>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag =="Player" && !GetComponentInParent<TurretController>().isdead)
        {
         dir = (playerpos.position - transform.position).normalized;

        int layerMaskToPlayer = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D playerhit = Physics2D.Raycast(transform.position, dir, 20f, layerMaskToPlayer);

            if (playerhit.collider.tag != null)
            {
                if (playerhit.collider.tag == "Player" && !GameManager.instance.playerdie)
                {
                    if(!actplay)
                    {
                        turret_act.Play();
                        actplay = true;
                    }
                    
                    GetComponentInParent<TurretController>().battlemode = true;  
                }
                else if (playerhit.collider.tag != "Player" && !GameManager.instance.playerdie)
                {
                    GetComponentInParent<TurretController>().battlemode = false;
                    actplay = false;
                }
                else if (GameManager.instance.playerdie)
                {
                    GetComponentInParent<TurretController>().battlemode = false;
                    actplay = false;
                }
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponentInParent<TurretController>().battlemode = false;
            actplay = false;
        }
    }
}
