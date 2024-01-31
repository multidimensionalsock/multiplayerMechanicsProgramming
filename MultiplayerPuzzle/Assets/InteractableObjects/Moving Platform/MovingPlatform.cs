using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MovingPlatform : NetworkBehaviour
{
    Transform[] positionsToMoveTo;
    int aimPos; //posotion in the above array to move to
    GameObject movingBlock;
    [SerializeField] float speed;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        positionsToMoveTo = GetComponentsInChildren<Transform>();
        movingBlock = transform.GetChild(0).gameObject;
        aimPos = 1; //skip child 0 (the actual block)
        StartCoroutine(MoveBlock());
    }

    IEnumerator MoveBlock()
    {
        Debug.Log("go");
        while (1 == 1)
        {
            movingBlock.transform.position = Vector2.MoveTowards(movingBlock.transform.position, positionsToMoveTo[aimPos].position, speed * Time.fixedDeltaTime);

            if (movingBlock.transform.position == positionsToMoveTo[aimPos].position)
            {
                aimPos++;
                if (aimPos >= positionsToMoveTo.Length)
                {
                    aimPos = 1;
                }
            }
            yield return new WaitForFixedUpdate();
        }

        
    }

}
