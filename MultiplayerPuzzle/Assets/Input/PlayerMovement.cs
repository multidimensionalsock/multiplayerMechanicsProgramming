using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerMovement : NetworkBehaviour
{
    PlayerInput m_input;
    Vector2 m_moveDirection;
    //[SerializeField] AnimationCurve m_forceAdjust;
    [SerializeField] float m_moveForce;
    [SerializeField] float maxSpeed;
    Rigidbody2D m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_input = GetComponent<PlayerInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_input.currentActionMap.FindAction("Move").performed += MoveStart;
        m_input.currentActionMap.FindAction("Move").canceled += MoveEnd;
    }

    void MoveStart(InputAction.CallbackContext context)
    {
        if (!IsOwner) { return;  }
        m_moveDirection = context.ReadValue<Vector2>();
        StartCoroutine(Move());
    }

    void MoveEnd(InputAction.CallbackContext context)
    {
        if (!IsOwner) { return; }
        m_moveDirection = Vector2.zero;
    }

    IEnumerator Move()
    {
        
        while(m_moveDirection != Vector2.zero)
        {
            Debug.Log(m_moveDirection);
            m_rigidbody.AddForce( m_moveForce * Time.fixedDeltaTime * m_moveDirection);
            m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, maxSpeed);
            //set max force to move force and it keeps about the same
            //set to zero vector at end of this if you want to just stop. or could set velocity lower?like 0.1?
            yield return new WaitForFixedUpdate();
        }
        m_rigidbody.velocity = Vector2.zero;
    }
}
