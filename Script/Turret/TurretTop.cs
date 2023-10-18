using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTop : MonoBehaviour
{
    Transform player;
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<TurretController>().battlemode)
        {
            dir = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if(player.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(angle , Vector3.forward);
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
