using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public float speed = 5f;
    public Joystick joystick;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    public bool CanMove= true;
    private bool isUsed=false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (!CanMove)

        {
            if (!isUsed)
            {
                isUsed = true;
                movement = Vector2.zero;
                animator.SetInteger("direction", 0);
            }
            return;
            
        }
        
        if(isUsed) isUsed = false;
        if (new Vector2(horizontal, vertical).magnitude > 0.01f)
        {
            // Choose dominant axis
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                movement = new Vector2(Mathf.Sign(horizontal), 0f); // strictly horizontal

                animator.SetInteger("direction", 1); // horizontal uses left animation

                spriteRenderer.flipX = horizontal > 0; // flip when moving right
            }
            else
            {
                movement = new Vector2(0f, Mathf.Sign(vertical)); // strictly vertical

                spriteRenderer.flipX = false; // reset flipX when vertical

                if (vertical > 0)
                    animator.SetInteger("direction", 4); // Up
                else
                    animator.SetInteger("direction", 3); // Down
            }
        }
        else
        {
            movement = Vector2.zero;
            animator.SetInteger("direction", 0); // Idle
        }
    }

    void FixedUpdate()
    {
        if (!CanMove)

        {

            return;

        }
        ;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }



    public void MoveToHouse(Transform[] pointers, WaysToGo[] ways,HouseTrigger house)
    {
        movement = Vector2.zero;

        StartCoroutine(MovingHome(pointers, ways,house));
    }

    private IEnumerator MovingHome(Transform[] pointers, WaysToGo[] ways, HouseTrigger h)
    {
        //throw new NotImplementedException();
        CanMove = false;


        for (int i = 0; i < pointers.Length; i++)
        {
            switch (ways[i])
            {
                case WaysToGo.Up:
                    animator.SetInteger("direction", 4); // Up


                    break;
                case WaysToGo.Down:
                    animator.SetInteger("direction", 3); // Down

                    break;
                case WaysToGo.Left:
                    animator.SetInteger("direction", 1); // horizontal uses left animation
                    spriteRenderer.flipX = false; // flip when moving right

                    break;
                case WaysToGo.Right:
                    animator.SetInteger("direction", 1); // horizontal uses left animation

                    spriteRenderer.flipX = true; // flip when moving right
                    break;
            }

            while ((pointers[i].position - transform.position).magnitude > 0.05)
            {
                Vector2 pos = pointers[i].position;
                //print(pos);
                print(pos + "Pos");
                print(pointers[i].position + "localpos");
                print(rb.position + pos * speed * Time.fixedDeltaTime + "rb.position + pos * speed * Time.fixedDeltaTime");
                rb.MovePosition(rb.position + (pos - rb.position).normalized * speed * Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();

            }
        }
        h.Finish();
        CanMove = true;

    }
}
public enum WaysToGo
{
    Up, Down, Left, Right,
}