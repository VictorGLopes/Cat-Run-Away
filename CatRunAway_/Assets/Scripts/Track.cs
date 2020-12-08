using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [Header("StaticObstacles")]
    public GameObject[] obstacles;
    public Vector2 numberOfObstacles; //Quantidade de obstaculos que terá

    [Header("Coin")]
    public GameObject coin;
    //public GameObject[] coin;
    public Vector2 numberOfCoins;

    public List<GameObject> newObstacles;
    public List<GameObject> newCoins;



    // Start is called before the first frame update
    void Start()
    {
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y); //coloquei o int entre parentes, pois não aceitava a variavel float, ai converti.
        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);

        for (int i = 0; i < newNumberOfObstacles; i++)
        {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
            newObstacles[i].SetActive(false);
        }

        for (int i = 0; i < newNumberOfCoins; i++)
        {
            newCoins.Add(Instantiate(coin, transform));
            //newCoins.Add(Instantiate(coin[Random.Range(0, coin.Length)], transform));
            newCoins[i].SetActive(false);
        }

        PositionateObstacles();
        PositionateCoins();
    }

    void PositionateObstacles()
    {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            //Saber o tamanho total da pista para colocar aqui como dado
            float posZMin = (300f / newObstacles.Count) + (300f / newObstacles.Count) * i;
            float posZMax = (300f / newObstacles.Count) + (300f / newObstacles.Count) * i + 1;
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin + 15, posZMax));
            newObstacles[i].SetActive(true);
            if(newObstacles[i].GetComponent<ChangeLane>() != null)
            {
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
            }
        }
    }
    void PositionateCoins()
    {
        float minZPos = 10f;
        for (int i = 0; i < newCoins.Count; i++)
        {
            float maxZPos = minZPos + 10f;
            float randomZPos = Random.Range(minZPos + 10f, maxZPos);
            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
            newCoins[i].SetActive(true);
            //newCoins[i].transform.GetChild(coin.Length).transform.gameObject.SetActive(true);
            newCoins[i].GetComponent<ChangeLane>().PositionLane();
            minZPos = randomZPos + 2f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().IncreaseSpeed();
            Invoke("PositionateTrack", 0.2f); 
        }
    }
    void PositionateTrack()
    {
        transform.position = new Vector3(0, 0, transform.position.z + 300f * 2f);
        PositionateObstacles();
        PositionateCoins();
    }
}
