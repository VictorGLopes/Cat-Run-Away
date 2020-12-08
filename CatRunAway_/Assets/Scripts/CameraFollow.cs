using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 offset;

    private float y;

    //Velocidade com que a Camera Segue o Player
    public float speedFollow = 5f;

    //No start seto o Player, que é o que a camera irá seguir e referencio o offset como um transform de posicao
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // offset = transform.position - player.position;
        offset = transform.position;
    }

    // Update is called once per frame
    //Aqui no LateUpdate, pego a posiçao do personagem em relação à camera.
    //E faço os calculos para que ele siga o player, mantendo certa distancia no eixo z
    void LateUpdate()
    {
        Vector3 followPos = player.position + offset;
        RaycastHit hit;
        if (Physics.Raycast(player.position, Vector3.down, out hit, 2.5f))
            y = Mathf.Lerp(y, hit.point.y, Time.deltaTime * speedFollow);
        else y = Mathf.Lerp(y, player.position.y, Time.deltaTime * speedFollow);
        followPos.y = offset.y + y;
        transform.position = followPos;

       //Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);
       //transform.position = newPosition;
    }
    
}
