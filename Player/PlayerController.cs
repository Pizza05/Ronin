using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    private float currentSpeed;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveInput;
    private bool isMoving;
    private bool isRunning;

    public PlayerHealth PlayerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (PlayerHealth.isDead == false) 
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            isMoving = moveInput.magnitude > 0;
            isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;

            currentSpeed = isRunning ? runSpeed : walkSpeed;
            rb.velocity = moveInput * currentSpeed;

            // อัปเดต Animator

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isdle", false);
            }
            else
            {
                animator.SetBool("isdle", true);
                animator.SetBool("isWalk", false);
            }

            // เช็คว่าตัวละครกำลังเคลื่อนที่แนวไหน
            if (moveInput.x != 0) // ถ้าเดินแนวนอน
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        
    }

    //public void TakeDamage()
    //{
        //int randomHit = Random.Range(0, 2); // สุ่ม TakeHit หรือ TakeHit2
    //}/

    public void Die()
    {
        rb.velocity = Vector2.zero; // หยุดการเคลื่อนไหวเมื่อ Player ตาย
        this.enabled = false; // ปิดการควบคุมตัวละคร
    }
}
