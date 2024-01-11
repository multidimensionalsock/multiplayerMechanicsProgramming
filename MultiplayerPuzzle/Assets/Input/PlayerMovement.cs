using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerMovement : NetworkBehaviour
{
    PlayerInput m_input;
    Vector3 m_moveDirection;
    [SerializeField] AnimationCurve m_forceAdjust;
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
        m_moveDirection = context.ReadValue<Vector2>();
        m_moveDirection = new Vector3(m_moveDirection.x, 0f, m_moveDirection.y);
        StartCoroutine(Move());
    }

    void MoveEnd(InputAction.CallbackContext context)
    {
        m_moveDirection = Vector2.zero;
    }

    IEnumerator Move()
    {
        
        while(m_moveDirection != Vector3.zero)
        {
            Debug.Log(m_moveDirection);
            transform.position += m_moveDirection;
            //m_rigidbody.AddForce( m_moveForce * Time.fixedDeltaTime * m_moveDirection);
            //set max force to move force and it keeps about the same
            //set to zero vector at end of this if you want to just stop. or could set velocity lower?like 0.1?
            yield return new WaitForFixedUpdate();
        }
    }
}
