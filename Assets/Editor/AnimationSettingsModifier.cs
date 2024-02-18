using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using Unity.Plastic.Newtonsoft.Json.Linq;
using System.Xml.Linq;

public class AnimationClipInspectorScript : MonoBehaviour
{
    // 선택된 객체를 처리하는 함수
    [MenuItem("Tools/FBX Root Animation")]
    private static void ProcessSelectedObject()
    {
        // 선택된 객체들을 가져와서 각각 처리합니다.
        Object[] selectedObjects = Selection.objects;
        foreach (Object selectedObject in selectedObjects)
        {
            // 선택한 객체의 경로를 가져옵니다.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // 선택한 객체가 .fbx 파일인지 확인합니다.
            ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (modelImporter != null)
            {
                modelImporter.clipAnimations = ApplyRootAnimationSet(modelImporter.clipAnimations, selectedObject);
                AssetDatabase.ImportAsset(assetPath);
                Debug.Log("Set Root Animation");
            }
            else
            {
                Debug.LogError("선택한 객체는 FBX 파일이 아닙니다: " + selectedObject.name);
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
