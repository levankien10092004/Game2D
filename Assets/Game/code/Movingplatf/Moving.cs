using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Moving : MonoBehaviour
{ 
    [SerializeField] protected int speed = 1; 
    [SerializeField] protected Transform start;
    [SerializeField] protected Transform end; 
    [SerializeField] protected Vector2 muctieu; 
    void Start() 
    { 
        muctieu = start.position; 
    } // Update is called once per frame
      void Update() 
    { 
        if (Vector2.Distance(transform.position, start.position) < 0.1f) 
        { 
            muctieu = end.position; 
        }
        if (Vector2.Distance(transform.position, end.position) < 0.1f)
        { 
            muctieu = start.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, muctieu, speed*Time.deltaTime);
    } 
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
    } }