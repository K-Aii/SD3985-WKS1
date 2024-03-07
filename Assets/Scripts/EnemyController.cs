using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    float timer;
    int direction = 1;

    Rigidbody2D rb;

    Animator anim;

    public bool broken = true;
    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!broken) { return;  }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            speed = -speed;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken) { return; }

        Vector2 position = rb.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed;
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed;
            anim.SetFloat("MoveX", direction);
            anim.SetFloat("MoveY", 0);
        }


        rb.MovePosition(position);
    }

    public void Fix()
    {
        broken = false;
        rb.simulated = false;
        anim.SetTrigger("Fixed");
        smokeEffect.Stop();

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}
