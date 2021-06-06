using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game.Terrain.Active;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class ObjectSpawnerWindow : EditorWindow
{
    private static GameObject _so;
    /// <summary>
    /// Object that will be spawned upon pressing a button.
    /// </summary>
    private GameObject _spawnedObjectPrefab;
    /// <summary>
    /// Found jump plants.
    /// </summary>
    private int _jumpPlantsFound;
    /// <summary>
    /// Gets this window.
    /// </summary>
    [MenuItem("Window/GONInspector/ObjectSpawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ObjectSpawnerWindow));
    }
    private void OnGUI()
    {
        GUILayout.Label("Object Spawner", EditorStyles.boldLabel);

        _spawnedObjectPrefab = EditorGUILayout.ObjectField("Prefab", _spawnedObjectPrefab, typeof(GameObject), false) as GameObject;
        _jumpPlantsFound =
            EditorGUILayout.IntField("Found jump plant refs", _jumpPlantsFound);

        if (GUILayout.Button("Spawn Object"))
        {
            Instantiate(_spawnedObjectPrefab);
        }

        if (GUILayout.Button("Find jump plants"))
        {
            FindJumpPlants();
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void FindJumpPlants()
    {
        _jumpPlantsFound = FindObjectsOfType<JumpPlant>().Length;
    }
}

//TODO: Finding assets https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html