using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOval : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.parent == this.transform)
            {
                collision.transform.SetParent(null);
            }
        }
    }
}
