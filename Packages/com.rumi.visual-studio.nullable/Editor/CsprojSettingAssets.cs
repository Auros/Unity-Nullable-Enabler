using UnityEditor;
using UnityEngine;

namespace Rumi.VisualStudio.Nullable.Editor
{
    public sealed class CsprojSettingAssets : ScriptableObject
    {
        public const string assetPath = "Assets/" + nameof(CsprojSettingAssets) + ".asset";

        public static CsprojSettingAssets instance
        {
            get
            {
                _instance = AssetDatabase.LoadAssetAtPath<CsprojSettingAssets>(assetPath);
                if (_instance == null)
                {
                    _instance = CreateInstance<CsprojSettingAssets>();

                    AssetDatabase.CreateAsset(_instance, assetPath);
                    AssetDatabase.SaveAssets();
                }

                return _instance;
            }
        }
        static CsprojSettingAssets? _instance;

        public bool enableSDKStyle { get => _enableSDKStyle; set => _enableSDKStyle = value; }
        [SerializeField] bool _enableSDKStyle = true;

        public bool enableNullable { get => _enableNullable; set => _enableNullable = value; }
        [SerializeField] bool _enableNullable = true;

        public bool enableLog { get => _enableLog; set => _enableLog = value; }
        [SerializeField] bool _enableLog = true;
    }
}
