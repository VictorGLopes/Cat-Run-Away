using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Nesse script é onde o player é instanciado em um game object array, ao clicar em Run, no Menu.
public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] players;

    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(players[GameManager.gm.characterIndex], transform.position, Quaternion.identity);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
