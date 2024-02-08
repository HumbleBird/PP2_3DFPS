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
            // ������ ��θ� �����ɴϴ�.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // ������ �̸��� �����ɴϴ�.
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            // ������ Ȯ���ڸ� �����ɴϴ�.
            string fileExtension = Path.GetExtension(assetPath);
            // �� ������ �̸��� ����ϴ�.
            string newFileName = prefixToAdd + fileName + suffixToAdd + fileExtension;
            // �� ������ ��θ� ����ϴ�.
            string newPath = Path.GetDirectoryName(assetPath) + "/" + newFileName;

            // ������ �̸��� �����մϴ�.
            AssetDatabase.RenameAsset(assetPath, newFileName);
            // ����� ������ ����Ʈ�մϴ�.
            AssetDatabase.ImportAsset(newPath);
        }

        // ����� ������ �����մϴ�.
        AssetDatabase.Refresh();
        Debug.Log("Files renamed with prefix or suffix.");
    }


    [MenuItem("Tools/Rename Animation Clips to FBX Name", false, 10)]
    private static void RenameAnimationClipsToFBXName()
    {
        // ���� ���õ� ��� ������Ʈ�� �����ɴϴ�.
        Object[] selectedObjects = Selection.objects;

        // ���õ� ��� ������Ʈ�� ���� �ݺ��մϴ�.
        foreach (Object selectedObject in selectedObjects)
        {
            // ���õ� ������Ʈ�� ��θ� �����ɴϴ�.
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // �ش� ����� Ȯ���ڸ� Ȯ���Ͽ� .fbx �������� Ȯ���մϴ�.
            if (Path.GetExtension(assetPath).ToLower() == ".fbx")
            {
                // .fbx ���Ͽ� ���� Importer ������ �����ɴϴ�.
                ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                if (modelImporter != null)
                {
                    // .fbx ������ Animation �ǿ� �ִ� �ִϸ��̼� Ŭ������ �̸��� �����մϴ�.
                    ModelImporterClipAnimation[] animations = modelImporter.defaultClipAnimations;
                    foreach (ModelImporterClipAnimation animation in animations)
                    {
                        // �ִϸ��̼� Ŭ���� �̸��� .fbx ������ �̸����� �����մϴ�.
                        animation.name = Path.GetFileNameWithoutExtension(assetPath);
                    }

                    // ����� �ִϸ��̼� Ŭ������ ������ Importer�� �����մϴ�.
                    modelImporter.clipAnimations = animations;
                    // ����� Importer ������ �����մϴ�.
                    AssetDatabase.ImportAsset(assetPath);
                    // ����� ������ �����մϴ�.
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
                // .fbx ������ �ƴ� ��� �ش� ������Ʈ�� �����մϴ�.
                Debug.LogWarning("Selected object is not an .fbx file: " + assetPath);
            }
        }
    }
}

