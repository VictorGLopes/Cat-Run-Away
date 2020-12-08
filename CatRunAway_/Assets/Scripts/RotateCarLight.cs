using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script utilizado para fazer a Spotlight encima do carro, girar
public class RotateCarLight : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed, 0);
    }
}
