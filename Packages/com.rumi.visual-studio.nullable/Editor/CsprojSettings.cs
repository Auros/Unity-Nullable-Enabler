using System;
using UnityEditor;
using UnityEngine;

namespace Rumi.VisualStudio.Nullable.Editor
{
    public sealed class CsprojSettings : ScriptableObject
    {
        public const string assetPath = "Assets/" + nameof(CsprojSettings) + ".asset";

        public static CsprojSettings instance
        {
            get
            {
                try
                {
                    _instance = AssetDatabase.LoadAssetAtPath<CsprojSettings>(assetPath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                if (_instance == null)
                {
                    _instance = CreateInstance<CsprojSettings>();

                    try
                    {
                        AssetDatabase.CreateAsset(_instance, assetPath);
                        AssetDatabase.SaveAssets();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                return _instance;
            }
        }
        static CsprojSettings? _instance;

        public bool enableSDKStyle { get => _enableSDKStyle; set => _enableSDKStyle = value; }
        [SerializeField] bool _enableSDKStyle = true;

        public bool enableNullable { get => _enableNullable; set => _enableNullable = value; }
        [SerializeField] bool _enableNullable = true;

        public bool enableLog { get => _enableLog; set => _enableLog = value; }
        [SerializeField] bool _enableLog = true;
    }
}
