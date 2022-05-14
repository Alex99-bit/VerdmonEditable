using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador2 : MonoBehaviour
{
    public List<GameObject> spawns = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < spawns.Count; i++)
            {
                if (spawns[i].active)
                {
                    spawns[i].SetActive(false);
                }
                else
                {
                    spawns[i].SetActive(true);
                }
            }
        }
    }
}
