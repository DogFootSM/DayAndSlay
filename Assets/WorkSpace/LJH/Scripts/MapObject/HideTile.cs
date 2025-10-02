using UnityEngine;
using UnityEngine.Tilemaps;

public class HideTile : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private Color originColor;
    private Color hideColor;

    private void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        
        originColor = tilemapRenderer.material.color;
        hideColor = originColor;
        hideColor.a = 0.25f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            tilemapRenderer.material.color = hideColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemapRenderer.material.color = originColor;
        }
    }
}