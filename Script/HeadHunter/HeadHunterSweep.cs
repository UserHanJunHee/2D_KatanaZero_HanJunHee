using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHunterSweep : MonoBehaviour
{
    int random;

    Animator animator;
    GameObject sweeplaserattackbox;
    void Start()
    {
        random = Random.Range(0, 2);
        animator = GetComponent<Animator>();

        sweeplaserattackbox = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (random == 0)//왼쪽
        {
            transform.position = new Vector2(6.5f, 3f);
            transform.localScale = new Vector3(1f, 1f, 1f);
            sweeplaserattackbox.transform.rotation *= Quaternion.Euler(0, 0, -132 * Time.deltaTime);
        }
        else//오른쪽
        {
            transform.position = new Vector2(-6.5f, 3f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            sweeplaserattackbox.transform.rotation *= Quaternion.Euler(0, 0, 132 * Time.deltaTime);
        }

        if ((animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f))
        {
            Destroy(gameObject);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            sweeplaserattackbox.SetActive(false);
        }

    }
}
