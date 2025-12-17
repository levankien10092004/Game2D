using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Moving : MonoBehaviour
{
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected Transform start;
    [SerializeField] protected Transform end;

    protected Vector2 muctieu;
    private Transform player;
    private bool removeParent = false;

    void Start()
    {
        muctieu = start.position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, start.position) < 0.1f)
            muctieu = end.position;

        if (Vector2.Distance(transform.position, end.position) < 0.1f)
            muctieu = start.position;

        transform.position = Vector2.MoveTowards(transform.position,
            muctieu,
            speed * Time.deltaTime
        );
    }

    private void LateUpdate()
    {
        if (removeParent && player != null)
        {
            player.SetParent(null);
            removeParent = false;
            player = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            removeParent = true;
        }
    }
}