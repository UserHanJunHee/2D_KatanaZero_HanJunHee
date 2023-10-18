using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUnit_Arm : MonoBehaviour
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

            dir = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle , Vector3.forward);
        

    }
}
