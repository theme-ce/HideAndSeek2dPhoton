using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Connecting to server.");

        PhotonNetwork.NickName = GameSettings.Instance.Nickname;
        PhotonNetwork.GameVersion = GameSettings.Instance.GameVersion;
        PhotonNetwork.SendRate = 10;
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.MaximumTransferUnit = 520;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server for reason " + cause.ToString());
    }
}
