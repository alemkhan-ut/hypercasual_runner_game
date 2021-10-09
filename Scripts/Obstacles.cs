using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] private bool isStart_;
    [SerializeField] private GameObject[] obstacles_;

    private void Start()
    {
        if (!isStart_)
        {

            int obstacleAmount = (int)Random.Range(1f, 3f);

            for (int i = 0; i < obstacleAmount; i++)
            {
                int randomObstacle = (int)Random.Range(1f, 3f);
                obstacles_[randomObstacle].gameObject.SetActive(true);
            }

        }
    }
}
