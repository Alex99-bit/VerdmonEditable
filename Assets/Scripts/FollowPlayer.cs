using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform posPlayer;
    AudioSource controlAudio;
    [SerializeField] AudioClip[] audios;
    Vector3 dPlayer;
    bool cercaPlayer, disparar, perseguir;
    Animator animator;
    Rigidbody2D rigidEnemy;
    public GameObject bullet;
    public GameObject canon, pies;
    CharacterStats enemyStats;
    public LayerMask enemyMask, ground;
    static bool expandirCamara;
    public ParticleSystem humo;


    const string IS_ON_THE_GROUND = "isOnTheGround", IS_ALIVE = "isAlive", IS_RUNNING = "isRunning", IS_SHOOTING = "isShooting";


    // Start is called before the first frame update
    void Start()
    {
        rigidEnemy = GetComponent<Rigidbody2D>();
        posPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        enemyStats = new CharacterStats("SoldierEnemy", "Soldado frances");
        StartCoroutine(Shooting());
        controlAudio = GetComponent<AudioSource>();
        perseguir = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            // Sigue la posision del player

            Debug.DrawRay(pies.transform.position, Vector2.down * 0.5f, Color.red);
            Debug.DrawRay(pies.transform.position, Vector2.left * 0.45f, Color.red);
            dPlayer = posPlayer.transform.position - this.transform.position;

            Debug.DrawRay(transform.position, dPlayer, Color.red);
            PlayerCerca();
            
            if (perseguir)
            {
                Detener();
            }
        }
        else if (GameManager.instance.currentGameState == GameState.inicio)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.instance.currentGameState == GameState.inGame)
        {
            if (perseguir)
            {
                SeguirPlayer();
                Jump();
            }
        }
    }

    void SelectAudio(int indice, float volumen)
    {
        controlAudio.PlayOneShot(audios[indice], volumen);
    }

    void SeguirPlayer()
    {
        if (!cercaPlayer)
        {
            // Si no esta demaciado cerca, el enemigo persigue al player
            transform.Translate(dPlayer * Time.deltaTime * enemyStats.getSpeed());
            animator.SetBool(IS_RUNNING, true);
            disparar = false;
        }
        else
        {
            // Si esta lo suficioentemente serca, se detiene y dispara
            transform.Translate(dPlayer * 0);
            animator.SetBool(IS_RUNNING, false);
            disparar = true;
            // Sonido de disparo
            
        }

        // Cambia el sprite del enemigo, dependiendo la posicion del player
        if (posPlayer.position.x > transform.position.x)
        {
            // El player esta a la derecha
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            // El player esta a la izquierda
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void Detener()
    {
        float aux;
        if(Physics2D.Raycast(this.transform.position, dPlayer, 13f, enemyMask))
        {
            //**    True para que le dispare, false para que lo persiga **//

            // Inteligencia basica de los enemigos para colocarse un poco mejor para disparar al player
            if (!(Physics2D.Raycast(pies.transform.position, Vector2.right, 2.5f, ground)) || !(Physics2D.Raycast(pies.transform.position, Vector2.left, 2.5f, ground)))
            {
                if (posPlayer.position.y > this.transform.position.y)
                {
                    aux = 2f;
                    if ((posPlayer.position.y) <= (this.transform.position.y + aux))
                    {
                        cercaPlayer = true;
                    }
                    else
                    {
                        cercaPlayer = false;
                    }
                }
                else if (posPlayer.position.y < this.transform.position.y)
                {
                    aux = -2f;
                    if (posPlayer.position.y >= (this.transform.position.y + aux))
                    {
                        cercaPlayer = true;
                    }
                    else
                    {
                        cercaPlayer = false;
                    }
                }
                else
                {
                    aux = 0;
                    cercaPlayer = true;
                }
            }
            else
            {
                cercaPlayer = false;
            }
            
        }
        else
        {
            cercaPlayer = false;
        }
    }

    IEnumerator Shooting()
    {
        while (GameManager.instance.currentGameState == GameState.inGame)
        {
            // Rutina para que el enemigo dispare
            yield return new WaitForSeconds(0.8f);
            if (disparar)
            {
                Instantiate(bullet, canon.transform);
                humo.Play();
                SelectAudio(0, 0.4f);
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Caja"))
        {
            disparar = true;
        }
        else
        {
            disparar = false;
        }
    }

    void PlayerCerca()
    {
        // Retorna un true o false si esta cerca del jugador o no
        //Debug.DrawRay(transform.position, dPlayer, Color.red);
        if (Physics2D.Raycast(this.transform.position, dPlayer, 20f, enemyMask))
        {
            perseguir = true;
            expandirCamara = true;
        }
        else
        {
            expandirCamara = false;
            perseguir = false;
            animator.SetBool(IS_RUNNING, false);
        }
    }

    void Jump()
    {
        if (IsTochingTheGround())
        {
            // Si hay algo delante, salta
            if (Physics2D.Raycast(pies.transform.position, Vector2.right, 1f, ground) || Physics2D.Raycast(pies.transform.position, Vector2.left, 1f, ground))
            {
                rigidEnemy.AddForce(Vector2.up * enemyStats.getJumpForce(), ForceMode2D.Impulse);
            }
        }
    }

    public bool GetExpandirCamara()
    {
        return expandirCamara;
    }

    bool IsTochingTheGround()
    {
        if (Physics2D.Raycast(pies.transform.position, Vector2.down, 1f, ground))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
