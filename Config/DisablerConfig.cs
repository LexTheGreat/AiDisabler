using System;
using System.Collections.Generic;
using System.IO;
using AiDisabler.Config.Types;
using BepInEx;
using Newtonsoft.Json;

namespace AiDisabler.Config {
    public class DisablerConfig {
        public string configPath = Path.Combine(Paths.ConfigPath, "AiDisabler.json");
        public AiGroup rootGroup;
        public bool GlobalDisabled = false;
        public bool GlobalKilled = false;
        private List<Ai> CachedDisabled = new List<Ai>();
        private List<Ai> CachedKilled = new List<Ai>();
        public DisablerConfig() {
            LoadConfig();
            CacheConfig(rootGroup, true);
            if (Plugin.configPrintAi.Value) {
                Plugin.Log.LogInfo("=================CachedDisabled=================");
                foreach (Ai ai in CachedDisabled) {
                    Plugin.Log.LogInfo($"CachedDisabled: (ID:{ai.ID} NAM:{ai.Name} TID:{ai.TypeId} CID:{ai.ClassId}) -> {ai.IsDisabled}");
                }
                Plugin.Log.LogInfo("=================CachedKilled===================");
                foreach (Ai ai in CachedKilled) {
                    Plugin.Log.LogInfo($"CachedKilled: (ID:{ai.ID} NAM:{ai.Name} TID:{ai.TypeId} CID:{ai.ClassId}) -> {ai.IsKilled}");
                }
                Plugin.Log.LogInfo("=================MatchTest======================");
                List<Ai> testList = new List<Ai>() {
                    new Ai() { TypeId = "egal", ClassId = "Animal" },
                    new Ai() { TypeId = "mrpuffton", ClassId = "Creepy" },
                    new Ai() { TypeId = "twins", ClassId = "Creepy", Name = "Twins" }
                };

                foreach (Ai ai in testList) {
                    Plugin.Log.LogInfo($"{ai.TypeId}? -> {CachedDisabled.Contains(ai)} {CachedKilled.Contains(ai)}");
                    bool one = this.CheckDisabled(ai, out string Disabler);
                    bool two = this.CheckKilled(ai, out string Killer);
                    Plugin.Log.LogInfo($"{ai.TypeId}?? -> {Disabler} {Killer}");
                }
                Plugin.Log.LogInfo("=================EndTest========================");
            }
        }

        private void LoadConfig() {
            rootGroup = new AiGroup();
            Plugin.Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is doing config.");
            if (File.Exists(configPath)) {
                try {
                    rootGroup = JsonConvert.DeserializeObject<AiGroup>(File.ReadAllText(configPath), new AiConverter());
                    Plugin.Log.LogInfo("AiDisabler.json loaded.");
                } catch (Exception e) {
                    Plugin.Log.LogError($"Delete AiDisabler.json and restart the game to fix this issue! Or fix your json errors.");
                    Plugin.Log.LogError($"Error loading AiDisabler.json: {e}");
                }
            } else {
                Plugin.Log.LogError("AiDisabler.json not found. Please copy over the AiDisabler.json from the zip file to the BepInEx/config folder.");
                return;
            }
            Plugin.Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is done doing config!");
        }

