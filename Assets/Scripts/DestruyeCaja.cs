using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruyeCaja : MonoBehaviour
{
    /*  En esta clase que va ligada a la caja esta la programacion que si recibe cierta cantidad de daño se puede romper,
        y soltar aleatoreamente un power up, puntos de score o un item    */
    int vida, randomIdx;
    public ParticleSystem astillas;
    public List<GameObject> spawns = new List<GameObject>();
    PlayerMecha points;


    // Start is called before the first frame update
    void Start()
    {
        vida = 100;
        points = new PlayerMecha();
        randomIdx = Random.Range(0, spawns.Count);
        if (points.GetPoint() >= 10 && spawns.Count <= 2)
        {
            spawns.Remove(spawns[2]);
        }
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Aux de momento no se utiliza
        int aux = Random.Range(0,2);

        if(collision.gameObject.CompareTag("BulletPlayer") || collision.gameObject.CompareTag("BulletEnemy"))
        {
            // En caso de que el objeto que colisione sea una bala, la caja recibira daño
            //Instantiate(astillas,transform.position,Quaternion.identity);
            astillas.Play();
            vida -= 35;
        }

        if (vida <= 0) // Caja destruida
        {
            astillas.Play();
            aux = 1;
            if(aux == 1)
            {
                Instantiate(spawns[randomIdx], transform.position, Quaternion.identity);
            }
            
            //Invoke("Destruye", 0.5f);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int aux = Random.Range(0, 2);

        if (collision.collider.CompareTag("Monster") || collision.collider.CompareTag("BulletEnemy"))
        {
            astillas.Play();
            vida -= 35;
        }

        if (vida <= 0) // Caja destruida
        {
            astillas.Play();
            aux = 1;
            if (aux == 1)
            {
                Instantiate(spawns[randomIdx], transform.position, Quaternion.identity);
            }

            //Invoke("Destruye", 0.5f);
            Destroy(this.gameObject);
        }
    }
}
