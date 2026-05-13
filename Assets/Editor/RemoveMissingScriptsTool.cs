using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class RemoveMissingScriptsTool
{
    [MenuItem("Tools/Missing Scripts/Remove From Open Scene")]
    public static void RemoveFromOpenScene()
    {
        int totalRemoved = 0;

        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (GameObject obj in allObjects)
        {
            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);

            if (removed > 0)
            {
                Debug.Log($"Removed {removed} missing script(s) from scene object: {obj.name}");
                totalRemoved += removed;
                EditorUtility.SetDirty(obj);
            }
        }

        if (totalRemoved > 0)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        Debug.Log($"Total missing scripts removed from open scene: {totalRemoved}");
    }

    [MenuItem("Tools/Missing Scripts/Remove From All Prefabs")]
    public static void RemoveFromAllPrefabs()
    {
        int totalRemoved = 0;

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);

            int removedInPrefab = 0;

            Transform[] children = prefabRoot.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);

                if (removed > 0)
                {
                    removedInPrefab += removed;
                    totalRemoved += removed;
                    Debug.Log($"Removed {removed} missing script(s) from prefab: {path} / {child.name}");
                }
            }

            if (removedInPrefab > 0)
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);

            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Total missing scripts removed from prefabs: {totalRemoved}");
    }
}