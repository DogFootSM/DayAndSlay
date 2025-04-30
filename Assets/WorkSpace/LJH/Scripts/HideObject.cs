using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    private Color originColor;
    private Color hideColor;
    private void Start()
    {
        originColor = gameObject.GetComponent<SpriteRenderer>().color;
        
        hideColor = originColor;
        hideColor.a = 0.5f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //투명화 처리
            gameObject.GetComponent<SpriteRenderer>().color = hideColor;
        }
    }

    private void OnTriggerExit2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Player"))
        {
            //비투명화 처리
            gameObject.GetComponent<SpriteRenderer>().color = originColor;
        }
    }

}
