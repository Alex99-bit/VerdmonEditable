using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{

    Rigidbody2D rigidBullet;
    public float speed;

    /* Identifica si el jugador esta a la izq o derecha del enemigo
       true = derecha, false = izquierda */
    Transform playerPos;
    int pos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Dispara();
    }

    void Dispara()
    {
        rigidBullet = GetComponent<Rigidbody2D>();

        if(playerPos.position.x > transform.position.x)
        {
            pos = 1;
        }
        else if(playerPos.position.x < transform.position.x)
        {
            pos = -1;
        }

        rigidBullet.velocity = new Vector2(pos, 0) * speed;
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Limit") || collision.gameObject.CompareTag("Caja"))
        {
            Destroy(gameObject);
        }
    }
}
