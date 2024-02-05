using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class collisionWithBlock : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(gameObject.transform, true);
            //make it child
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = collision.gameObject.transform;
        }
    }
}
