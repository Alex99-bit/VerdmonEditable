using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMonster : MonoBehaviour
{
    Rigidbody2D rigidBullet;
    public float speed;
    //Collider2D collider;

    /* Identifica si el jugador esta a la izq o derecha del enemigo
       true = derecha, false = izquierda */
    Transform playerPos;
    float posx,posy;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Dispara();
    }

    void Dispara()
    {
        rigidBullet = GetComponent<Rigidbody2D>();

        if (playerPos.position.x > transform.position.x)
        {
            posx = 1;
        }
        else if (playerPos.position.x < transform.position.x)
        {
            posx = -1;
        }

        if(playerPos.position.y > transform.position.y)
        {
            posy = 1;
        }
        else
        {
            posy = 0;
        }

        rigidBullet.velocity = new Vector2(posx, posy) * speed;
        Destroy(gameObject, 5);
    }
}
