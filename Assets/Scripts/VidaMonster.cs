using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaMonster : MonoBehaviour
{
    public GameObject vidaMonster;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            vidaMonster.SetActive(true);
        }
    }
}
