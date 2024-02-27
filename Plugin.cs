using AiDisabler.Config;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Core.Logging.Interpolation;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace AiDisabler {
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin {
        internal new static ManualLogSource Log;
        static public DisablerConfig disablerConfig;
        static public ConfigEntry<bool> configPrintAi;

        public override void Load() {
            Log = base.Log;
            configPrintAi = Config.Bind("General.Toggles", "PrintOutAi", true, "Whether to print out the Ai when it's loaded/spawned.");
            disablerConfig = new DisablerConfig();
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            //InjectInGameObject();
            Patches.Patcher.DoPatching();
        }

        private void InjectInGameObject() {
            bool isEnabled = false;
            try {
                ClassInjector.RegisterTypeInIl2Cpp<AiDisabler.AiDisablerBehaviour>();
                GameObject val = new GameObject("AiDisabler");
                val.AddComponent<AiDisabler.AiDisablerBehaviour>();
                ((UnityEngine.Object)val).hideFlags = (HideFlags)61;
                UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)val);
            } catch {
                BepInExErrorLogInterpolatedStringHandler val2 = new BepInExErrorLogInterpolatedStringHandler(48, 0, out isEnabled);
                if (isEnabled) {
                    ((BepInExLogInterpolatedStringHandler)val2).AppendLiteral("FAILED to Register Il2Cpp Type: AiDisablerBehaviour!");
                }
                Log.LogError(val2);
            }
        }
    }
}