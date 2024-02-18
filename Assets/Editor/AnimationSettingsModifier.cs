using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using Unity.Plastic.Newtonsoft.Json.Linq;
using System.Xml.Linq;

public class AnimationClipInspectorScript : MonoBehaviour
{
    // ���õ� ��ü�� ó���ϴ� �Լ�
    [MenuItem("Tools/FBX Root Animation")]
    private static void ProcessSelectedObject()
    {
        // ���õ� ��ü���� �����ͼ� ���� ó���մϴ�.
        Object[] selectedObjects = Selection.objects;
        foreach (Object selectedObject in selectedObjects)
        {
            // ������ ��ü�� ��θ� �����ɴϴ�.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // ������ ��ü�� .fbx �������� Ȯ���մϴ�.
            ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (modelImporter != null)
            {
                modelImporter.clipAnimations = ApplyRootAnimationSet(modelImporter.clipAnimations, selectedObject);
                AssetDatabase.ImportAsset(assetPath);
                Debug.Log("Set Root Animation");
            }
            else
            {
                Debug.LogError("������ ��ü�� FBX ������ �ƴմϴ�: " + selectedObject.name);
            }

        }
    }

    private static ModelImporterClipAnimation[] ApplyRootAnimationSet(ModelImporterClipAnimation[] animations, Object obj)
    {
        if (animations == null)
            return null;

        List<ModelImporterClipAnimation> updatedAnimations = new List<ModelImporterClipAnimation>();

        foreach (var animation in animations)
        {
            animation.lockRootRotation = true;
            animation.lockRootHeightY = true;
            animation.lockRootPositionXZ = false;

            updatedAnimations.Add(animation);
        }

        return updatedAnimations.ToArray();
    }
}
