using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 //Esse é um script basico, são as luzes que seguem o jogador o game todo.
 //Desabilito no Start, para dar um "efeito" de que a policia estava chegando até ele
public class LightFollower : MonoBehaviour
{
    public GameObject[] luzes;
   
    void Awake()
    {
        luzes[0].SetActive(false);
        luzes[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("LigarLuz", 2f);
    }
    void LigarLuz()
    {
        luzes[0].SetActive(true);
        luzes[1].SetActive(true);
    }
}
