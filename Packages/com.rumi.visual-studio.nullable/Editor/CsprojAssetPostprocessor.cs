using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEditorInternal;

namespace Rumi.VisualStudio.Nullable.Editor
{
    sealed class CsprojAssetPostprocessor : AssetPostprocessor
    {
        static bool OnPreGeneratingCSProjectFiles()
        {
            try
            {
                VisualStudioEditorPackagePatches.Patch();
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }

            nullableAssemblyList.Clear();

            string[] guids = AssetDatabase.FindAssets("t:asmdef");
            for (int i = 0; i < guids.Length; i++)
            {
                AssemblyDefinitionAsset asset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));
                string assetPath = AssetDatabase.GetAssetPath(asset);
                assetPath = Path.GetDirectoryName(assetPath);
                assetPath = Path.Combine(assetPath, "csc.rsp");

                if (File.Exists(assetPath))
                {
                    string cscContent = File.ReadAllText(assetPath).ToLower();
                    if (cscContent.Contains("-nullable:enable"))
                        nullableAssemblyList.Add(asset.name);
                }
            }

            return false;
        }

        static readonly List<string> nullableAssemblyList = new List<string>();

        static string OnGeneratedCSProject(string path, string content)
        {
            if (CsprojSettings.instance.enableNullable && nullableAssemblyList.Contains(Path.GetFileNameWithoutExtension(path)))
            {
                XDocument doc = XDocument.Parse(content);
                IEnumerable<XElement>? propertyGroup = doc.Element("Project")?.Elements("PropertyGroup");
                if (propertyGroup != null)
                {
                    foreach (var item in propertyGroup)
                        item.SetElementValue(XName.Get("Nullable"), "enable");

                    content = doc.ToString();
                }
            }

            return content;
        }
    }
}
