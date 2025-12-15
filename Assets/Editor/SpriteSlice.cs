using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteSlice : EditorWindow
{
    private int col;
    private int row;

    [MenuItem("Tools/Sprite Slice")]
    private static void OpenWindow()
    {
        GetWindow<SpriteSlice>("Sprite Grid Slice");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite Slice", EditorStyles.boldLabel);
        GUILayout.Space(5);

        col = EditorGUILayout.IntField("Col Count", col);
        row = EditorGUILayout.IntField("Row Count", row);
        GUILayout.Space(5);

        if (GUILayout.Button("Slice Selected Texture"))
        {
            SliceSelectedTexture();
        } 
    }

    private void SliceSelectedTexture()
    {
        //선택한 스프라이트 배열 할당
        Object[] selectedObjects = Selection.objects;
        List<Texture2D> textures = new List<Texture2D>();

        //스프라이트 이미지 리스트에 추가
        foreach (var obj in selectedObjects)
        {
            if (obj is Texture2D texture2D)
            {
                textures.Add(texture2D);
            }
        }

        if (textures.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "스프라이트 이미지 선택 필요", "확인");
            return;
        }

        foreach (var texture in textures)
        {
            SliceTexture(texture);
        }
        
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("완료", "슬라이스 완료", "확인");
    }

    private void SliceTexture(Texture2D texture)
    {
        //이미지 경로 Get
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer == null)
        {
            Debug.LogError("경로 오류");
            return;
        }
        
        //Slice할 이미지 개수 및 이미지 전체 크기
        int cellWidth = texture.width / col;
        int cellHeight = texture.height / row;
        
        //스프라이트 이미지 설정값 변경
        importer.isReadable = true;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.filterMode = FilterMode.Point;
        importer.maxTextureSize = 1024;
        importer.spritePixelsPerUnit = 32;
        
        List<SpriteMetaData> metaList = new List<SpriteMetaData>();

        Texture2D readableTexture = new Texture2D(texture.width, texture.height, texture.format, false);
        Graphics.CopyTexture(texture, readableTexture);
        
        //스프라이트 이미지 이름 인덱스
        int spriteCount = 0;
        
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                Rect rect = new Rect(x * cellWidth, (row - 1 - y) * cellHeight, cellWidth, cellHeight);

                if (HasOpaquePixels(readableTexture, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height))
                {
                    SpriteMetaData meta = new SpriteMetaData
                    {
                        name = $"{texture.name}_{spriteCount++}",
                        rect = rect,
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f)
                    };
                    metaList.Add(meta);
                }
            }
        }

        importer.spritesheet = metaList.ToArray();
        importer.SaveAndReimport();
        
        Object.DestroyImmediate(readableTexture);
    }

    private bool HasOpaquePixels(Texture2D tex, int x, int y, int width, int height)
    {
        Color[] pixels = tex.GetPixels(x, y, width, height);

        foreach (var p in pixels)
        {
            if (p.a > 0.01f)
            {
                return true;
            }
        }
        
        return false;
    }
    
}
