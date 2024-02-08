using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 5.0f;
    Rigidbody2D rb;
    private float xInput, yInput;

    public int maxHealth = 5;
    int currentHealth = 3;
    public int getHealth { get { return currentHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        print("CurrentHealth = "+currentHealth);
        //currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

    }

    private void FixedUpdate()
    {
        //transform.position += (new Vector3(xInput, yInput, 0)) * speed * Time.deltaTime;

        Vector2 movePos = new Vector2(xInput, yInput) * speed * Time.fixedDeltaTime;
        Vector2 newPos = (Vector2)transform.position + movePos;
        rb.MovePosition(newPos);
    }


    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
