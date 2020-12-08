using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCars : MonoBehaviour
{
    public float delay = 2f;
    float nextTime = 0;
    public float decreaseperdelay = 0.0002f;
    public GameObject carPrefab;

    public Transform[] positionsTransform;

    //Esse Vector3 é onde será randomizada a instanciaçao dos carros(obstaculos), sendo randomizado nos pontos
    //selecionados publicamente.
    Vector3 randomizePosition()
    {
        if(positionsTransform.Length == 1)
        {
            return positionsTransform[0].position;
        }
        else
        {
            if(positionsTransform.Length == 0)
            {
                return Vector2.zero;
            }
            int randomInt = Random.Range(0,positionsTransform.Length);
            return positionsTransform[randomInt].position;
        }
    }
    //O delay será diminuido ao longo do tempo, para que os carros sejam estanciados mais rapidamente
    private void Update()
    {
        if (Time.time >= nextTime)
        {
            InstantiatingCars();

            delay -= decreaseperdelay;
            nextTime = Time.time + delay;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject instance = Instantiate(carPrefab, randomizePosition(), carPrefab.transform.rotation, gameObject.transform);
            Destroy(instance, 0f);
        }
    }
    //Aqui é onde são criados e instanciados os carros.
    void InstantiatingCars()
    {
        GameObject instance = Instantiate(carPrefab, randomizePosition(), carPrefab.transform.rotation, gameObject.transform);
        if (instance == true)
        {
            FindObjectOfType<AudioManager>().Play("CarSound");
        }
        Destroy(instance, 20f);
    }
}
