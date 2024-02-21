using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatMovement : PlayerMovement
{
    public override void OnNetworkSpawn()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        m_rigidbody = GetComponent<Rigidbody2D>();
        //if (!IsOwner) return;
        m_input = GetComponent<PlayerInput>();
        m_input.enabled = true;

        m_input.currentActionMap.FindAction("Move").performed += MoveStart;
        m_input.currentActionMap.FindAction("Move").canceled += MoveEnd;
        m_input.currentActionMap.FindAction("Attack").performed += Attack;

        transform.GetChild(0).gameObject.SetActive(true);

    }

    protected void MoveStart(InputAction.CallbackContext context)
    {
        if (!IsOwner) { return; }
        m_moveDirection = context.ReadValue<Vector2>();
        StartCoroutine(Move());
    }

    protected void MoveEnd(InputAction.CallbackContext context)
    {
        if (!IsOwner) { return; }
        m_moveDirection = Vector2.zero;
    }


    protected void Attack(InputAction.CallbackContext context)
    {
        if (!IsOwner) { return; };
    }


    protected IEnumerator Move()
    {
        Debug.Log(m_moveDirection);
        while (m_moveDirection != Vector2.zero)
        {
            Debug.Log(m_moveDirection);
            m_rigidbody.AddForce(m_moveForce * Time.fixedDeltaTime * m_moveDirection);
            m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, maxSpeed);

            MoveCharacterServerRpc(m_rigidbody.velocity);
            yield return new WaitForFixedUpdate();
        }
        m_rigidbody.velocity = Vector2.zero;
        MoveCharacterServerRpc(m_rigidbody.velocity);
    }

    [ServerRpc]
    protected void MoveCharacterServerRpc(Vector2 velocity)
    {
        m_rigidbody.velocity = velocity;
    }
}
