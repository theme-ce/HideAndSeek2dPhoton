using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    public Button playButton;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void Connect()
    {
        Debug.Log("Connecting to server.");

        PhotonNetwork.ConnectUsingSettings();

        playButton.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("master", new RoomOptions { MaxPlayers = 8 });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully.", this);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("WaitingRoom");
    }
}
