using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabMaker : EditorWindow
{
    public static GameObject pickupPrefabTemplate;
    public ItemObject[] items;
    public static string pickupFolder;

    [MenuItem("MyWindow/PrefabMaker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PrefabMaker));
    }

    void OnGUI()
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty itemsArray = so.FindProperty("items");

        pickupPrefabTemplate = (GameObject)EditorGUILayout.ObjectField("Pickup prefab template", pickupPrefabTemplate, typeof(GameObject), false);
        pickupFolder = EditorGUILayout.TextField("Save path", pickupFolder);
        EditorGUILayout.PropertyField(itemsArray, true);

        so.ApplyModifiedProperties();

        if (GUILayout.Button("Make pickup prefabs"))
        {
            CreatePickupPrefabs();
        }
    }

    void CreatePickupPrefabs()
    {
        if (items == null || pickupPrefabTemplate == null) return;

        foreach (ItemObject item in items)
        {
            string path = pickupFolder + item.DisplayName + "_Pickup" + ".prefab";
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            GameObject pickupPrefab = (GameObject)PrefabUtility.InstantiatePrefab(pickupPrefabTemplate);

            PickupScript pickupScript = pickupPrefab.GetComponent<PickupScript>();
            pickupScript.SetupPickup(item, false);

            SavePickup saveScript = pickupPrefab.GetComponent<SavePickup>();
            saveScript.AssignId();

            PrefabUtility.SaveAsPrefabAssetAndConnect(pickupPrefab, path, InteractionMode.UserAction);
            DestroyImmediate(pickupPrefab);
        }

        items = null;
    }
}
