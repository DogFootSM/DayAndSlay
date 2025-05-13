using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class SpriteSliceSelector : EditorWindow
{
    private Texture2D sourceTexture;
    private Object[] slicedSprites;
    private Vector2 scrollPos;
    private Dictionary<Sprite, bool> spriteSelection = new();
    private string outputFolder = "Assets/Resources/Preset/Animations/Character/SHIRT";

    private bool selectAll = false;

    private int pixelsPerUnit = 32;
    private FilterMode filterMode = FilterMode.Point;
    private int maxSize = 1024;

    [MenuItem("Tools/슬라이스 스프라이트 선택 복사기")]
    public static void ShowWindow()
    {
        GetWindow<SpriteSliceSelector>("슬라이스 선택 복사기");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("원본 텍스처", EditorStyles.boldLabel);
        sourceTexture = (Texture2D)EditorGUILayout.ObjectField(sourceTexture, typeof(Texture2D), false);

        if (sourceTexture != null && GUILayout.Button("슬라이스 목록 불러오기"))
        {
            LoadSlicedSprites();
        }

        if (slicedSprites != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(selectAll ? "전체 해제" : "전체 선택"))
            {
                ToggleAllSelections(!selectAll);
                selectAll = !selectAll;
            }
            EditorGUILayout.EndHorizontal();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (Object obj in slicedSprites)
            {
                if (obj is Sprite sprite)
                {
                    if (!spriteSelection.ContainsKey(sprite))
                        spriteSelection[sprite] = false;

                    EditorGUILayout.BeginHorizontal();
                    spriteSelection[sprite] = EditorGUILayout.Toggle(spriteSelection[sprite], GUILayout.Width(20));

                    GUILayout.Label(sprite.texture, GUILayout.Width(32), GUILayout.Height(32));
                    EditorGUILayout.LabelField(sprite.name);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            outputFolder = EditorGUILayout.TextField("출력 경로", outputFolder);

            pixelsPerUnit = EditorGUILayout.IntField("Pixels Per Unit", pixelsPerUnit);
            filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", filterMode);
            maxSize = EditorGUILayout.IntField("Max Size", maxSize);

            if (GUILayout.Button("선택한 스프라이트 복사"))
            {
                CopySelectedSprites();
            }
        }
    }

    void LoadSlicedSprites()
    {
        string path = AssetDatabase.GetAssetPath(sourceTexture);
        slicedSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
        spriteSelection.Clear();
    }

    void ToggleAllSelections(bool state)
    {
        foreach (Object obj in slicedSprites)
        {
            if (obj is Sprite sprite)
            {
                spriteSelection[sprite] = state;
            }
        }
    }

    void CopySelectedSprites()
    {
        if (sourceTexture == null)
        {
            Debug.LogError("소스 텍스처가 없습니다");
            return;
        }

        string texturePath = AssetDatabase.GetAssetPath(sourceTexture);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(texturePath);
        if (!importer.isReadable)
        {
            importer.isReadable = true;
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
        }

        Directory.CreateDirectory(outputFolder);

        foreach (var pair in spriteSelection)
        {
            if (pair.Value)
            {
                Sprite sprite = pair.Key;

                Texture2D extracted = new((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] pixels = sourceTexture.GetPixels(
                    (int)sprite.rect.x, (int)sprite.rect.y,
                    (int)sprite.rect.width, (int)sprite.rect.height);
                extracted.SetPixels(pixels);
                extracted.Apply();

                byte[] bytes = extracted.EncodeToPNG();
                string exportPath = Path.Combine(outputFolder, sprite.name + ".png");
                File.WriteAllBytes(exportPath, bytes);

                AssetDatabase.ImportAsset(exportPath);
                TextureImporter texImporter = (TextureImporter)TextureImporter.GetAtPath(exportPath);
                texImporter.textureType = TextureImporterType.Sprite;
                texImporter.spritePixelsPerUnit = pixelsPerUnit;
                texImporter.filterMode = filterMode;
                texImporter.maxTextureSize = maxSize;
                texImporter.spriteImportMode = SpriteImportMode.Multiple;
                texImporter.SaveAndReimport();

                Debug.Log($"복사 및 설정 완료: {sprite.name}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("선택한 스프라이트 복사 완료!");
    }
}