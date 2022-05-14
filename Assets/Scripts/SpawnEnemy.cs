using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Esta clase da la funcion de spawnear enemigos y moverse de un lado a otro (el spawn)
    public GameObject enemy;
    public float seg;
    Rigidbody2D rigidSpawn;
    bool cambioLado;
    public float speed;
    int i;
    public int numero_enemigos;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        rigidSpawn = this.GetComponent<Rigidbody2D>();
        cambioLado = false;
        StartCoroutine(spawnEnemys());
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            if (!cambioLado)
            {
                rigidSpawn.velocity = new Vector2(1, 0) * speed;
            }
            else
            {
                rigidSpawn.velocity = new Vector2(-1, 0) * speed;
            }
        }
        else if(GameManager.instance.currentGameState == GameState.inicio)
        {
            i = 0;
        }
    }

    IEnumerator spawnEnemys()
    {
        do
        {
            // Subrutina que spawnea los enemigos 
            yield return new WaitForSeconds(seg);

            if (GameManager.instance.currentGameState == GameState.inGame)
            {
                Instantiate(enemy, this.transform);
                i++;
            }
        }while (i<numero_enemigos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limit"))
        {
            if (!cambioLado)
            {
                cambioLado = true;
            }
            else
            {
                cambioLado = false;
            }
        }
    }
}
