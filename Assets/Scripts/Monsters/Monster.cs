using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField] protected int   m_damage;
    [SerializeField] protected float m_speed;

    protected abstract void Move();

    protected virtual void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.I.TakeDamage(m_damage);
        }
    }
}
