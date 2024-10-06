using System.Reflection;
using System;
using System.Linq;
using HarmonyLib;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;

namespace Rumi.VisualStudio.Nullable.Editor
{
    [InitializeOnLoad]
    sealed class VisualStudioEditorPackagePatches
    {
        static VisualStudioEditorPackagePatches() => Patch();

        static readonly Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().First(static x => x.GetName().Name == "Unity.VisualStudio.Editor");

#if UNITY_EDITOR_WIN
        static readonly Type type = assembly.GetType("Microsoft.Unity.VisualStudio.Editor.VisualStudioForWindowsInstallation");
#elif UNITY_EDITOR_OSX
        static readonly Type type = assembly.GetType("Microsoft.Unity.VisualStudio.Editor.VisualStudioForMacInstallation");
#endif

        static readonly Type legacyType = assembly.GetType("Microsoft.Unity.VisualStudio.Editor.LegacyStyleProjectGeneration");
        static readonly Type sdkType = assembly.GetType("Microsoft.Unity.VisualStudio.Editor.SdkStyleProjectGeneration");

        static readonly IGenerator legacy = (IGenerator)Activator.CreateInstance(legacyType);
        static readonly IGenerator sdk = (IGenerator)Activator.CreateInstance(sdkType);

        static readonly Harmony harmony = new Harmony("Rumi.VisualStudio.Nullable.Editor");
        
        static bool isPathed = false;
        public static void Patch()
        {
            if (isPathed)
                return;

            isPathed = true;

            try
            {
                MethodInfo? org = AccessTools.PropertyGetter(type, "ProjectGenerator");
                MethodInfo? post = typeof(VisualStudioEditorPackagePatches).GetMethod("Post", BindingFlags.NonPublic | BindingFlags.Static);

                if (org == null)
                {
                    UnityEngine.Debug.LogError("Patch failed!");
                    UnityEngine.Debug.LogError($"{type.Name}.ProjectGenerator property does not exist\nThis means that the Visual Studio Editor package has been modified.\nPlease upload the issue with the Unity version and the package version.");

                    return;
                }
                else if (post == null)
                {
                    UnityEngine.Debug.LogError("Patch failed!");
                    UnityEngine.Debug.LogError($"{typeof(VisualStudioEditorPackagePatches).Name}.{nameof(Post)}({nameof(IGenerator)}) method does not exist");

                    return;
                }

                harmony.Patch(org, null, new HarmonyMethod(post));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Patch failed!");
                UnityEngine.Debug.LogException(e);
            }

            if (CsprojSettingAssets.instance.enableLog)
                UnityEngine.Debug.Log($"[{nameof(VisualStudioEditorPackagePatches)}] Patch success!");
        }

        static void Post(ref IGenerator __result)
        {
            if (CsprojSettingAssets.instance.enableSDKStyle)
                __result = sdk;
            else
                __result = legacy;
        }
    }
}