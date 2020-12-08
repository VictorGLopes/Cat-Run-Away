using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 0;
    }

    // Update is called once per frame
    //Faço com que a o carro se mova na direçao contraria da qual o Player vem.
    void Update()
    {
        transform.Translate(0, 0, -moveSpeed);
    }
    //Caso o personagem entre no colisor do campo de visão do carro, sua velocidade será aumentada.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            moveSpeed = 1f;
        }
    }
    //Caso o carro saia de perto do campo de visão do player, sua velocidade será 0
    private void OnTriggerExit(Collider other)
    {
        moveSpeed = 0;
    }
}
