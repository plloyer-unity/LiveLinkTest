using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestScript), true)]
public class TestScriptEditor : Editor
{
    TestScript Target => target as TestScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Target.enabled)
            return;

        if (GUILayout.Button("SpawnGameObjectAndPrefab"))
        {
            Target.SpawnGameObjectAndPrefab();
        }
    }
}
