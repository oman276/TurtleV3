using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PrefabReplacer : EditorWindow
{
    private GameObject prefab;

    [MenuItem("Tools/Prefab Replacer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabReplacer>("Prefab Replacer");
    }

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace"))
        {
            ReplaceSelectedObjectsWithPrefab();
        }
    }

    void ReplaceSelectedObjectsWithPrefab()
    {
        if (prefab == null)
        {
            Debug.LogError("No prefab selected!");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject obj in selectedObjects)
        {
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            //newObj.transform.SetParent(obj.transform.parent);

            // Match the world position and rotation
            newObj.transform.position = obj.transform.position;
            newObj.transform.rotation = obj.transform.rotation;
            // Explicitly setting scale to match the original object's world scale
            //AdjustScaleToMatchWorldScale(newObj, obj);

            // Calculate and set the local scale to match the world scale of the original object
            //Vector3 worldScale = GetWorldScale(obj.transform);
            /*
            newObj.transform.localScale = new Vector3(worldScale.x / newObj.transform.lossyScale.x,
                                                      worldScale.y / newObj.transform.lossyScale.y,
                                                      worldScale.z / newObj.transform.lossyScale.z);
            */

            newObj.name = obj.name;
            DestroyImmediate(obj);
        }
    }

    Vector3 GetWorldScale(Transform transform)
    {
        Vector3 worldScale = transform.localScale;
        Transform parent = transform.parent;

        while (parent != null)
        {
            worldScale = Vector3.Scale(worldScale, parent.localScale);
            parent = parent.parent;
        }

        return worldScale;
    }

    void AdjustScaleToMatchWorldScale(GameObject newObj, GameObject originalObj)
    {
        Transform newObjTransform = newObj.transform;
        Transform originalTransform = originalObj.transform;

        Vector3 originalWorldScale = originalTransform.lossyScale;
        Vector3 scaleRatio = new Vector3(
            newObjTransform.lossyScale.x/ originalWorldScale.x ,
            newObjTransform.lossyScale.y/ originalWorldScale.y,
            newObjTransform.lossyScale.z/ originalWorldScale.z);

        newObjTransform.localScale = Vector3.Scale(newObjTransform.localScale, scaleRatio);
    }
}