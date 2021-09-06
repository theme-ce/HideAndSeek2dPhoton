using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public static GameSceneManager instance;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Text timeText;
    [SerializeField] Text gameOverText;

    private int _whichPlayerIsSeeker;
    private Animator _levelLoaderAnim;
    private int _playerIdx;
    private float _prepareTime = 16f;
    private float _gameTime = 180f;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        if (GameObject.Find("LevelLoader") != null) { _levelLoaderAnim = GameObject.Find("LevelLoader").GetComponent<Animator>(); }
        if (GameObject.Find("StartGameButton")) { GameObject.Find("StartGameButton").SetActive(false); }
    }

    private void Start()
    {
        _playerIdx = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

        SetPlayerPosition();

        _prepareTime = GameController.Instance.prepareTime;
        _gameTime = GameController.Instance.gameTime * 60f;

        if (PhotonNetwork.IsMasterClient) { StartCoroutine(DelayToPickSeeker(_prepareTime)); }
    }

    private void Update()
    {
        TimeCheck();
        PlayerCheck();
    }

    void TimeCheck()
    {
        if (_prepareTime > 0)
        {
            _prepareTime -= Time.deltaTime;
            if (Mathf.Floor(_prepareTime % 60) > 9)
            {
                timeText.text = Mathf.Floor(_prepareTime / 60).ToString() + " : " + Mathf.Floor(_prepareTime % 60).ToString();
            }
            else
            {
                timeText.text = Mathf.Floor(_prepareTime / 60).ToString() + " : " + "0" + Mathf.Floor(_prepareTime % 60).ToString();
            }
        }
        else
        {
            if (_gameTime > 0)
            {
                _gameTime -= Time.deltaTime;
                if (Mathf.Floor(_gameTime % 60) > 9)
                {
                    timeText.text = Mathf.Floor(_gameTime / 60).ToString() + " : " + Mathf.Floor(_gameTime % 60).ToString();
                }
                else
                {
                    timeText.text = Mathf.Floor(_gameTime / 60).ToString() + " : " + "0" + Mathf.Floor(_gameTime % 60).ToString();
                }
                timeText.color = Color.red;
            }
            else
            {
                timeText.text = "Times Up!";

                HiderWin();
            }
        }
    }

    void PlayerCheck()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        int seekerCount = 0;

        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerManager>().isSeeker)
            {
                seekerCount += 1;
            }
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount <= seekerCount)
        {
            SeekerWin();
        }
    }

    void SetPlayerPosition()
    {
        Vector3 position = spawnPoints[_playerIdx].position;

        PlayerManager.localPlayer.SetPosition(position);
    }

    void PickSeeker()
    {
        _whichPlayerIsSeeker = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        photonView.RPC("RPC_SyncSeeker", RpcTarget.All, _whichPlayerIsSeeker);
    }

    [PunRPC]
    void RPC_SyncSeeker(int playerNumber)
    {
        _whichPlayerIsSeeker = playerNumber;
        PlayerManager.localPlayer.BecomeSeeker(_whichPlayerIsSeeker);
    }

    void HiderWin()
    {
        timeText.gameObject.SetActive(false);
        gameOverText.text = "Hider Win!";

        ChangeSceneTo("WaitingRoom");
    }

    void SeekerWin()
    {
        timeText.gameObject.SetActive(false);
        gameOverText.text = "Seeker Win!";

        ChangeSceneTo("WaitingRoom");
    }

    public void ChangeSceneTo(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        _levelLoaderAnim.SetTrigger("GameEnd");

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator DelayToPickSeeker(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);

        PickSeeker();
    }
}
