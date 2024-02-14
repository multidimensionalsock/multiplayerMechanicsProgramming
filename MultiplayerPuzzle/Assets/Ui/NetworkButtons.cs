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

    ulong ClientID = 0;

    private void Awake()
    {
        ClientID = (ulong)Random.Range(1, 10000);
        gameInfo.clientID = ClientID;
        HostBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartHost(); CreateHostServerRpc(); });
        ServerBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartServer(); });
        ClientBtn.onClick.AddListener( () => { 
        NetworkManager.Singleton.StartClient(); CreateClientServerRpc(); });
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateHostServerRpc()
    {
        gameObject.SetActive(false);
        GameObject player = Instantiate(HostPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerMovement>().gameInfo = gameInfo;
        gameInfo.AddPlayer(ClientID, player);
        player.GetComponent<NetworkObject>().Spawn();
        player.GetComponent<PlayerMovement>().m_clientID = ClientID;
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateClientServerRpc()
    {
        gameObject.SetActive(false);
        GameObject player = Instantiate(HostPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerMovement>().gameInfo = gameInfo;
        gameInfo.AddPlayer(ClientID, player);
        player.GetComponent<NetworkObject>().Spawn();
        player.GetComponent<PlayerMovement>().m_clientID = ClientID;
    }

}
