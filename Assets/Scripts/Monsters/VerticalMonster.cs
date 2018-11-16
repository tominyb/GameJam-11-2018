using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMonster : Monster
{
    [SerializeField] private float m_patrolDistance;

    private Vector3 m_startingPoint;
    private Vector3 m_direction;

    private void Awake()
    {
        m_startingPoint = transform.position;
        int direction   = Random.Range(0,2);
        m_direction     = direction == 0 ? Vector3.up : Vector3.down;
    }

    protected override void Move()
    {
        if (Vector3.Distance(m_startingPoint, transform.position) > m_patrolDistance)
        {
            m_direction = -m_direction;
        }

        transform.position += m_speed * m_direction * Time.fixedDeltaTime;
    }
}
