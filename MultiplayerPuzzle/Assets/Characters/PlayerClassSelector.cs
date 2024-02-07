using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerClassSelector : NetworkBehaviour
{
    public GameObject human;
    public GameObject cat;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (!IsLocalPlayer) { return; }
        if (IsServer)
        {
            CreateHumanServerRpc();
            return;
        }
        CreateCatServerRpc();

    }

    [ServerRpc]
    void CreateCatServerRpc()
    {
        GameObject projectile = Instantiate(cat, transform.position, transform.rotation);
        projectile.GetComponent<PlayerMovement>().spawningObject = gameObject;
        projectile.GetComponent<NetworkObject>().Spawn();
        

    }

    [ServerRpc]
    void CreateHumanServerRpc()
    {
        GameObject projectile = Instantiate(human, transform.position, transform.rotation);
        projectile.GetComponent<PlayerMovement>().spawningObject = gameObject;
        projectile.GetComponent<NetworkObject>().Spawn();

    }
}
