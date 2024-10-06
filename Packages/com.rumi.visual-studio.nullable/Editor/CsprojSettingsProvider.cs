using UnityEditor;
using UnityEngine;

#if ENABLE_RUNI_ENGINE
using static RuniEngine.Editor.EditorTool;
#endif

namespace Rumi.VisualStudio.Nullable.Editor
{
    sealed class CsprojSettingsProvider : SettingsProvider
    {
        public CsprojSettingsProvider(string path, SettingsScope scopes) : base(path, scopes) { }

        static SettingsProvider? instance;
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider() => instance ??= new CsprojSettingsProvider("Runi Engine/Csproj", SettingsScope.Project)
        {
            keywords = new string[]
            {
                nameof(CsprojSettings.enableSDKStyle), nameof(CsprojSettings.enableNullable)
            }
        };

        SerializedObject? serializedObject;
        SerializedProperty? enableSDKStyle;
        SerializedProperty? enableNullable;
        SerializedProperty? enableLog;
        public override void OnGUI(string searchContext)
        {
            serializedObject ??= new SerializedObject(CsprojSettings.instance);

            enableSDKStyle ??= serializedObject.FindProperty("_enableSDKStyle");
            enableNullable ??= serializedObject.FindProperty("_enableNullable");
            enableLog ??= serializedObject.FindProperty("_enableLog");

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

#if ENABLE_RUNI_ENGINE
            EditorGUILayout.PropertyField(enableSDKStyle, new GUIContent(TryGetText("project_setting.csproj.sdk")));
            EditorGUI.BeginDisabledGroup(!enableSDKStyle.boolValue);
            EditorGUILayout.PropertyField(enableNullable, new GUIContent(TryGetText("project_setting.csproj.nullable"), TryGetText("project_setting.csproj.nullable.info")));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(enableLog, new GUIContent(TryGetText("project_setting.csproj.log")));

            EditorGUILayout.HelpBox(TryGetText("project_setting.csproj.warning"), MessageType.Warning);
#else
            EditorGUILayout.PropertyField(enableSDKStyle, new GUIContent("Enable SDK Style"));
            EditorGUI.BeginDisabledGroup(!enableSDKStyle.boolValue);
            EditorGUILayout.PropertyField(enableNullable, new GUIContent("Enable Nullable", "Enables Nullable in the project file with the same name as the assembly name that has -nullable enable written in the csc.rsp file."));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(enableLog, new GUIContent("Enable Log"));

            EditorGUILayout.HelpBox("The SDK project style forces the activation of the SdkStyleProjectGeneration class, which is written for the Visual Studio Code editor, and hooks into its internal code using Harmony 2.2.2.\nTo apply the changes, regenerate the project file in your preferences.", MessageType.Warning);
#endif

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}
