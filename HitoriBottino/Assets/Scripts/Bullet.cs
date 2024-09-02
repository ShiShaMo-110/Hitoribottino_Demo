using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    enum BulletDirection{
        BULLET_UP,
        BULLET_DOWN,
        BULLET_RIGHT,
        BULLET_LEFT
    }
    [SerializeField] BulletDirection thisBulletDirection;
    float bulletSpeed = 10.0f;
    Vector3 bulletVelocity;
    SpriteRenderer bulletRenderer;
    EnemyMove enemyMove;
    
    Rigidbody2D bulletRigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRenderer = this.GetComponent<SpriteRenderer>();
        bulletRigidbody2D = this.GetComponent<Rigidbody2D>();
        switch(thisBulletDirection)
        {
            case BulletDirection.BULLET_UP:
                bulletVelocity = new Vector3(0, bulletSpeed, 0);
                break;
            case BulletDirection.BULLET_DOWN:
                bulletVelocity = new Vector3(0, -bulletSpeed, 0);
                break;
            case BulletDirection.BULLET_LEFT:
                bulletVelocity = new Vector3(-bulletSpeed, 0, 0);
                break;
            case BulletDirection.BULLET_RIGHT:
                bulletVelocity = new Vector3(bulletSpeed, 0, 0);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidbody2D.velocity = bulletVelocity;
        if(!bulletRenderer.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            enemyMove = other.GetComponent<EnemyMove>();
            enemyMove.HP --;
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Floor")
        {
            Destroy(this.gameObject);
        }
    }
}
