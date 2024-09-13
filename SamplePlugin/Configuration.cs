using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SamplePlugin 
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;
        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;
        public float FontSize { get; internal set; }

        // The below exist just to make saving less cumbersome
        public void Save()
        {
            Plugin.PluginInterface.SavePluginConfig(this);
        }
    }
}
