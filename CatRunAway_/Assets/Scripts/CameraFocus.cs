using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

//É basicamente o mesmo scrípt do CameraFollow, unica diferença no void Start 
//em que acrescentei um multiplier de distancia por float, no offset
public class CameraFocus : MonoBehaviour
{
    private Transform player;
    private Vector3 offset;

    private float y;
    public float speedFollow = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // offset = transform.position - player.position;
        offset = transform.position * 5f;
    }

    // Update is called once per frame
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
