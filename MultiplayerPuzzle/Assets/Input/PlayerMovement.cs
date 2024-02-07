using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerMovement : NetworkBehaviour
{
    protected PlayerInput m_input;
    protected Vector2 m_moveDirection;
    //[SerializeField] AnimationCurve m_forceAdjust;
    [SerializeField] protected float m_moveForce;
    [SerializeField] protected float maxSpeed;
    protected Rigidbody2D m_rigidbody;
    public GameObject spawningObject;
    protected bool localObj; 

    protected Vector2 facingDirection;

    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        localObj = spawningObject.GetComponent<PlayerClassSelector>().IsLocalPlayer; //spawning object is null on cat so ntohing under it is called? 
        if (!localObj) { return; }
        m_input = GetComponent<PlayerInput>();
        m_input.enabled = true;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_input.currentActionMap.FindAction("Move").performed += MoveStart;
        m_input.currentActionMap.FindAction("Move").canceled += MoveEnd;
        m_input.currentActionMap.FindAction("Attack").performed += Attack;

        transform.GetChild(0).GetComponent<Camera>().enabled = true;
    }

    protected void MoveStart(InputAction.CallbackContext context)
    {
        if (!localObj) { return;  }
        m_moveDirection = context.ReadValue<Vector2>();
        facingDirection = m_moveDirection;
        StartCoroutine(Move());
    }

    protected void MoveEnd(InputAction.CallbackContext context)
    {
        if (!localObj) { return; }
        m_moveDirection = Vector2.zero;
    }

    protected virtual void Attack(InputAction.CallbackContext context)
    {
        if (!localObj) { return; };
    }

    IEnumerator Move()
    {
        Debug.Log(m_moveDirection);
        while(m_moveDirection != Vector2.zero)
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
    void MoveCharacterServerRpc(Vector2 velocity)
    {;
        m_rigidbody.velocity = velocity;
    }

}
