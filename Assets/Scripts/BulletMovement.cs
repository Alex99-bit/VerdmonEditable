using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Rigidbody2D rigidBullet;
    public float speed;
    SpriteRenderer playerSprite;
    int pos;


    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        Dispara();
    }

    void Dispara()
    {
        rigidBullet = GetComponent<Rigidbody2D>();

        if(playerSprite.flipX == false)
        {
            pos = 1;
        }
        else
        {
            pos = -1;
        }
        //print(pos);
        rigidBullet.velocity = new Vector2(pos, 0) * speed;

        Destroy(gameObject, 1); // Destruye la bala despues de 2 seg
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Limit") || collision.gameObject.CompareTag("Caja") || collision.gameObject.CompareTag("Monster"))
        {
            // Diseñar que solo le quite vida al enemigo
            //Destroy(collision.gameObject); // Desaparece el enemigo
            Destroy(gameObject); // Desaparece la bala
        }
    }
}
