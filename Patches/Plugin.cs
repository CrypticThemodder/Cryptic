using BepInEx;
using System.ComponentModel;

namespace Cryptic.Patches
{
    [Description(Cryptic.PluginInfo.Description)]
    [BepInPlugin(Cryptic.PluginInfo.GUID, Cryptic.PluginInfo.Name, Cryptic.PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        private void OnEnable()
        {
            Menu.ApplyHarmonyPatches();
        }

        private void OnDisable()
        {
            Menu.RemoveHarmonyPatches();
        }
    }
}
