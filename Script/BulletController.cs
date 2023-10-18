using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Transform player;
    Rigidbody2D rb2D;

    public AudioClip bulletsound;
    AudioSource audioSource;

    [TextArea]
    public string testString;

    float speed = 30f;
    //public Vector3 dir;
    Vector3 dir;
    bool counter = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        dir = (new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
        rb2D = GetComponent<Rigidbody2D>();
     

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void Update()
    {
        //Debug.Log(dir.magnitude);
        testString = $"Speed : {speed}, Dir : {dir}";
        transform.position += dir * speed * Time.deltaTime;
        //Vector2 dir2 = dir * speed * Time.deltaTime;
        //rb2D.MovePosition(rb2D.position + dir2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Attack" && !counter)
        {
            audioSource.clip = bulletsound;
            audioSource.Play();

            collision.GetComponent<Slash>().slash.enabled = false;
            counter = true;
            transform.localScale = new Vector3(-0.2f, 0.2f, 1f);
            transform.gameObject.tag = "CounterBullet";
            dir = -dir;
        }

        if(collision.tag=="Ground")
        {
            Destroy(this.gameObject);
        }
       
    }
    
}
