using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks
{
    public static GameController Instance;

    public float prepareTime = 5f;
    public float gameTime = 3f;
    public float hiderSpeed = 5f;
    public float seekerSpeed = 5f;

    private PhotonView _view;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        DontDestroyOnLoad(this);

        _view = GetComponent<PhotonView>();
    }

    public void ChangeValue(float prepareTime, float gameTime, float hiderSpeed, float seekerSpeed)
    {
        _view.RPC("RPC_SyncValue", RpcTarget.All, prepareTime, gameTime, hiderSpeed, seekerSpeed);
    }

    [PunRPC]
    void RPC_SyncValue(float prepareTime, float gameTime, float hiderSpeed, float seekerSpeed)
    {
        this.prepareTime = prepareTime;
        this.gameTime = gameTime;
        this.hiderSpeed = hiderSpeed;
        this.seekerSpeed = seekerSpeed;
    }
}
