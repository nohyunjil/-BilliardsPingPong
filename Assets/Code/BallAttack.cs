using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    public float health;
    public float primaryDamage;
    public float secondaryDamage;

    public bool isAttacker = false; // 공격자인지 아닌지를 결정하는 변수

    private Rigidbody rb;
    private bool hasDealtPrimaryDamage = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        BallAttack otherBall = collision.gameObject.GetComponent<BallAttack>();

        if (otherBall)
        {
            // 만약 이 공이 공격자라면, 다른 공에게 데미지를 주고 자신은 데미지를 받지 않는다.
            if (isAttacker)
            {
                if (!hasDealtPrimaryDamage)
                {
                    otherBall.TakeDamage(primaryDamage);
                    hasDealtPrimaryDamage = true;
                }
                else
                {
                    otherBall.TakeDamage(secondaryDamage);
                }
            }
            // 만약 이 공이 공격자가 아니라면, 다른 공에게서 데미지를 받을 수 있다.
            else if (!otherBall.isAttacker)
            {
                TakeDamage(otherBall.secondaryDamage);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}





