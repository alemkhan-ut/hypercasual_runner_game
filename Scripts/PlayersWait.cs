using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersWait : MonoBehaviour
{
    [SerializeField] private OrderCard _orderCardToStart;
    private void OnEnable()
    {
        StartCoroutine(WaitPlayerConnection());
    }

    private IEnumerator WaitPlayerConnection()
    {
        StopCoroutine(WaitPlayerConnection());

        yield return new WaitForSeconds(5f);

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            GameOptions.instance.StartLevel();
        }

        StartCoroutine(WaitPlayerConnection());
    }
}
