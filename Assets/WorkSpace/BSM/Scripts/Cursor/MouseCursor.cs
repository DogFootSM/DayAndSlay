using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D cursorImage;
      
    private void Awake()
    {
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware); 
    }
}
