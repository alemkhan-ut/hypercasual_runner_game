using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LobbyOptions : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _createRoomInput;
    [SerializeField] private InputField _joinRoomInput;

    public void CreateRoom()
    {
        PhotonNetwork.LeaveRoom();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        string roomName = "ROOM # " + UnityEngine.Random.Range(0, 999) + DateTime.UtcNow.Second.ToString();

        //if (_createRoomInput.text == "" && _createRoomInput == null)
        //{

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Создана комната с названием: " + roomName);
        //}
        //else
        //{
        //    PhotonNetwork.CreateRoom(_createRoomInput.text, roomOptions);
        //    Debug.Log("Создана комната с названием: " + _createRoomInput.text);
        //}
    }

    private void Update()
    {

    }

    public void JoinRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinRoom(_joinRoomInput.text);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.LeaveRoom();

        CreateRoom();

        //if (PhotonNetwork.CountOfRooms > 0)
        //{
        //    PhotonNetwork.JoinRandomRoom();
        //}
        //else
        //{
        //    CreateRoom();
        //}
    }

    public void JoinInOfflineRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinRandomRoom();
    }

    public void SinglePlayMode()
    {
        SceneManager.LoadScene(2);
    }
    public void MultiPlayMode()
    {
        JoinInOfflineRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Присоеден к комнате: " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.Name == "offline room")
        {
            PhotonNetwork.LoadLevel(1);
        }
        else
        {
            PhotonNetwork.LoadLevel(3);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }


}
