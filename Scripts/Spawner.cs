using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _MP_PlayerPrefab;
    [SerializeField] private GameObject _SP_PlayerPrefab;
    [SerializeField] private SpawnPosition[] _spawnPositions;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            SpawnOnlinePlayer();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SpawnLocalPlayer();
        }
    }

    public void SpawnLocalPlayer()
    {
        SpawnPosition randomSpawn = _spawnPositions[Random.Range(0, _spawnPositions.Length)];
        GameObject player = Instantiate(_SP_PlayerPrefab, randomSpawn.transform.position, Quaternion.identity);
        player.GetComponent<PlayerMover>().LineNumber = randomSpawn.LineNumber;
    }

    public void SpawnOnlinePlayer()
    {
        SpawnPosition randomSpawn = _spawnPositions[Random.Range(0, _spawnPositions.Length)];
        GameObject player = PhotonNetwork.Instantiate(_MP_PlayerPrefab.name, randomSpawn.transform.position, Quaternion.identity);
        player.GetComponent<PlayerMover>().LineNumber = randomSpawn.LineNumber;
    }
}
