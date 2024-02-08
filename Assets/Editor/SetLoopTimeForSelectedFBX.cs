using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SetLoopTimeForSelectedFBX : EditorWindow
{
    private bool loopTime = false;

    // ������ �޴��� "Set LoopTime for Selected FBX"�� �߰��մϴ�.
    [MenuItem("Tools/Set LoopTime for Selected FBX")]
    private static void OpenWindow()
    {
        // SetLoopTimeForSelectedFBX �����츦 ���ϴ�.
        GetWindow<SetLoopTimeForSelectedFBX>("Set LoopTime");
    }

    private void OnGUI()
    {
        // �������� ������ �����մϴ�.
        GUILayout.Label("Set LoopTime for Selected FBX", EditorStyles.boldLabel);
        // LoopTime�� ��۷� �����մϴ�.
        loopTime = EditorGUILayout.Toggle("LoopTime:", loopTime);

        if (GUILayout.Button("Apply"))
        {
            // ������ .fbx ���Ͽ� LoopTime�� �����մϴ�.
            SetLoopTime(loopTime);
        }
    }

    // ������ .fbx ���Ͽ� LoopTime�� �����ϴ� �Լ�
    private void SetLoopTime(bool value)
    {
        Object[] selectedObjects = Selection.objects;
        foreach (Object selectedObject in selectedObjects)
        {
            // ������ ������Ʈ�� ��θ� �����ɴϴ�.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // ������ ������Ʈ�� .fbx �������� Ȯ���մϴ�.
            ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (modelImporter != null)
            {
                // ������ .fbx ������ ��� �ִϸ��̼� Ŭ���� LoopTime�� �����մϴ�.
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

    // �ִϸ��̼� Ŭ���� LoopTime�� �����ϴ� �Լ�
    private ModelImporterClipAnimation[] SetLoopTimeForAnimations(ModelImporterClipAnimation[] animations, bool value)
    {
        if (animations == null)
            return null;

        List<ModelImporterClipAnimation> updatedAnimations = new List<ModelImporterClipAnimation>();
        foreach (var animation in animations)
        {
            // �� �ִϸ��̼� Ŭ���� LoopTime�� �����մϴ�.
            animation.loopTime = value;
            updatedAnimations.Add(animation);
        }
        return updatedAnimations.ToArray();
    }
}
