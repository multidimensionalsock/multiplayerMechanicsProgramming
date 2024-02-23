using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatMovement : PlayerMovement
{
    GameObject magicPillarCollision = null;

    protected override void Attack(InputAction.CallbackContext context)
    {
        if (magicPillarCollision != null ) 
        { 
            magicPillarCollision.GetComponent<magicalPillar>().TurnOnMagicalPillarServerRpc(); 
            //set to ingore collision now 
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collieding");
        if (collision.gameObject.layer == 10)
        {
            magicPillarCollision = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.layer == 10)
        //{
        //    magicPillarCollision = null;
        //}
    }
}
