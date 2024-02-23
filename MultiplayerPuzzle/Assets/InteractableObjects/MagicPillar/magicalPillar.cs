using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class magicalPillar : NetworkBehaviour
{
    Rigidbody2D m_rigidbody;
    NetworkVariable<bool> isLit = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        if (IsHost)
        {
            isLit.Value = false;
            isLit.OnValueChanged += LitValueChanged;
            
        }
        else
        {
            if (isLit.Value != false)
            {
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            }
            else
            {
                GetComponent<Animator>().SetBool("Lit", true);
                m_rigidbody.constraints = RigidbodyConstraints2D.None;
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            isLit.OnValueChanged += LitValueChanged;
        }
    }


    [ServerRpc]
    public void TurnOnMagicalPillarServerRpc()
    {
        isLit.Value = true;
        GetComponent<Animator>().SetBool("Lit", true);
    }

    void LitValueChanged(bool prev, bool newval)
    {
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

}
