using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D enemyRigidbody2D;
    private Vector3 enemyVelocityInFloatArea;
    private Vector3 enemyVelocityInFloatAreaTemp;
    [SerializeField] FloatAreaCheck floatAreaCheck;
    int temp = 0;
    int direction = 1;
    public int HP = 10;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyJump();
        InFloatArea();
        EnemyDeath();
    }
    void EnemyJump()
    {
        temp ++;
        if(temp == 10 && Mathf.Abs(enemyRigidbody2D.velocity.x) < 0.3f)
        {
            direction *= -1;
        }
        if(temp == 500)
        {
            enemyRigidbody2D.velocity = new Vector2(direction * 5.0f, 5.0f);
            temp = 0;
        }
    }
    void InFloatArea()
    {
        if(floatAreaCheck.getisEnter())
        {
            enemyVelocityInFloatAreaTemp = enemyRigidbody2D.velocity;
            enemyVelocityInFloatArea = enemyVelocityInFloatAreaTemp * FloatAreaCheck.speedInFloatArea;
        }
        if(floatAreaCheck.getisInFloatArea())
        {
            enemyRigidbody2D.velocity = enemyVelocityInFloatArea;
        }
        if(floatAreaCheck.getisExit())
        {
            enemyRigidbody2D.velocity = enemyVelocityInFloatAreaTemp;
        }
    }
    void EnemyDeath()
    {
        if(HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
