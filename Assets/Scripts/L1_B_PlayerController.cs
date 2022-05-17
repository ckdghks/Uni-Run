/* 물체 움직이는 벡터나 포지션 이용 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_B_PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    //public Rigidbody rb;
    // Update is called once per frame

    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;

    public AudioClip deathClip; // 사망시 재생할 오디오 클립

    private int jumpCount = 0; // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태

    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();


    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Input.GetButtonDown("Vertical") && jumpCount < 2)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }

        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        animator.SetBool("Grounded", isGrounded);

    }

    private void Die()
    {
        // 사망 처리
        animator.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        rigid.velocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Enemy" && !isDead)
        {
            Debug.Log("HIT!!");
            GameManager.instance.SubLife(1);   // 빼줄 만큼의 양수 값을 넣어주세요.
            int curLife = GameManager.instance.ReturnLife();
            if (curLife <= 0)
            {
                Die();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 바닥에서 벗어났음을 감지하는 처리
        isGrounded = false;
    }
}
