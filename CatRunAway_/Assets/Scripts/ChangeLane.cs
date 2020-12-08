using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeLane : MonoBehaviour
{
    public GameObject[] randomObjects;

    //Esse script gambiarra que fiz, para que os objetos fossem randomizados apenas nas lanes pré-determinadas.
   public void PositionLane()
    {
       
        int randomLane = Random.Range(-10, 11);
        transform.position = new Vector3(randomLane, transform.position.y, transform.position.z);
        if(randomLane == 9 || randomLane == 8 || randomLane == 7 || randomLane == 6 || randomLane == 5 || randomLane == 4 || randomLane == 3 || randomLane == 2 ||
            randomLane == 1 || randomLane == -9 || randomLane == -8 || randomLane == -7 || randomLane == -6 || randomLane == -5 || randomLane == -4 || randomLane == -3 ||
            randomLane == -2 || randomLane == -1)
        {
            int[] randomLanes = new int[] { -10, 0, 10 };
            transform.position = new Vector3(randomLanes[Random.Range(0, randomLanes.Length)], transform.position.y, transform.position.z);
        }
    }
}
