using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteRename : EditorWindow
{
     private string newNamePrefix = "";
     private bool addIndex = true;

     [MenuItem("Tools/SpriteRename")]
     private static void ShowWindow()
     {
          GetWindow<SpriteRename>("SpriteRename");
     }

     private void OnGUI()
     {
          GUILayout.Label("파일 리네임 설정", EditorStyles.boldLabel);
          GUILayout.Space(5);
          
          newNamePrefix = EditorGUILayout.TextField("새 이름 :", newNamePrefix);
          
          if (GUILayout.Button("선택된 파일 리네임 실행", GUILayout.Height(30)))
          {
               RenameSelectedFiles();
          }
     }

     private void RenameSelectedFiles()
     {
          var selectedObjects = Selection.objects;

          if (selectedObjects == null || selectedObjects.Length == 0)
          {
               EditorUtility.DisplayDialog("경고", "하나 이상 파일 선택 필요", "확인");
               return;
          }

          for (int i = 0; i < selectedObjects.Length; i++)
          {
               var obj = selectedObjects[i];
               string path = AssetDatabase.GetAssetPath(obj);
               
               if(string.IsNullOrEmpty(path)) continue;

               //Prefix Name 필요할 경우 수정
               string newName = i.ToString();
               
               string error = AssetDatabase.RenameAsset(path, newName);
          }
          AssetDatabase.SaveAssets();
          
     }
     
}
