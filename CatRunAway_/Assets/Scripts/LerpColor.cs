using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public float speed = 1f;
    public Material startColor;
    public Material endColor;
    public bool repeatable = false;
    public float startTime;
    Renderer rend;

    // Start is called before the first frame update

    //Dou o componente do Renderer para o rend e atribuo o material que será renderizado em game, 
    //pego o rend e atribuo a ele a cor inicial.
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = startColor;
    }

    // Update is called once per frame

    //pego o rend e atribuo a ele o Lerp entre as cores, uma interpolaçao, entre a inicial e a final
    void Update()
    {
      if(!repeatable)
        {
            float t = (Time.time - startTime) * speed;
            rend.material.Lerp(startColor, endColor, t);
        }
        else
        {
            float t = Mathf.PingPong(Time.time, startTime)/ startTime;
            rend.material.Lerp(startColor, endColor, t);
        }
    }
}
