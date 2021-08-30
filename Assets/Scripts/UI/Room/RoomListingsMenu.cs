using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListings;

    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform child in _content)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (RoomInfo info in roomList)
        {
            if(info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else
            {
                RoomListing listing = Instantiate(_roomListings, _content);

                if (listing != null)
                {
                    listing.SetRoomInfo(info);
                    _listings.Add(listing);
                } 
                
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_listings);
        }
        else if (stream.IsReading)
        {
            _listings = (List<RoomListing>) stream.ReceiveNext();
        }
    }
}

