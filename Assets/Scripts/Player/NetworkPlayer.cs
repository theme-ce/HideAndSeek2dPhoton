using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    protected PlayerManager player;
    protected Vector3 remotePlayerPosition;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            return;
        }

        var lagDistance = remotePlayerPosition - transform.position;

        if(lagDistance.magnitude > 5f)
        {
            transform.position = remotePlayerPosition;
            lagDistance = Vector3.zero;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            remotePlayerPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
