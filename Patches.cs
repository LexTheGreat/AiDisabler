
using AiDisabler.Config.Types;
using HarmonyLib;
using Sons.Ai.Vail;

namespace AiDisabler.Patches {
    public class Patcher {
        public static void DoPatching() {
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }

    }

    [HarmonyPatch(typeof(VailActor), nameof(VailActor.Awake))]
    class VailActorAwakePatch {
        public static void Postfix(ref VailActor __instance) {
            if (Plugin.configPrintAi.Value) {
                Plugin.Log.LogInfo("================VailActor.Awake===================== (Ai Is About To Spawn)");
                Plugin.Log.LogInfo("AI.Name: " + __instance.name);
                Plugin.Log.LogInfo("AI.Group: " + __instance._activeGroup);
                Plugin.Log.LogInfo("AI.ClassId: " + __instance.ClassId);
                Plugin.Log.LogInfo("AI.TypeId: " + __instance.TypeId);
                Plugin.Log.LogInfo("AI.UniqueId: " + __instance.UniqueId);
                Plugin.Log.LogInfo("AI.FamilyId: " + __instance.FamilyId);
            }
            if (Plugin.disablerConfig.CheckDisabled(new Ai() { TypeId = $"{__instance.TypeId}", ClassId = $"{__instance.ClassId}", Name = $"{__instance.name}" }, out string disablerId)) {
                UnityEngine.Object.Destroy(__instance.transform.gameObject);
                if (Plugin.configPrintAi.Value)
                    Plugin.Log.LogInfo($"AI.IsDisabled: true + {disablerId}");
            } else {
                if (Plugin.configPrintAi.Value)
                    Plugin.Log.LogInfo("AI.IsDisabled: false");
            }
        }
    }

    [HarmonyPatch(typeof(VailActor), nameof(VailActor.Start))]
    class VailActorStartPatch {
        public static void Postfix(ref VailActor __instance) {
            if (Plugin.configPrintAi.Value) {
                Plugin.Log.LogInfo("================VailActor.Start===================== (Ai is alive/Near player)");
                Plugin.Log.LogInfo("AI.Name: " + __instance.name);
                Plugin.Log.LogInfo("AI.Group: " + __instance._activeGroup);
                Plugin.Log.LogInfo("AI.ClassId: " + __instance.ClassId);
                Plugin.Log.LogInfo("AI.TypeId: " + __instance.TypeId);
                Plugin.Log.LogInfo("AI.UniqueId: " + __instance.UniqueId);
                Plugin.Log.LogInfo("AI.FamilyId: " + __instance.FamilyId);
            }

            if (Plugin.disablerConfig.CheckKilled(new Ai() { TypeId = $"{__instance.TypeId}", ClassId = $"{__instance.ClassId}", Name = $"{__instance.name}" }, out string killerId)) {
                if (__instance._damageEnabled) {
                    if (__instance.GetHealth() > 0) {
                        __instance.DrainHealth(__instance.GetHealth());
                    }
                }
                if (Plugin.configPrintAi.Value)
                    Plugin.Log.LogInfo($"AI.IsKilled: true + {killerId}");
            } else {
                if (Plugin.configPrintAi.Value)
                    Plugin.Log.LogInfo("AI.IsKilled: false");
            }
        }
    }
}