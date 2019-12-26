using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private GameObject ball;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector2(-1,-1), Quaternion.identity);

        if (ball == null)
        {
            ball = PhotonNetwork.Instantiate("Ball", new Vector2(0, 0), Quaternion.identity);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Match", new RoomOptions {MaxPlayers = 10}, TypedLobby.Default);
    }
}