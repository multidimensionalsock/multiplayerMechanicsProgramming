using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatMovement : PlayerMovement
{
    GameObject magicPillarCollision = null;
    GameObject teleporter = null;

    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 9, true);
    }

    protected override void Attack(InputAction.CallbackContext context)
    {
        if (magicPillarCollision != null ) 
        { 
            magicPillarCollision.GetComponent<magicalPillar>().TurnOnMagicalPillarServerRpc(); 
            //set to ingore collision now 
        }
        if (teleporter != null )
        {
            teleporter.GetComponent<Teleport>().TeleportMe(gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            magicPillarCollision = collision.gameObject;

        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("collieding");
        if (collision.gameObject.layer == 10)
        {
            magicPillarCollision = null;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            teleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            teleporter = null;
        }
    }
}
