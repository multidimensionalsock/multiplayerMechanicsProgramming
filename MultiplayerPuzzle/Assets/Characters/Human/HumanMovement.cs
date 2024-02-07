using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HumanMovement : PlayerMovement
{
    [SerializeField] GameObject m_attackObject;
    [SerializeField] float fireAttackCooldown;
    bool canFireAttack = true;
    protected override void Attack(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer) { return; };
        if (!canFireAttack) { return; }
        CreateGameObjectServerRPC();
        StartCoroutine(Cooldown(fireAttackCooldown));
    }

    [ServerRpc]
    void CreateGameObjectServerRPC()
    {
        GameObject projectile = Instantiate(m_attackObject, transform.position, transform.rotation);
        projectile.GetComponent<NetworkObject>().Spawn();

        projectile.GetComponent<Rigidbody2D>().velocity = facingDirection * 1f;
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.Angle(Vector2.zero, m_moveDirection)));
    }

    //cooldown
    protected IEnumerator Cooldown(float cooldownTime)
    {
        canFireAttack = false;
        yield return new WaitForSecondsRealtime(cooldownTime);
        canFireAttack = true;
    }
}