        private void CacheConfig(AiGroup aiGroup, bool isRoot = false) {
            if (isRoot) {
                Plugin.Log.LogDebug($"===Cache'n Root -> ID:{aiGroup.ID} NAM:{aiGroup.Name} TID:{aiGroup.TypeId} CID:{aiGroup.ClassId} IDS:{aiGroup.IsDisabled} IKL:{aiGroup.IsKilled}");
                if (aiGroup.IsDisabled != null)
                    CachedDisabled.Add(aiGroup);
                if (aiGroup.IsKilled != null)
                    CachedKilled.Add(aiGroup);
            }

            foreach (Ai child in aiGroup.Children) {
                if ((aiGroup.ID != null || aiGroup.ID != "") && (child.ID == null || child.ID == "")) {
                    child.ID = $"{aiGroup.ID}.|{child.Name}-{child.TypeId}-{child.ClassId}|";
                    Plugin.Log.LogDebug($"-^Parrent {aiGroup.ID} has override'n Child's ID to {child.ID} due to missing ID!");
                }


                Plugin.Log.LogDebug($"+Cache'n AI: ID:{child.ID} NAM:{child.Name} TID:{child.TypeId} CID:{child.ClassId} IDS:{child.IsDisabled} IKL:{child.IsKilled}");

                if (CachedDisabled.Contains(child)) {
                    Plugin.Log.LogDebug($"-^CachedDisabled of ID:{child.ID} NAM:{child.Name} TID:{child.TypeId} CID:{child.ClassId} IDS:{child.IsDisabled} IKL:{child.IsKilled} existing, overriding...");
                    CachedDisabled.Remove(child);
                }
                if (CachedKilled.Contains(child)) {
                    Plugin.Log.LogDebug($"-^CachedKilled of ID:{child.ID} NAM:{child.Name} TID:{child.TypeId} CID:{child.ClassId} IDS:{child.IsDisabled} IKL:{child.IsKilled} existing, overriding...");
                    CachedKilled.Remove(child);
                }

                if (aiGroup.IsDisabled != null && child.IsDisabled == null) {
                    child.IsDisabled = aiGroup.IsDisabled;
                    CachedDisabled.Add(child);
                    Plugin.Log.LogDebug($"-^Parrent {aiGroup.ID} has override'n Child's IsDisabled to {child.IsDisabled}");
                } else {
                    if (child is AiGroup) {
                        if (child.Name != null || child.TypeId != null || child.ClassId != null)
                            CachedDisabled.Add(child);
                    } else {
                        Plugin.Log.LogDebug($"+ Added CachedDisabled (ID:{child.ID} NAM:{child.Name} TID:{child.TypeId} CID:{child.ClassId}) -> {child.IsKilled}");
                        CachedDisabled.Add(child);
                    }
                }
                if (aiGroup.IsKilled != null && child.IsKilled == null) {
                    child.IsKilled = aiGroup.IsKilled;
                    CachedKilled.Add(child);
                    Plugin.Log.LogDebug($"-^Parrent {aiGroup.ID} has override'n Child's IsKilled to {child.IsKilled}");
                } else {
                    if (child is AiGroup) {
                        if (child.Name != null || child.TypeId != null || child.ClassId != null)
                            CachedKilled.Add(child);
                    } else {
                        Plugin.Log.LogDebug($"+ Added CachedKilled (ID:{child.ID} NAM:{child.Name} TID:{child.TypeId} CID:{child.ClassId}) -> {child.IsKilled}");
                        CachedKilled.Add(child);
                    }
                }

                if (child is AiGroup) {
                    Plugin.Log.LogDebug($"+==Cache'n AiGroup -> ID:{child.ID} NAM:{child.Name} TID:{child.TypeId} CID:{child.ClassId} IDS:{child.IsDisabled} IKL:{child.IsKilled}");
                    CacheConfig((AiGroup)child);
                    Plugin.Log.LogDebug($"+==Cache'n AiGroup: {child.ID} finished!");
                }
                Plugin.Log.LogDebug($"===Cache'n Root with id {aiGroup.ID}, finished!");
            }
        }

        public bool CheckDisabled(Ai ai, out string idOfDisabler) {
            for (int i = CachedDisabled.ToArray().Length; i-- > 0;) {
                if (CachedDisabled[i].Equals(ai)) {
                    idOfDisabler = CachedDisabled[i].ID;
                    return (bool)CachedDisabled[i].IsDisabled;
                }
            }
            idOfDisabler = "";
            return false;
        }

        public bool CheckKilled(Ai ai, out string idOfKiller) {
            for (int i = CachedKilled.ToArray().Length; i-- > 0;) {
                if (CachedKilled[i].Equals(ai)) {
                    idOfKiller = CachedKilled[i].ID;
                    return (bool)CachedDisabled[i].IsKilled;
                }
            }
            idOfKiller = "";
            return false;
        }
    }
}