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

    ulong ClientID;

    private void Awake()
    {
        ClientID = (ulong)Random.Range(0, 10000);
        HostBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartHost(); CreateHostServerRpc(); });
        ServerBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartServer(); });
        ClientBtn.onClick.AddListener( () => { NetworkManager.Singleton.StartClient(); CreateClientServerRpc(); });
    }

    [ServerRpc]
    private void CreateHostServerRpc()
    {
        GameObject player = Instantiate(HostPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<NetworkObject>().Spawn();
        player.GetComponent<PlayerMovement>().m_clientID = ClientID;

    }

    [ServerRpc]
    private void CreateClientServerRpc()
    {
        GameObject player = Instantiate(HostPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<NetworkObject>().Spawn();
        player.GetComponent<PlayerMovement>().m_clientID = ClientID;
    }
}
