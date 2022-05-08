using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEditor.SceneManagement;


namespace Editor
{
    [CustomEditor(typeof(LevelsConfig))]
    class LevelsConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Selected Scenes To Last Pack"))
            {
                var config = target as LevelsConfig;

                var lastPack = config.packs.Last();


                var sceneGuids = Selection.assetGUIDs.Where(guid =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    return path != null && AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(SceneAsset);
                });

                var newScenes = sceneGuids
                    .Select(guid => new EditorBuildSettingsScene(guid, enabled: true))
                    .Where(scene => !EditorBuildSettings.scenes.Contains(scene));

                EditorBuildSettings.scenes = EditorBuildSettings.scenes
                    .Concat(newScenes)
                    .ToArray();

                List<Level> newLevels = sceneGuids
                    .Select(guid => {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        var scene = SceneManager.GetSceneByPath(path);
                        var isLoaded = scene.isLoaded;
                        if (!isLoaded)
                        {
                            scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                        }

                        var totalStars = scene.GetRootGameObjects()
                            .SelectMany(go => go.GetComponentsInChildren<StarComponent>())
                            .Count();

                        var level = new Level { sceneName = scene.name, totalStars = totalStars };

                        if (!isLoaded)
                        {
                            EditorSceneManager.CloseScene(scene, removeScene: true);
                        }

                        return level;
                    })
                    .ToList();

                lastPack.levels.AddRange(newLevels);
            }
        }
    }
}
