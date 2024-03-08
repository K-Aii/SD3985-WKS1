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

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Animator anim;
    Vector2 lookDirection = new Vector2(1,0);

    public GameObject projectilePrefab;

    AudioSource audioSource;
    public AudioClip launchSound;
    public AudioClip hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        print("CurrentHealth = "+currentHealth);
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(xInput, yInput);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);
        anim.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null) {
                //Debug.Log("Raycast hit: "+hit.collider.gameObject);
                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

    }

    private void FixedUpdate()
    {
        //transform.position += (new Vector3(xInput, yInput, 0)) * speed * Time.deltaTime;

        Vector2 movePos = new Vector2(xInput, yInput) * speed * Time.fixedDeltaTime;
        Vector2 newPos = (Vector2)transform.position + movePos;
        rb.MovePosition(newPos);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hurtSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        anim.SetTrigger("Launch");
        PlaySound(launchSound);
    }

    
}
