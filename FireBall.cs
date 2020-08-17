using Photon.Pun;
using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviourPun
{

    public float speed = 10f;
    public float destroyTime = 2f;
    private bool shootLeft = false;
    public SpriteRenderer sprite;




    public void Start()
    {

    }

    IEnumerator destroyFireBall() 
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("destroy", RpcTarget.AllBuffered);

    }
    [PunRPC]
    public void destroy()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void chageDirection() 
    {
        shootLeft = true;
        sprite.flipX = true;
    }

    void Update()
    {
        if (!shootLeft)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        destroy();
    }


}
