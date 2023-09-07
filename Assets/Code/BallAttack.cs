using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    public float health;
    public float primaryDamage;
    public float secondaryDamage;

    public bool isAttacker = false; // ���������� �ƴ����� �����ϴ� ����

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
            // ���� �� ���� �����ڶ��, �ٸ� ������ �������� �ְ� �ڽ��� �������� ���� �ʴ´�.
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
            // ���� �� ���� �����ڰ� �ƴ϶��, �ٸ� �����Լ� �������� ���� �� �ִ�.
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





