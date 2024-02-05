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
    int rasndom;
    [SerializeField] GameObject m_attackObject;
    [SerializeField] float fireAttackCooldown;
    bool canFireAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        PingServerRpc();
        m_input = GetComponent<PlayerInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_input.currentActionMap.FindAction("Move").performed += MoveStart;
        m_input.currentActionMap.FindAction("Move").canceled += MoveEnd;
        m_input.currentActionMap.FindAction("Attack").performed += Attack;
        rasndom = Random.Range(0, 100);
    }

    void MoveStart(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer) { return;  }
        m_moveDirection = context.ReadValue<Vector2>();
        StartCoroutine(Move());
    }

    void MoveEnd(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer) { return; }
        m_moveDirection = Vector2.zero;
    }

    void Attack(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer) { return; };
        if (!canFireAttack) { return; }
        CreateGameObjectServerRPC();
        StartCoroutine(
        Cooldown(fireAttackCooldown));
    }

    //cooldown
    IEnumerator Cooldown(float cooldownTime)
    {
        canFireAttack = false;
        yield return new WaitForSecondsRealtime(cooldownTime);
        canFireAttack = true;
    }

    [ServerRpc]
    void CreateGameObjectServerRPC()
    {
        GameObject projectile = Instantiate(m_attackObject, transform.position, transform.rotation);
        projectile.GetComponent<NetworkObject>().Spawn();
        
        projectile.GetComponent<Rigidbody2D>().velocity = m_moveDirection * 1f;
        projectile.transform.rotation = Quaternion.Euler( new Vector3(0, 0, Vector2.Angle(Vector2.zero, m_moveDirection)));
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

    [ServerRpc]
    public void PingServerRpc()
    {
        Debug.Log("Working on the server" + rasndom);
    }
}
