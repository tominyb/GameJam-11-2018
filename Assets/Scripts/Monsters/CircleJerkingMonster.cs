using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleJerkingMonster : Monster
{
    [SerializeField] private float m_patrolRadius;

    private Vector3 m_startingPoint;
    private float   m_direction;

    private void Awake()
    {
        Vector3 startingPointDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;
        m_startingPoint                = m_patrolRadius * startingPointDirection;
        m_direction                    = Random.Range(0, 2) * 2 - 1;
    }

    protected override void Move()
    {
        transform.RotateAround(m_startingPoint, Vector3.forward, m_speed * m_direction * Time.fixedDeltaTime);
    }
}
