using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TransparentObject : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if(!photonView.IsMine)
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && photonView.IsMine)
        {
            Color temp = this.transform.parent.GetComponent<SpriteRenderer>().color;
            temp.a = 0.6f;

            this.transform.parent.GetComponent<SpriteRenderer>().color = temp;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && photonView.IsMine)
        {
            Color temp = this.transform.parent.GetComponent<SpriteRenderer>().color;
            temp.a = 1f;

            this.transform.parent.GetComponent<SpriteRenderer>().color = temp;
        }
    }
}
