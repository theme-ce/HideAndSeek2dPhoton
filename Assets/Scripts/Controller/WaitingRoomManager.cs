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

    public GameObject settingPanel;
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public Canvas hostCanvas;
    public GameObject gameController;
    public Dropdown prepareTimeDropdown;
    public Dropdown gameTimeDropdown;
    public Dropdown hiderSpeedDropdown;
    public Dropdown seekerSpeedDropdown;

    private Animator _levelLoaderAnim;
    private int _playeridx;
    private PhotonView _view;

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

        _view = GetComponent<PhotonView>();

        if (GameObject.Find("LevelLoader") != null)
        {
            _levelLoaderAnim = GameObject.Find("LevelLoader").GetComponent<Animator>();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            hostCanvas.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        _playeridx = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

        if (PlayerManager.localPlayer == null)
        {
            int randomPos = Random.Range(0, 7);
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[_playeridx].position, Quaternion.identity);
        }
        else
        {
            SetPlayerPosition();
        }

        if (GameController.Instance == null)
        {
            PhotonNetwork.InstantiateRoomObject(gameController.name, Vector3.zero, Quaternion.identity);
        }

        if (PlayerManager.localPlayer)
        {
            PlayerManager.localPlayer.BecomeHider();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            hostCanvas.gameObject.SetActive(true);
        }
    }

    public void ClickToStartGame()
    {
        _view.RPC("ChangeSceneTo", RpcTarget.All, "GameScene");
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
        Debug.Log(float.Parse(prepareTimeDropdown.options[prepareTimeDropdown.value].text));

        float prepareTime = float.Parse(prepareTimeDropdown.options[prepareTimeDropdown.value].text);
        float gameTime = float.Parse(gameTimeDropdown.options[gameTimeDropdown.value].text);
        float hiderSpeed = float.Parse(hiderSpeedDropdown.options[hiderSpeedDropdown.value].text);
        float seekerSpeed = float.Parse(seekerSpeedDropdown.options[seekerSpeedDropdown.value].text);

        GameController.Instance.ChangeValue(prepareTime, gameTime, hiderSpeed, seekerSpeed);

        settingPanel.SetActive(false);
    }
}
