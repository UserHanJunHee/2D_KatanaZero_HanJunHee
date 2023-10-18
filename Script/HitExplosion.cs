using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitExplosion : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject explosion2;
    GameObject explosion3;
    AudioSource audioSource;

    float delta = 0f;
    int explosionnum = 2;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        explosion2 = transform.GetChild(1).gameObject;
        explosion3 = transform.GetChild(2).gameObject;

        //transform.position = headhunterpos.position;
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if(delta >0.25f)
        {
            delta = 0;
            switch (explosionnum)
            {
                case 0:
                    if (audioSource.isPlaying)
                        Destroy(gameObject);
                    break;
                case 1:
                    audioSource.Play();
                    explosion3.SetActive(true);
                    break;
                case 2:
                    audioSource.Play();
                    explosion2.SetActive(true);
                    break;
            }
            explosionnum--;
        }
    }
}
