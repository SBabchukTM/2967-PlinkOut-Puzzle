#if UNITY_EDITOR  
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Editor.TMPReplacer
{
    public class ReplaceTMPFontTool : EditorWindow
    {
        private TMP_FontAsset _newFontAsset;
        private string _searchPath = "Assets";

        [MenuItem("Tools/Replace TMP Font Tool")]
        public static void ShowWindow() =>
                GetWindow<ReplaceTMPFontTool>("Replace TMP Font");

        private void OnGUI()
        {
            GUILayout.Label("Replace TMP Font in Prefabs", EditorStyles.boldLabel);

            _newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", _newFontAsset, typeof(TMP_FontAsset), false);

            _searchPath = EditorGUILayout.TextField("Search Path", _searchPath);

            if (!GUILayout.Button("Replace Font in Prefabs"))
                return;

            if (!_newFontAsset)
            {
                Debug.LogError("Please select a new TMP Font Asset.");

                return;
            }

            ReplaceFontInPrefabs();
        }

        private void ReplaceFontInPrefabs()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab", new[]
            {
                _searchPath
            });

            var replacedCount = 0;

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                var textComponents = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
                var hasChanges = false;

                foreach (var textComponent in textComponents)
                    if (textComponent.font != _newFontAsset)
                    {
                        textComponent.font = _newFontAsset;
                        hasChanges = true;
                    }

                if (hasChanges)
                {
                    EditorUtility.SetDirty(prefab);
                    replacedCount++;
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"Replaced TMP Font in {replacedCount} prefabs.");
        }
    }
}
#endif