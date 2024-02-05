using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class boxMove : NetworkBehaviour
{
    Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        m_rigidbody.velocity = Vector3.zero;
    }
}
