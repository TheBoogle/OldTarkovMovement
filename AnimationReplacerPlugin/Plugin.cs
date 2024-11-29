using OldTarkovMovement;
using OldTarkovMovement.Patches;
using BepInEx;
using System;

namespace OldTarkovMovement
{
    [BepInPlugin("com.boogle.animationreplacer.blockfirearmonsprint", "12.11AnimationReplacer", "1.0.3")]
    public class OldTarkovMovement : BaseUnityPlugin
    {
        public void Awake()
        {
            Logger.LogInfo("Loading: Animation Replacer");

            try
            {
                new StateReplacer().Enable();
                new FixForSmoothMotherfuckingSpeed().Enable();
                new BushSpeedStateRemover().Enable();
                new OldTiltingFix().Enable();
                //new ProdceduralBlindfire().Enable();
                //new PreSprintAccelerationFix().Enable();
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
