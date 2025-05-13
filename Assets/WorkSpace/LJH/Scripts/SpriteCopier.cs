using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteCopier : EditorWindow
{
    string sourcePath = "Assets/Imports/옷";
    string targetPath = "Assets/MySprites/옷";
    string keyword = "clothes_"; // 예시 키워드

    [MenuItem("Tools/복사기 열기")]
    public static void ShowWindow()
    {
        GetWindow<SpriteCopier>("스프라이트 복사기");
    }

    void OnGUI()
    {
        GUILayout.Label("스프라이트 복사 도구", EditorStyles.boldLabel);
        sourcePath = EditorGUILayout.TextField("소스 폴더", sourcePath);
        targetPath = EditorGUILayout.TextField("타겟 폴더", targetPath);
        keyword = EditorGUILayout.TextField("이름 필터 키워드", keyword);

        if (GUILayout.Button("복사 실행"))
        {
            CopySprites();
        }
    }

    void CopySprites()
    {
        if (!AssetDatabase.IsValidFolder(sourcePath) || !AssetDatabase.IsValidFolder(targetPath))
        {
            Debug.LogError("폴더 경로가 올바르지 않음");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { sourcePath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            if (!Path.GetFileName(assetPath).Contains(keyword)) continue;

            string fileName = Path.GetFileName(assetPath);
            string destPath = Path.Combine(targetPath, fileName);

            AssetDatabase.CopyAsset(assetPath, destPath);
        }

        AssetDatabase.Refresh();
        Debug.Log("복사 완료!");
    }
}