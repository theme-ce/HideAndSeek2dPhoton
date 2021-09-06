using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform warpTo;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = warpTo.position;

            StartCoroutine(FreezeMove(other.gameObject));
        }
    }

    IEnumerator FreezeMove(GameObject player)
    {
        player.GetComponent<PlayerController>().canMove = false;

        yield return new WaitForSeconds(0.5f);

        player.GetComponent<PlayerController>().canMove = true;
    }
}
