using AnimationReplacer;
using BepInEx;
using System;

namespace YourNamespace
{
    [BepInPlugin("com.boogle.animationreplacer.blockfirearmonsprint", "12.11AnimationReplacer", "1.0.2")]
    public class SprintMod : BaseUnityPlugin
    {
        public void Awake()
        {
            Logger.LogInfo("Loading: Animation Replacer");

            try
            {
                new EnableSprintPatch().Enable();
            }
            catch (Exception ex)
            {
                Logger.LogError($"A PATCH IN {GetType().Name} FAILED. SUBSEQUENT PATCHES HAVE NOT LOADED");
                Logger.LogError($"{GetType().Name}: {ex}");
                throw;
            }

            Logger.LogInfo("Completed: Animation Replacer");
        }
    }
}
