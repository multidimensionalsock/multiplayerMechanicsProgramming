using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkButtons : NetworkBehaviour
{
    [SerializeField] Button HostBtn;
    [SerializeField] Button ServerBtn;
    [SerializeField] Button ClientBtn;

    [SerializeField] GameObject HostPrefab;
    [SerializeField] GameObject ClientPrefab;
    [SerializeField] GameInfo gameInfo;

    //ulong ClientID = 0;

    private void Awake()
    {
        HostBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartHost(); CreateHostServerRpc(); });
        ServerBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartServer(); });
        ClientBtn.onClick.AddListener( () => { 
        NetworkManager.Singleton.StartClient(); CreateClientServerRpc(); });
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateHostServerRpc(ServerRpcParams rpcParams = default)
    {
        gameObject.SetActive(false);
        GameObject player = Instantiate(HostPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerMovement>().gameInfo = gameInfo;
        //gameInfo.AddPlayer(ClientID, player);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateClientServerRpc(ServerRpcParams rpcParams = default)
    {
        gameObject.SetActive(false);
        GameObject player = Instantiate(ClientPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerMovement>().gameInfo = gameInfo;
        player.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
    }

}
