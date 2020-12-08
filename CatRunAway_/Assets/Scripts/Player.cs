using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Movimentacao")]
    public float speed;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeight;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public float maxJumpLength = 30f;
    public float slideLength;

    private bool sliding = false;
    private float slideStart;
    private Vector3 boxColliderSize;

    [Header("Vida")]
    public int maxLife = 3;
    private int currentLife;

    private UIManager uiManager;

    private Animator anim;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private int currentLane = 10;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;

    //Teste
    public float jumpForce;

    //Mobile Controls
    private bool isSwipping = false;
    private Vector2 startingTouch;

    [HideInInspector]
    public float score;

    [HideInInspector]
    public int coins;

    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        currentLife = maxLife;
        //blinkingValue = Shader.PropertyToID("UnlitBlinking");
        uiManager = FindObjectOfType<UIManager>();
        GameManager.gm.StartMissions();
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;

        Invoke("StartRun", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;

        score += Time.deltaTime * speed;
        uiManager.UpdateScore((int)score);

        //Inputs para PC
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-10);
           
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(10);

        }
        else if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }
        //Inputs para mobile
        if(Input.touchCount == 1)
        {
            if(isSwipping)
            {
                Vector2 diff = Input.GetTouch(0).position - startingTouch;
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);
                if(diff.magnitude >0.01f)
                {
                    if(Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        if (diff.y < 0)
                        {
                            Slide();
                        }
                        else
                        {
                            Jump();
                        }
                       
                    }
                    else
                    {
                        if(diff.x < 0)
                        {
                            ChangeLane(-10);
                        }
                        else
                        {
                            ChangeLane(10);
                        }
                    }

                    isSwipping = false;
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startingTouch = Input.GetTouch(0).position;
                isSwipping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isSwipping = false;
            }
        }

        if(jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if(ratio >= 1)
            {
                jumping = false;
                anim.SetBool("Jumping", false);
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }
        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed = 0.5f);

        if(sliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLength;
            if(ratio >= 1f)
            {
                sliding = false;
                anim.SetBool("Slide", false);
                boxCollider.size = boxColliderSize;
            }
        }
        if(speed ==0)
        {
            verticalTargetPosition.y = jumpHeight = 0;
        }
    }
    private void FixedUpdate()
    {
        Invoke("CanMove", 3f);
    }
    //Ativa a movimentação do personagem após certo tempo, para sair de Idle
    void StartRun()
    {
        anim.Play("Walk");
        speed = minSpeed;
        canMove = true;
    }

    //Habilita o personagem a trocar de lane, por meio dos Inputs
    public void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane > 20)
            return;
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 10), 0, 0);
        if (speed == 0)
        {
            currentLane = targetLane * 0;
            verticalTargetPosition = new Vector3(0,0,0);
        }
        if(jumping && speed == 0)
        {
            currentLane = targetLane * 0;
            verticalTargetPosition = new Vector3(0, jumpLength, 0);
        }
    }
    //Classe onde é feita o pulo
    void Jump()
    {
        if(!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLength);
            anim.SetBool("Jumping", true);
            jumping = true;
        }
    }
    //Classe onde é feita o deslizar/slide
    void Slide()
    {
        if(!jumping && !sliding)
        {
            slideStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / slideLength);
            anim.SetBool("Slide", true);
            Vector3 newSize = boxCollider.size;
            newSize.y = newSize.y / 4;
            boxCollider.size = newSize;
            sliding = true;
            if(speed == 0)
            {
                anim.SetBool("Slide", false);
                sliding = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coins++;
            uiManager.UpdateCoins(coins);
            //abaixo eu estou desativando o objeto filho, que é o que possui o collider.
            //antigo de uma moeda apenas:
            other.transform.parent.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("CoinHit");
            //mais de uma moeda:
            //other.gameObject.SetActive(false);
        }

        //Se o jogador se colidir com um objeto com tag Obstacle, ele morre.
        if (other.CompareTag("Obstacle"))
        {
            canMove = false;
            currentLife--;
            anim.SetTrigger("Hit");
            speed = 0;
            if(currentLife <= 0)
            {
                speed = 0;

                if (PlayerPrefs.GetFloat("Highscore") < score)
                {
                    PlayerPrefs.SetFloat("Highscore", score);
                }
                anim.SetBool("DeathFallBack", true);
                FindObjectOfType<AudioManager>().Play("ConeHit");
                FindObjectOfType<AudioManager>().Play("CatHit");
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 3f);
            }
        }
        //Se o jogador se colidir com um objeto com tag BusHit, ele morre.
        if (other.CompareTag("BusHit"))
        {
            canMove = false;
            currentLife--;
            anim.SetTrigger("Hit");
            speed = 0;
            if (currentLife <= 0)
            {
                speed = 0;

                if (PlayerPrefs.GetFloat("Highscore") < score)
                {
                    PlayerPrefs.SetFloat("Highscore", score);
                }
                anim.SetBool("DeathFallBack", true);
                FindObjectOfType<AudioManager>().Play("BusHit");
                FindObjectOfType<AudioManager>().Play("CatHit");
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 3f);
            }
        }
        if (other.CompareTag("PlacaHit"))
        {
            canMove = false;
            currentLife--;
            anim.SetTrigger("Hit");
            speed = 0;
            if (currentLife <= 0)
            {
                speed = 0;

                if (PlayerPrefs.GetFloat("Highscore") < score)
                {
                    PlayerPrefs.SetFloat("Highscore", score);
                }
                anim.SetBool("DeathFallBack", true);
                FindObjectOfType<AudioManager>().Play("PlacaHit");
                FindObjectOfType<AudioManager>().Play("CatHit");
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 3f);
            }
        }
        //Se o jogador se colidir com um objeto com tag Car, ele morre.
        if (other.CompareTag("HitCar"))
        {
            canMove = false;
            currentLife--;
            anim.SetTrigger("CarHit");
            speed = 0;
            if (currentLife <= 0)
            {
                speed = 0;

                if(PlayerPrefs.GetFloat("Highscore") < score)
                {
                    PlayerPrefs.SetFloat("Highscore", score);
                }
                anim.SetBool("DeathByCar", true);
                FindObjectOfType<AudioManager>().Play("HitCar");
                FindObjectOfType<AudioManager>().Play("CatAtropelado");                
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 3f);
            }
        }
    }
    //Habilita o deslocamento em Z do personagem
    void CanMove()
    {
        canMove = true;
        rb.velocity = Vector3.forward * speed;
    }
   //Ativa o Menu, tanto para atribuir as moedas coletadas quando para encerrar a corrida, ao morrer.
    void CallMenu()
    {
        GameManager.gm.coins += coins;
        GameManager.gm.EndRun();
    }
    //Aumenta a velocidade do personagem ao passar pelo trigger, onde é atribuida essa classe
    public void IncreaseSpeed()
    {
        speed *= 1.02f;
        if (speed >= maxSpeed)
            speed = maxSpeed;
        jumpLength *= 1.02f;
        if (jumpLength >= maxJumpLength)
            jumpLength = maxJumpLength;
    }
}
