using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyMonster : Monster
{
    [SerializeField] private Projectile m_projectilePrefab;
    [SerializeField] private float      m_projectileSpeed;
    [SerializeField] private float      m_projectileTimeInterval;

    private MeshRenderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<MeshRenderer>();
        StartCoroutine(IShootProjectiles());
    }

    protected override void Move()
    {
        transform.Rotate(Vector3.forward, m_speed * Time.fixedDeltaTime);
    }

    private void ShootProjectile()
    {
        Projectile projectile         = Instantiate(m_projectilePrefab);
        projectile.transform.position = transform.position + transform.right * m_renderer.bounds.extents.x * 2.0f;
        projectile.Speed              = transform.right * m_projectileSpeed;
    }

    private IEnumerator IShootProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_projectileTimeInterval);
            ShootProjectile();
        }
    }
}
