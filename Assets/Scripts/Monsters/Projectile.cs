using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int   m_damage;
    [SerializeField] private float m_fadeTime;

    private Vector3 m_speed;

    public Vector3 Speed
    {
        set
        {
            m_speed = value;
            StartCoroutine(IShoot());
        }
    }

    private void FixedUpdate()
    {
        transform.position += m_speed * Time.fixedDeltaTime;
    }

    private IEnumerator IShoot()
    {
        yield return new WaitForSeconds(m_fadeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") // Kill player here.
        {
            
        }
    }
}
