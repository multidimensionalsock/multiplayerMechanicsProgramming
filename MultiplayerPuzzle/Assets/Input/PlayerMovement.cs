using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerMovement : NetworkBehaviour
{
    protected PlayerInput m_input;
    public Vector2 m_moveDirection;
    //[SerializeField] AnimationCurve m_forceAdjust;
    [SerializeField] protected float m_moveForce;
    [SerializeField] protected float maxSpeed;
    protected Rigidbody2D m_rigidbody;
    public GameObject spawningObject;
    protected bool localObj;
    public GameInfo gameInfo;

    protected Vector2 facingDirection;
    public ulong m_clientID = 0;

    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        if (gameInfo.clientID != m_clientID) return;
        m_input = GetComponent<PlayerInput>();
        m_input.enabled = true;
        
        m_input.currentActionMap.FindAction("Move").performed += MoveStart;
        m_input.currentActionMap.FindAction("Move").canceled += MoveEnd;
        m_input.currentActionMap.FindAction("Attack").performed += Attack;

        //transform.GetChild(0).GetComponent<Camera>().enabled = true;
    }

    protected void MoveStart(InputAction.CallbackContext context)
    {
        m_moveDirection = context.ReadValue<Vector2>();
        PassDirectionalDataServerRpc(m_clientID);
    }


    protected void MoveEnd(InputAction.CallbackContext context)
    {
        m_moveDirection = Vector2.zero;
        PassDirectionalDataServerRpc(m_clientID);
    }

    [ServerRpc(RequireOwnership = false)]
    void PassDirectionalDataServerRpc(ulong clientID)
    {
        Debug.Log(gameInfo.playerList);
        GameObject player = gameInfo.playerList[clientID];
        player.GetComponent<PlayerMovement>().m_moveDirection = m_moveDirection;
    }


    protected virtual void Attack(InputAction.CallbackContext context)
    {
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;
        if (m_moveDirection != Vector2.zero)
        {
            m_rigidbody.AddForce(m_moveForce * Time.fixedDeltaTime * m_moveDirection);
            m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, maxSpeed);
            return;
        }
        m_rigidbody.velocity = Vector2.zero;
        
    }

}
