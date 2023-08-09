using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SnakeTail")
        {
            FindObjectOfType<GameManager>().GenerateFood();
            Destroy(gameObject);
        }
    }
}
