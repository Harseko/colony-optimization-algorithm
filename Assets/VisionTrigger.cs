using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTrigger : MonoBehaviour
{
    public CellScript cell;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "FindBaseTrigger")
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Base"))
            {   
                if (cell.focus == collision.gameObject)
                {
                    cell.handleFindBaseTrigger(collision);
                }
            }
        }

        if (gameObject.name == "EntersBaseTrigger")
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Base"))
            {
                if (cell.focus == collision.gameObject)
                {
                    cell.handleCollissionWithBase(collision);
                }
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.name == "EntersBaseTrigger")
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Base"))
            {
                if (cell.focus == collision.gameObject)
                {
                    cell.handleCollissionWithBase(collision);
                }
            }
        }
    }
}
