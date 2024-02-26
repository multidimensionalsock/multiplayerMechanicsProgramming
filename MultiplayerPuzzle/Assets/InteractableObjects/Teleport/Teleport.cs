using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Teleport : NetworkBehaviour
{
    [SerializeField] GameObject teleportPoint;
    GameObject teleportobj;

    public void TeleportMe(GameObject obj)
    {
        teleportobj = obj;
        teleportMeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void teleportMeServerRpc()
    {
        teleportobj.transform.position = teleportPoint.transform.position;
    }
}
