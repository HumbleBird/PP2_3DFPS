using UnityEngine;
using UnityEditor;
using System.IO;

public class RenameFilesWithPrefixOrSuffix : EditorWindow
{
    private string prefixToAdd = "";
    private string suffixToAdd = "";

    [MenuItem("Tools/Rename Animation Clips to FBX Name", false, 10)]
    private static void RenameAnimationClipsToFBXNameMenu()
    {
        RenameAnimationClipsToFBXName();
    }

    [MenuItem("Tools/Rename Selected Files", false, 11)]
    private static void ShowRenameFilesWindow()
    {
        RenameFilesWithPrefixOrSuffix window = GetWindow<RenameFilesWithPrefixOrSuffix>("Rename Files");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Rename Selected Files", EditorStyles.boldLabel);
        prefixToAdd = EditorGUILayout.TextField("Prefix to Add:", prefixToAdd);
        suffixToAdd = EditorGUILayout.TextField("Suffix to Add:", suffixToAdd);

        if (GUILayout.Button("Rename Files"))
        {
            RenameFilesWithPrefixOrSuffix2();
        }
    }

    private void RenameFilesWithPrefixOrSuffix2()
    {
        Object[] selectedObjects = Selection.objects;

        foreach (Object selectedObject in selectedObjects)
        {
            // 파일의 경로를 가져옵니다.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // 파일의 이름을 가져옵니다.
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            // 파일의 확장자를 가져옵니다.
            string fileExtension = Path.GetExtension(assetPath);
            // 새 파일의 이름을 만듭니다.
            string newFileName = prefixToAdd + fileName + suffixToAdd + fileExtension;
            // 새 파일의 경로를 만듭니다.
            string newPath = Path.GetDirectoryName(assetPath) + "/" + newFileName;

            // 파일의 이름을 변경합니다.
            AssetDatabase.RenameAsset(assetPath, newFileName);
            // 변경된 파일을 임포트합니다.
            AssetDatabase.ImportAsset(newPath);
        }

        // 변경된 내용을 저장합니다.
        AssetDatabase.Refresh();
        Debug.Log("Files renamed with prefix or suffix.");
    }


    [MenuItem("Tools/Rename Animation Clips to FBX Name", false, 10)]
    private static void RenameAnimationClipsToFBXName()
    {
        // 현재 선택된 모든 오브젝트를 가져옵니다.
        Object[] selectedObjects = Selection.objects;

        // 선택된 모든 오브젝트에 대해 반복합니다.
        foreach (Object selectedObject in selectedObjects)
        {
            // 선택된 오브젝트의 경로를 가져옵니다.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // 해당 경로의 확장자를 확인하여 .fbx 파일인지 확인합니다.
            if (Path.GetExtension(assetPath).ToLower() == ".fbx")
            {
                // .fbx 파일에 대한 Importer 설정을 가져옵니다.
                ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                if (modelImporter != null)
                {
                    // .fbx 파일의 Animation 탭에 있는 애니메이션 클립들의 이름을 변경합니다.
                    ModelImporterClipAnimation[] animations = modelImporter.defaultClipAnimations;
                    foreach (ModelImporterClipAnimation animation in animations)
                    {
                        // 애니메이션 클립의 이름을 .fbx 파일의 이름으로 변경합니다.
                        animation.name = Path.GetFileNameWithoutExtension(assetPath);
                    }

                    // 변경된 애니메이션 클립들의 설정을 Importer에 적용합니다.
                    modelImporter.clipAnimations = animations;
                    // 변경된 Importer 설정을 적용합니다.
                    AssetDatabase.ImportAsset(assetPath);
                    // 변경된 내용을 저장합니다.
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Debug.Log("Animation clip names in " + assetPath + " updated to FBX name: " + Path.GetFileNameWithoutExtension(assetPath));
                }
                else
                {
                    Debug.LogWarning("Failed to get ModelImporter for: " + assetPath);
                }
            }
            else
            {
                // .fbx 파일이 아닌 경우 해당 오브젝트는 무시합니다.
                Debug.LogWarning("Selected object is not an .fbx file: " + assetPath);
            }
        }
    }
}

