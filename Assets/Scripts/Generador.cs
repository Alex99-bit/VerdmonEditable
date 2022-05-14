using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    public List<GameObject> spawns = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for(int i = 0; i < spawns.Count; i++)
            {
                spawns[i].SetActive(true);
            }
        }
    }
}
