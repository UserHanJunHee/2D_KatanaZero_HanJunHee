using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShot : MonoBehaviour
{
    public GameObject bullet;
    Rigidbody2D rb2D;

    bool counter = false;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack" && !counter)
        {
            counter = true;
            collision.GetComponent<Slash>().slash.enabled = false;
            transform.localScale = new Vector3(-0.2f, 0.2f, 1f);
            rb2D.velocity *= -1;
            transform.gameObject.tag = "CounterBullet";
        }

        if (collision.tag == "Ground")
        {
            Destroy(this.gameObject);
        }

    }

}
