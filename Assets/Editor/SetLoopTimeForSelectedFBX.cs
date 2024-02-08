using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SetLoopTimeForSelectedFBX : EditorWindow
{
    private bool loopTime = false;

    // 에디터 메뉴에 "Set LoopTime for Selected FBX"를 추가합니다.
    [MenuItem("Tools/Set LoopTime for Selected FBX")]
    private static void OpenWindow()
    {
        // SetLoopTimeForSelectedFBX 윈도우를 엽니다.
        GetWindow<SetLoopTimeForSelectedFBX>("Set LoopTime");
    }

    private void OnGUI()
    {
        // 윈도우의 제목을 설정합니다.
        GUILayout.Label("Set LoopTime for Selected FBX", EditorStyles.boldLabel);
        // LoopTime을 토글로 설정합니다.
        loopTime = EditorGUILayout.Toggle("LoopTime:", loopTime);

        if (GUILayout.Button("Apply"))
        {
            // 선택한 .fbx 파일에 LoopTime을 적용합니다.
            SetLoopTime(loopTime);
        }
    }

    // 선택한 .fbx 파일에 LoopTime을 적용하는 함수
    private void SetLoopTime(bool value)
    {
        Object[] selectedObjects = Selection.objects;
        foreach (Object selectedObject in selectedObjects)
        {
            // 선택한 오브젝트의 경로를 가져옵니다.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // 선택한 오브젝트가 .fbx 파일인지 확인합니다.
            ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (modelImporter != null)
            {
                // 선택한 .fbx 파일의 모든 애니메이션 클립에 LoopTime을 적용합니다.
                modelImporter.clipAnimations = SetLoopTimeForAnimations(modelImporter.clipAnimations, value);
                AssetDatabase.ImportAsset(assetPath);
                Debug.Log("LoopTime set to " + value.ToString() + " for FBX: " + selectedObject.name);
            }
            else
            {
                Debug.LogError("Selected object is not an FBX file: " + selectedObject.name);
            }
        }
    }

    // 애니메이션 클립의 LoopTime을 설정하는 함수
    private ModelImporterClipAnimation[] SetLoopTimeForAnimations(ModelImporterClipAnimation[] animations, bool value)
    {
        if (animations == null)
            return null;

        List<ModelImporterClipAnimation> updatedAnimations = new List<ModelImporterClipAnimation>();
        foreach (var animation in animations)
        {
            // 각 애니메이션 클립의 LoopTime을 설정합니다.
            animation.loopTime = value;
            updatedAnimations.Add(animation);
        }
        return updatedAnimations.ToArray();
    }
}
