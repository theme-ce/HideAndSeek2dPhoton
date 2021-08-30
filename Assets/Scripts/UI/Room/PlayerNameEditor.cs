using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNameEditor : MonoBehaviour
{
    [SerializeField] Text playerNameEditText;
    [SerializeField] Text playerNameText;

    private void Update()
    {
        playerNameText.text = PhotonNetwork.NickName;
    }

    public void ChangeName()
    {
        PhotonNetwork.NickName = playerNameEditText.text;
    }
}
