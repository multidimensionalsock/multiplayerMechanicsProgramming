using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameInfo : NetworkBehaviour
{
    public Dictionary<ulong, GameObject> playerList = new Dictionary<ulong, GameObject>();
   
    public void AddPlayer(ulong playerId, GameObject playerObj)
    {
        playerList.Add(playerId, playerObj);
    }
}
