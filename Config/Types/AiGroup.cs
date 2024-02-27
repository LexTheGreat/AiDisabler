using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using Newtonsoft.Json;

namespace AiDisabler.Config.Types {
    public class AiGroup : Ai {
        public List<Ai> Children { get; set; }
        public AiGroup() {

            this.Children = new List<Ai>();
        }
        public void printTest() {
            foreach (Ai child in Children) {
                if (child is AiGroup) {
                    Plugin.Log.LogInfo($"AiGroup: {child.ID} {child.Name} {child.TypeId} {child.ClassId} {child.IsDisabled} {child.IsKilled}");
                } else if (child is Ai) {
                    Plugin.Log.LogInfo($"Ai: {child.ID} {child.Name} {child.TypeId} {child.ClassId} {child.IsDisabled} {child.IsKilled}");
                }
            }
        }
        public string toJson() {
            return JsonConvert.SerializeObject(Children, Formatting.Indented);
        }
    }
}