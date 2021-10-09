using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.NetworkingClient.EnableLobbyStatistics = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene(2); // Connect to Single Play
    }
}
