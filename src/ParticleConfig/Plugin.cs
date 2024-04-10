using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace ParticleConfig
{
    [BepInPlugin(PluginId, "ParticleConfig", "1.0.0")]
    public class ParticleConfigPlugin : BaseUnityPlugin
    {
        public const string PluginId = "patricnox.valheim.ParticleConfig";
        private Harmony _harmony;
        private static ParticleConfigPlugin _instance;
        public static ManualLogSource p = BepInEx.Logging.Logger.CreateLogSource(PluginId);
        private Dictionary<string, ConfigEntry<bool>> settings = new Dictionary<string, ConfigEntry<bool>>();
        private static List<string> disabledParticles = new List<string>();

        [UsedImplicitly]
        private void Awake()
        {
            _instance = this;
            settings = PluginConfig.Setup(Config);
            AddDisabledParticales();
            _harmony = Harmony.CreateAndPatchAll(typeof(ParticleConfigPlugin).Assembly, PluginId);
            p.LogInfo("[ParticleConfig] Started");
        }

        void AddDisabledParticales()
        {
            foreach (var kvp in settings)
            {
                if (kvp.Value.Value == false) disabledParticles.Add(kvp.Key);
            }
        }

        [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.SetParticleArrayEnabled))]
        private static class SetParticleArrayEnabled_Patch
        {
            private static bool Prefix(GameObject[] psystems, bool enabled)
            {
                foreach (GameObject gameObject in psystems)
                {
                    ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
                    foreach (var ps in componentsInChildren)
                    {
                        var emission = ps.emission;
                        if (ParticleConfigPlugin.IsParticleDisabled(ps.name)) emission.enabled = false;
                        else emission.enabled = enabled;
                    }

                    MistEmitter componentInChildren = gameObject.GetComponentInChildren<MistEmitter>();
                    if (componentInChildren) componentInChildren.enabled = enabled;
                }

                return false;
            }
        }
        private static bool IsParticleDisabled(string settingName)
        {
            return disabledParticles.Contains(settingName);
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
