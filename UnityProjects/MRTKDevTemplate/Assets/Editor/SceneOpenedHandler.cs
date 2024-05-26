using Photon.Pun;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// This class initializes upon loading due to the attribute below and attaches an event handler
// to the sceneOpened event of the EditorSceneManager.
[InitializeOnLoad]
public class SceneOpenedHandler
{
    static SceneOpenedHandler()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        // Determine the owner status based on the scene name
        bool ownerStatus = false;
        if (scene.name == "HL_Hand_Streaming")
        {
            ownerStatus = true;
        }
        else if (scene.name == "PC_Hand_Streaming")
        {
            ownerStatus = false;
        }

        // Load prefabs from the Resources folder
        var prefab1 = Resources.Load<GameObject>("openxr_left_hand");
        var prefab2 = Resources.Load<GameObject>("openxr_left_hand 1");

        // Process each loaded prefab
        ProcessPrefab(prefab1, ownerStatus);
        ProcessPrefab(prefab2, ownerStatus);
    }

    // This method processes a single prefab, finding all Owner components and setting their owner flag
    private static void ProcessPrefab(GameObject prefab, bool ownerStatus)
    {
        if (prefab != null)
        {
            // Get all Owner components in the prefab, including those on the prefab root
            Owner[] allOwners = prefab.GetComponentsInChildren<Owner>(true);
            foreach (Owner owner in allOwners)
            {
                owner.owner = ownerStatus;
            }

            // Mark the prefab as dirty and save the changes to it
            EditorUtility.SetDirty(prefab);
            PrefabUtility.SavePrefabAsset(prefab);
        }
        else
        {
            Debug.LogError("Prefab not found in Resources!");
        }
    }
}
