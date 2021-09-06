using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPun, IPunObservable
{
    public static PlayerManager localPlayer;

    [SerializeField] GameObject pointLight;
    [SerializeField] GameObject selfLight;
    [SerializeField] Text pingText;
    [SerializeField] Text playerNameText;

    public bool isSeeker;

    private Camera _myCamera;
    private AudioListener _myAudio;
    private PlayerController _playerController;

    private void Awake()
    {
        _myCamera = transform.GetChild(2).GetComponent<Camera>();
        _myAudio = transform.GetChild(2).GetComponent<AudioListener>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.NickName == "")
        {
            PhotonNetwork.LocalPlayer.NickName = "Anonymous" + Random.Range(1, 9999).ToString();
        }

        Debug.Log(PhotonNetwork.LocalPlayer.NickName);

        playerNameText.text = PhotonNetwork.LocalPlayer.NickName;

        if (photonView.IsMine)
        {
            localPlayer = this;
        }

        DontDestroyOnLoad(this);

        SetPlayer();
    }

    private void Update()
    {
        pingText.text = "Ping : " + PhotonNetwork.GetPing().ToString() + " ms";
    }

    //Setup Player
    private void SetPlayer()
    {
        if (!photonView.IsMine)
        {
            _myCamera.gameObject.SetActive(false);
            _myAudio.gameObject.SetActive(false);
            pointLight.SetActive(false);
            selfLight.SetActive(false);
            return;
        }

        if (GameController.Instance != null)
        {
            SetMoveSpeed();
        }
        else
        {
            StartCoroutine(WaitForGameController());
        }
    }

    IEnumerator WaitForGameController()
    {
        yield return new WaitForSeconds(0.5f);

        SetMoveSpeed();
    }

    //Set Color for Seeker
    [PunRPC]
    private void RPC_SetSeekerColor()
    {
        transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(0.93f, 0.38f, 0.38f);
    }

    [PunRPC]
    private void RPC_SetHiderColor()
    {
        transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(0.26f, 0.72f, 1f);
    }

    //Recieve "seekerNumber" From GameController to Set Player to Become Seeker
    public void BecomeSeeker(int seekerNumber)
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[seekerNumber])
        {
            isSeeker = true;
            SetMoveSpeed();
            photonView.RPC("RPC_SetSeekerColor", RpcTarget.All);
        }
    }

    public void BecomeHider()
    {
        isSeeker = false;
        SetMoveSpeed();
        photonView.RPC("RPC_SetHiderColor", RpcTarget.All);
    }

    public void SetMoveSpeed()
    {
        if (isSeeker)
        {
            _playerController.moveSpeed = GameController.Instance.seekerSpeed;
        }
        else
        {
            _playerController.moveSpeed = GameController.Instance.hiderSpeed;
        }
    }

    public void SetPosition(Vector3 position)
    {
        photonView.RPC("RPC_SetPosition", RpcTarget.All, position);
    }

    [PunRPC]
    void RPC_SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void BecomeSeekerByCatch()
    {
        isSeeker = true;
        photonView.RPC("RPC_SetSeekerColor", RpcTarget.All);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isSeeker);
            stream.SendNext(_playerController.moveSpeed);
        }
        else if (stream.IsReading)
        {
            isSeeker = (bool)stream.ReceiveNext();
            _playerController.moveSpeed = (float)stream.ReceiveNext();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<PlayerManager>().isSeeker && isSeeker)
            {
                collision.gameObject.GetComponent<PlayerManager>().BecomeSeekerByCatch();
            }
        }
    }
}
