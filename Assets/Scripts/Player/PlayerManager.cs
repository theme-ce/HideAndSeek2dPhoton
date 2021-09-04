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
    public bool isSpectator;

    private PhotonView _view;
    private Camera _myCamera;
    private AudioListener _myAudio;
    private PlayerController _playerController;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _myCamera = transform.GetChild(2).GetComponent<Camera>();
        _myAudio = transform.GetChild(2).GetComponent<AudioListener>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        playerNameText.text = PhotonNetwork.LocalPlayer.NickName;

        if (_view.IsMine)
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
        if (!_view.IsMine)
        {
            _myCamera.gameObject.SetActive(false);
            _myAudio.gameObject.SetActive(false);
            pointLight.SetActive(false);
            selfLight.SetActive(false);
            return;
        }
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
            _playerController.moveSpeed = GameController.Instance.seekerSpeed;
            _view.RPC("RPC_SetSeekerColor", RpcTarget.All);
        }
    }

    public void BecomeHider()
    {
        isSeeker = false;
        _playerController.moveSpeed = GameController.Instance.hiderSpeed;
        _view.RPC("RPC_SetHiderColor", RpcTarget.All);
    }

    public void SetPosition(Vector3 position)
    {
        _view.RPC("RPC_SetPosition", RpcTarget.All, position);
    }

    [PunRPC]
    void RPC_SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void BecomeSeekerByCatch()
    {
        isSeeker = true;
        _view.RPC("RPC_SetSeekerColor", RpcTarget.All);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isSeeker);
        }
        else if (stream.IsReading)
        {
            isSeeker = (bool)stream.ReceiveNext();
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
