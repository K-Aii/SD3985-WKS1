using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject ruby = GameObject.Find("Ruby");
        RubyController controller = collision.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.getHealth < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(this.gameObject);
                controller.PlaySound(collectedSound);
            }
            else {
                print("HP is full.");
            }
        }
        else
        {
            print("Controller not detected");
        }
    }
}
