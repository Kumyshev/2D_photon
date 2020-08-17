using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    public PhotonView pv;

    public float pl_Health = 100f;

    public float pl_MoveSpeed = 10f;
    private Vector2 pl_Vector;

    public Rigidbody2D rb;

    [HideInInspector]
    public bool IsGrounded;

    public SpriteRenderer sprite;
    private GameObject fireBall;
    public GameObject ballPref;
    public Transform ballSpawnR;
    public Transform ballSpawnL;

    private RightButton rightButton;
    private LeftButton leftButton;
    private JumpButton jumpButton;
    private ShootButton shootButton;
    private AttackButton attackButton;
    
    private gameManager GameManager;


    private bool right;
    private bool left;

    public Transform AttackPointR;
    public Transform AttackPointL;
    public float attackRange = -0.25f;
    public LayerMask layerMask;

    private Animator m_Animation;

    private Collider2D[] hitEnemies;

    private HealthBar health;

    private HealthManager healthManager;


    private void Start()
    {
        GameManager = FindObjectOfType<gameManager>();

        healthManager = FindObjectOfType<HealthManager>();

        if (photonView.IsMine)
        {
            rb = GetComponent<Rigidbody2D>();
            
            rightButton = FindObjectOfType<RightButton>();
            leftButton = FindObjectOfType<LeftButton>();
            jumpButton = FindObjectOfType<JumpButton>();
            shootButton = FindObjectOfType<ShootButton>();
            attackButton = FindObjectOfType<AttackButton>();

            


            m_Animation = GetComponent<Animator>();


            PlayerUIHealth();
        }
        if (!photonView.IsMine)
        {
            health = FindObjectOfType<HealthBar>();
            
        }
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInputs();
        }
        else
        {
            SmoothMovement();
            enemyHealthStatus();
        }    

            

        Death();
    }

    private void SmoothMovement() 
    {
        transform.position = Vector3.Lerp(transform.position, pl_Vector, Time.deltaTime * 10);
    }
    private void ProcessInputs() 
    {

        m_Animation.SetBool("isJumping", false);
        m_Animation.SetBool("isCasting", false);

        
        if (GameManager.DirectionChange) 
        {
            left = true;
            right = false;
        }
        GameManager.DirectionChange = false;
        
        if (rightButton.Pressed)
        {
            right = true;
            left = false;
           
            rb.velocity = pl_MoveSpeed * transform.right;

        }
        if (leftButton.Pressed)
        {
            left = true;
            right = false;
            rb.velocity = pl_MoveSpeed * transform.right * -1;
        }

        m_Animation.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));

  
        if (right) 
        {
            sprite.flipX = false;
            pv.RPC("OnDirectionChange_R", RpcTarget.Others);
        }
        if (left)
        {
            sprite.flipX = true;
            pv.RPC("OnDirectionChange_L", RpcTarget.Others);
        }
        if (jumpButton.Pressed && IsGrounded) 
        {
            Jump();
        }
        if (shootButton.Pressed) 
        {
            Shoot();
        }
        if (attackButton.Pressed)
        {
            Attack();
        }
        if (!attackButton.Pressed)
        {
            m_Animation.SetBool("isAttacking", false);
        }

        HealthStatus();
    }


    [PunRPC]
    void OnDirectionChange_L()
    {
        sprite.flipX = true;
    }

    [PunRPC]
    void OnDirectionChange_R()
    {
        sprite.flipX = false;
    }

    public void Jump()
    {
        rb.velocity = new Vector3(0, 30, 0);
        m_Animation.SetBool("isJumping", true);
    }

    public void Shoot() 
    {

        if (!sprite.flipX)
        {
            fireBall = PhotonNetwork.Instantiate(ballPref.name, ballSpawnR.position, Quaternion.identity);
        }
        if (sprite.flipX)
        {
            fireBall = PhotonNetwork.Instantiate(ballPref.name, ballSpawnL.position, Quaternion.identity);
            fireBall.GetComponent<PhotonView>().RPC("chageDirection", RpcTarget.AllBuffered);
        }

        m_Animation.SetBool("isCasting", true);
        shootButton.Pressed = false;
    }

    private void OnDrawGizmos()
    {
        if (AttackPointR == null) 
        {
            return;
        }
        Gizmos.DrawWireSphere(AttackPointL.position, attackRange);
    }
    public void Attack() 
    {
        if (!sprite.flipX)
        {
            hitEnemies = Physics2D.OverlapCircleAll(AttackPointR.position, attackRange, layerMask);
        }
        if (sprite.flipX)
        {
            hitEnemies = Physics2D.OverlapCircleAll(AttackPointL.position, attackRange, layerMask);
        }
        m_Animation.SetBool("isAttacking", true);

        foreach (Collider2D enemy in hitEnemies)
        {
            m_Animation.SetTrigger("Attacking");
            //enemy.GetComponent<MyPlayer>().MeleeDamage(20);
            enemy.GetComponent<PhotonView>().RPC("MeleeDamage", RpcTarget.AllBuffered);
            attackButton.Pressed = false;
        }

    }

    [PunRPC]
    public void MeleeDamage() 
    {
        pl_Health -= 10;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FireBall")
        {
            pl_Health -= 10;
        }

        if (photonView.IsMine) {
            if (collision.gameObject.tag == "Ground")
            {
                IsGrounded = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Ground")
            {
                IsGrounded = false;
            }
        }

    }


    public void PlayerUIHealth() 
    {
        foreach (var player in GameManager.playerInfo)
        {
            if (PhotonNetwork.NickName == player.NickName)
            {
                health = player.health;
            }
        }
    }




    public void HealthStatus() 
    {
        health.Health(pl_Health);
    }

    public void enemyHealthStatus()
    {
        foreach (var player in GameManager.playerInfo)
        {
            if (healthManager.healthBar1.tag != player.health.tag)
            {
                health = healthManager.healthBar1;
            }
            if (healthManager.healthBar2.tag != player.health.tag)
            {
                health = healthManager.healthBar2;
            }
        }

        health.Health(pl_Health);
    }


    public void Death()
    {
        if (pl_Health <= 0)
        {
            m_Animation.SetTrigger("isDie");
            Destroy(this.gameObject);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(pl_Health);

        }
        else if (stream.IsReading) 
        {
            pl_Vector = (Vector3)stream.ReceiveNext();
            pl_Health = (float)stream.ReceiveNext();
        }
    }
}
