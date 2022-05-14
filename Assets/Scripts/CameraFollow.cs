using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0.2f, 0.0f, -10f);
    public float dampingTime = 0.3f;
    public Vector3 velocity = Vector3.zero;
    FollowPlayer camara;
    Camera propiedades;
    int indice;
    private AudioSource controlAudio;
    [SerializeField] private AudioClip[] audios;

    private void Awake()
    {
        // Los frames en los que ira la camara
        Application.targetFrameRate = 60;
        camara = new FollowPlayer();
        propiedades = GetComponentInParent<Camera>();
        controlAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            MoveCamera(true);
            //propiedades.sensorSize = 
            // Capta si hay enemigos cerca, en caso de que si, expande la camara para dar
            // más oportunidad de ver los enemigos (entra en modo ataque)
            if (camara.GetExpandirCamara())
            {
                // Expandir la camara
                //propiedades.sensorSize
            }
            else
            {
                // Regresarla a la posicion normal
            }
            SelectAudio();
        }
        else
        {
            controlAudio.Stop();
        }
    }

    public void SelectAudio()
    {
        if (!controlAudio.isPlaying)
        {
            indice = Random.Range(0, audios.Length);
            controlAudio.PlayOneShot(audios[indice], 0.65f);
        }
    }

    public void ResetCameraPosition()
    {
        MoveCamera(false);
    }

    void MoveCamera(bool smooth)
    {
        Vector3 destination = new Vector3(target.position.x - offset.x,target.position.y - offset.y, offset.z);
        if (smooth)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampingTime);
        }
        else
        {
            this.transform.position = destination;
        }
    }
}
