using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _roomName;

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected) return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;

        PhotonNetwork.JoinOrCreateRoom("master" , options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully.", this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully.");
        SceneManager.LoadScene("WaitingRoom");
    }
}
