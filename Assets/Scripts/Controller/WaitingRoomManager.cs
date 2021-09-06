using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public static WaitingRoomManager instance;

    [SerializeField] GameObject settingPanel;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Canvas hostCanvas;
    [SerializeField] GameObject gameController;
    [SerializeField] Dropdown prepareTimeDropdown;
    [SerializeField] Dropdown gameTimeDropdown;
    [SerializeField] Dropdown hiderSpeedDropdown;
    [SerializeField] Dropdown seekerSpeedDropdown;

    private Animator _levelLoaderAnim;
    private int _playeridx;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        if (GameObject.Find("LevelLoader") != null) { _levelLoaderAnim = GameObject.Find("LevelLoader").GetComponent<Animator>(); }

        if (PhotonNetwork.IsMasterClient)
        {
            hostCanvas.gameObject.SetActive(true);
        }

        if (GameController.Instance == null)
        {
            PhotonNetwork.InstantiateRoomObject(gameController.name, Vector3.zero, Quaternion.identity);
        }
    }

    private void Start()
    {
        _playeridx = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

        if (PlayerManager.localPlayer == null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[_playeridx].position, Quaternion.identity);
        }
        else
        {
            SetPlayerPosition();
        }


        if (PlayerManager.localPlayer)
        {
            PlayerManager.localPlayer.BecomeHider();
        }

    }

    public void ClickToStartGame()
    {
        photonView.RPC("ChangeSceneTo", RpcTarget.All, "GameScene");
    }

    void SetPlayerPosition()
    {
        Vector3 position = spawnPoints[_playeridx].position;

        PlayerManager.localPlayer.SetPosition(position);
    }

    [PunRPC]
    public void ChangeSceneTo(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        _levelLoaderAnim.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettingPanel()
    {
        prepareTimeDropdown.value = prepareTimeDropdown.options.FindIndex(option => option.text == GameController.Instance.prepareTime.ToString());
        gameTimeDropdown.value = gameTimeDropdown.options.FindIndex(option => option.text == GameController.Instance.gameTime.ToString());
        hiderSpeedDropdown.value = hiderSpeedDropdown.options.FindIndex(option => option.text == GameController.Instance.hiderSpeed.ToString());
        seekerSpeedDropdown.value = seekerSpeedDropdown.options.FindIndex(option => option.text == GameController.Instance.seekerSpeed.ToString());
        if (!settingPanel.activeSelf)
        {
            settingPanel.SetActive(true);
        }
        else
        {
            settingPanel.SetActive(false);
        }
    }

    public void ApplyConfig()
    {
        float prepareTime = float.Parse(prepareTimeDropdown.options[prepareTimeDropdown.value].text);
        float gameTime = float.Parse(gameTimeDropdown.options[gameTimeDropdown.value].text);
        float hiderSpeed = float.Parse(hiderSpeedDropdown.options[hiderSpeedDropdown.value].text);
        float seekerSpeed = float.Parse(seekerSpeedDropdown.options[seekerSpeedDropdown.value].text);

        GameController.Instance.ChangeValue(prepareTime, gameTime, hiderSpeed, seekerSpeed);

        settingPanel.SetActive(false);

        photonView.RPC("RPC_SyncSpeed", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SyncSpeed()
    {
        PlayerManager.localPlayer.SetMoveSpeed();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            hostCanvas.gameObject.SetActive(true);
        }
    }
}
