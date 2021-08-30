using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Color temp = this.transform.parent.GetComponent<SpriteRenderer>().color;
            temp.a = 0.6f;

            this.transform.parent.GetComponent<SpriteRenderer>().color = temp;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Color temp = this.transform.parent.GetComponent<SpriteRenderer>().color;
            temp.a = 1f;

            this.transform.parent.GetComponent<SpriteRenderer>().color = temp;
        }
    }
}
