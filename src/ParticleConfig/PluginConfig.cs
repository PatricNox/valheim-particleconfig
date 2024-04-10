using BepInEx.Configuration;

namespace ParticleConfig
{
    internal class PluginConfig
    {
        private static string[] settingNames = new string[]
        {
            "FogClouds",
            "cloud",
            "snow",
            "vfx_ground_fog",
            "fog_ball",
            "air dust",
            "splash spawner",
            "splash",
            "RainTest",
            "InfectedMine",
            "dust particles",
            "mist",
            "cloud (1)",
            "fastmoving_wetmist",
            "balls",
            "zinder",
            "Rain",
            "distant_rain",
            "snow (1)",
            "distant snow",
            "Whirl",
            "ash",
            "InteriorDust",
            "interior_dust (1)"
        };

        public static Dictionary<string, ConfigEntry<bool>> Setup(ConfigFile config)
        {
            Dictionary<string, ConfigEntry<bool>> settings = new Dictionary<string, ConfigEntry<bool>>();

            foreach (string settingName in settingNames)
            {
                settings[settingName] = config.Bind("Particles", settingName, true, "");
            }

            return settings;
        }
    }
}
