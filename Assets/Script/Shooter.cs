using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    float shoot = 3f;
    float reroad;
    bool isdead = false;

    private Animator animator;

    Vector2 diepos = new Vector2(0, 0.1f);

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isdead)
        {
            reroad += Time.deltaTime;
            if (reroad > shoot)
            {
                reroad = 0f;

                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="CounterBullet"&& !isdead)
        {
            transform.position -= new Vector3(0, 0.3f, 0);
            animator.SetTrigger("Die");
            Destroy(collision.gameObject);
            isdead = true;
        }
        else if(collision.tag == "Attack"&& !isdead)
        {
            transform.position -= new Vector3(0, 0.3f, 0);
            animator.SetTrigger("Die");      
            isdead = true;
        }
    }
}
