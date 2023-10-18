using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb2D;//이동하기위해 리치드바디
    private Animator animator;//애니메이터 관리용으로 불러오기

    public GameObject slash;//공격(왼클릭)할때 활성화 비활성화로 공격판정 만들기 위함


    public AudioClip[] zerosound;
    //0 = 사망
    //1 = 구르기
    //2 = 착지
    //3 = 점프
    //4 = 달리기

    AudioSource audioSource;

    float speed = 15f;//이동속도
    float jumpForce = 900f;//점프 힘
    float rollingForce = 3f;//구르는 힘

    public int jumpCount = 0;//점프는 1회만 가능하다
    private bool isRunning = false;//애니메이션 달리기 땅위에있는지 공격
    private bool runningsound = false;
    public bool isGrounded = true;//테스트 용도로 퍼블릭
    private bool isAttack = false;
    private bool isdead = false;


    //벽타기 탭

    public Transform wallChk;
    public float wallchkDistance;
    public LayerMask w_Layer;
    public bool isWall;
    public float slidingSpeed;
    public float wallJumpPower;
    public bool isWallJump;


    //잔상
    public Ghost ghost;

      




    private bool deadpos = false;//죽을때 애니메이션 크기가 안맞아서 위치 조정용
    private bool mujuk = false;
    private bool isRolling = false;//구르기

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isdead)
        {
            slash.SetActive(false);
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f && !deadpos)
            {
                if (isGrounded)
                {
                    rb2D.isKinematic = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                    deadpos = true;
                    return;
                }
            }
        }
        else if (GameManager.instance.enemynum != 0)
        {
            if (GameManager.instance.timer.fillAmount <= 0 && !isdead && GameManager.instance.scenenum != 1)
            {

                audioSource.clip = zerosound[0];
                if (audioSource.loop)
                {
                    audioSource.loop = false;
                }
                audioSource.Play();
                isdead = true;
                GameManager.instance.PlayerDie();
                animator.SetTrigger("Die");
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                if (!mujuk)
                    mujuk = true;
                else
                    mujuk = false;
            }



            //벽타기
            if(!isAttack)
            isWall = Physics2D.Raycast(wallChk.position, Vector2.left* transform.localScale.x, wallchkDistance, w_Layer);

            //animator.SetBool("Wall", isWall);
            //Gizmos.DrawRay(wallChk.position, Vector2.right * transform.localScale.x * wallchkDistance);


            //점프
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1 && !isRolling && !isWall)//점프는 1회 가능하고 , 구르는 도중이 아닐때 가능
            {
                audioSource.clip = zerosound[3];//점프 사운드
                audioSource.loop = false;
                audioSource.Play();

                rb2D.velocity = Vector2.zero;

                rb2D.AddForce(new Vector2(0, jumpForce));//위로 점프
            }
            else if (Input.GetKeyUp(KeyCode.Space) && rb2D.velocity.y > 0 && !isWall)//누르고 있는 시간에 따라 조금 더 높이 뛰게 짧게누르면 더 짧게 뛰게 해주는 부분
            {
                rb2D.velocity *= 0.5f;
            }

            //이동
            float xInput = Input.GetAxisRaw("Horizontal");//가로 값을 가져온다

            if (xInput != 0 && !isRolling)//ad로 이동중일때 또한 구르지 않을떄
            {
                if(!runningsound && isGrounded)
                {
                    runningsound = true;
                    audioSource.clip = zerosound[4];
                    audioSource.loop = true;
                    audioSource.Play();
                }
                

                //if(!audioSource.clip == zerosound[4])//달리기
                //{
                //    audioSource.clip = zerosound[4];
                //    audioSource.loop = true;
                //    audioSource.Play();
                //}
                //else
                //{
                //    audioSource.loop = true;
                //    audioSource.Play();
                //}
                //audioSource.loop = true;

                if (!isAttack)//공격중일땐 공격 방향을 끝까지 바라보고 있게 하기 위해서 공격죽일땐 안돌아 가게 막아줌
                {
                    transform.localScale = new Vector3(xInput, 1, 1);//좌우 바꿔줌
                }
                isRunning = true;
            }
            else//가만히 있을때 달리기를 꺼줌
            {
                audioSource.loop = false;
                runningsound = false;
                isRunning = false;
            }

            if (!isRolling && !isWallJump)//안구르고 있으면 이동함 가만히 있으면 xinput이 0이라 가만히 있는다.
            {
                float xSpeed = xInput * speed;
                Vector2 newVelocity = new Vector2(xSpeed, rb2D.velocity.y);
                rb2D.velocity = newVelocity;
            }

            //슬로우 모션중 잔상 메이커
            if(GameManager.instance.slowmode)
            {
                ghost.makeGhost = true;
            }
            else
            {
                if(!isRolling && !isWallJump)
                ghost.makeGhost = false;
            }



            //구르기


            if (isRolling)//구르기가 활성화되면 구릅니다.
            {
                ghost.makeGhost = true;
                float pos = transform.localScale.x;
                rb2D.AddForce(new Vector2(rollingForce * pos, rb2D.velocity.y));
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)//애니메이션을 마치면 구르기가 비활성화 됩니다
                {
                    ghost.makeGhost = false;
                    isRolling = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift) && isRunning && isGrounded)//가만히있을땐 못구른다 땅위에 있고 이동중일때 구른다
            {
                if (isAttack)
                {
                    if(xInput != 0)
                    {
                        transform.localScale = new Vector3(xInput, 1, 1);
                    }
                    slash.SetActive(false);
                    isAttack = false;
                }
                audioSource.clip = zerosound[1];

                if(audioSource.loop)
                    audioSource.loop = false;

                audioSource.Play();
                isRolling = true;
            }


            //공격

            if (isAttack)
            {
                slash.SetActive(true);//공격이 활성화되면 비활성화 되있는 베는 이팩트를 활성화 해줌
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)//애니메이션을 끝까지 재생하면
                {
                    slash.SetActive(false);//비활성화
                    isAttack = false;
                }

            }
            else if (Input.GetMouseButtonDown(0) && !isRolling)//구르고있지 않을때만 공격 가능합니다
            {
                isAttack = true;
            }

            //애니메이션 논리값 설정
            
        }
        else
        {
            rb2D.velocity = new Vector2(0,rb2D.velocity.y);
            audioSource.loop = false;
            if(isAttack)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)//애니메이션을 끝까지 재생하면
                {
                    slash.SetActive(false);//비활성화
                    isAttack = false;
                }
            }
            isRunning = false;
        }



        //벽 점프
        if (isWall)
        {
            isWallJump = false;
            ghost.makeGhost = false;
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * slidingSpeed);

            //Input.GetKeyDown(KeyCode.Space)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true;
                ghost.makeGhost = true;
                Invoke("FreezeX", 0.3f);    
            }
            if (isWallJump)
            {
                rb2D.velocity = new Vector2(-transform.localScale.x * wallJumpPower, 0.9f * wallJumpPower);
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
        }



        animator.SetBool("Wall", isWall);
        animator.SetBool("Run", isRunning);
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Roll", isRolling);
        animator.SetBool("Attack", isAttack);

    }


    //private void FixedUpdate()
    //{
    //    if(isWall)
    //    {
    //        isWallJump = false;
    //        rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * slidingSpeed);

    //        //Input.GetKeyDown(KeyCode.Space)
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            isWallJump = true;
    //            Invoke("FreezeX", 0.3f);
    //            rb2D.velocity = new Vector2(-transform.localScale.x * wallJumpPower, 0.9f * wallJumpPower);
    //            transform.localScale = new Vector3(transform.localScale.x*-1, 1, 1);
    //        }
    //    }
    //}

    void FreezeX()
    {
        isWallJump = false;
        ghost.makeGhost = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)//땅위에 닿았을때 점프 할 수 있도록 해줍니다.
    {
        if(collision.collider.tag =="Ground" && !isdead)
        {
            
            if (collision.contacts[0].normal.y > 0.7f)//윗부분으로 닿을때
            {
                audioSource.clip = zerosound[2];
                if(audioSource.loop)
                    audioSource.loop = false;
                audioSource.Play();

                runningsound = false;
                isGrounded = true;
                jumpCount = 0;
            }
            //else if(collision.contacts[0].normal.x > 0.7f || collision.contacts[0].normal.x > -0.7f)
            //{
            //    jumpCount = 0;
            //}
        }
        //if (collision.collider.tag == "Wall")
        //{
        //    isWall = true;
        //}
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && !isdead)
        {
            if (collision.contacts[0].normal.y > 0.7f)//윗부분으로 닿을때
            {
                //runningsound = false;
                isGrounded = true;
                jumpCount = 0;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" &&!isdead)
        {
            if(audioSource.clip != zerosound[3] && !isRolling)//점프 사운드
            {
                audioSource.clip = zerosound[3];
                audioSource.loop = false;
                audioSource.Play();
            }
            

            jumpCount++;
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//총알에 닿았을때 혹은 공격에 닿았을때 죽는거 추가할 부분
    {
        if (!isdead && !isRolling && !mujuk)
        {
            if (collision.tag == "Bullet")
            {
                Destroy(collision.gameObject);
                rb2D.velocity = Vector2.zero;
                slash.SetActive(false);
                isdead = true;
                GameManager.instance.PlayerDie();
                audioSource.clip = zerosound[0];
                if (audioSource.loop)
                {
                    audioSource.loop = false;
                }
                audioSource.Play();
                animator.SetTrigger("Die");

            }
            if (collision.tag == "EnemyAttack")
            {
                rb2D.velocity = Vector2.zero;
                slash.SetActive(false);
                isdead = true;
                GameManager.instance.PlayerDie();
                audioSource.clip = zerosound[0];
                if (audioSource.loop)
                {
                    audioSource.loop = false;
                }
                audioSource.Play();
                animator.SetTrigger("Die");
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)//레이저용도 지속되는동안에도 맞게끔
    {
        if (!isdead && !isRolling && !mujuk)
        {
            if (collision.tag == "EnemyAttack")
            {

                audioSource.Play();
                rb2D.velocity = Vector2.zero;
                slash.SetActive(false);
                isdead = true;
                GameManager.instance.PlayerDie();
                audioSource.clip = zerosound[0];
                if (audioSource.loop)
                {
                    audioSource.loop = false;
                }
                audioSource.Play();
                animator.SetTrigger("Die");
            }
        }
    }
}