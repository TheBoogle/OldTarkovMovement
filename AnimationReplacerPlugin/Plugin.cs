using OldTarkovMovement;
using OldTarkovMovement.Patches;
using BepInEx;
using System;
using BepInEx.Bootstrap;
using EFT;
using System.Threading.Tasks;

namespace OldTarkovMovement
{
    [BepInPlugin("com.boogle.oldtarkovmovement", "Old Tarkov Movement", "1.0.6")]
    public class Plugin : BaseUnityPlugin
    {
        public static bool IsForModern = false;
        
        public void Awake()
        {
            if (IsForModern)
            {
                Logger.LogInfo("Using Modern Variant");
            }
            else
            {
                Logger.LogInfo("Using Old Variant");
            }

            try
            {
                new StateReplacer().Enable();
                new FixForSmoothMotherfuckingSpeed().Enable();
                new BushSpeedStateRemover().Enable();
                new OldTiltingFix().Enable();

                if (!IsForModern)
                {
                    new ProceduralBlindfire().Enable();
                    new SendHandsInteractionStateChangedPatch().Enable();
                    new SetInteractInHandsPatch().Enable();
                    new DisableFancyInteractions().Enable();
                    new DropbackpackFix().Enable();

                    DelayFikaLoad();
                }

                //new BlindfireWhileRunning().Enable();

                //new TripwireInteractionPatch().Enable();
                //new RepairObjectivePatch().Enable();
                //new BeaconPlacePatch().Enable();
                //new ExfilInteractPatch().Enable();
                //new PreSprintAccelerationFix().Enable();
            }
            catch (Exception ex)
            {
                Logger.LogError($"A PATCH IN {GetType().Name} FAILED. SUBSEQUENT PATCHES HAVE NOT LOADED");
                Logger.LogError($"{GetType().Name}: {ex}");
                throw;
            }

            Logger.LogInfo("Completed: Old Tarkov Movement");
        }

        private async void DelayFikaLoad()
        {
            await Task.Delay(5 * 1000);
            
            bool FikaLoaded = Chainloader.PluginInfos.ContainsKey("com.fika.core");

            Logger.LogInfo($"Fika Loaded: {FikaLoaded}");

            if (FikaLoaded) // Fika patch
            {
                new FikaStateEnterPatch().Enable();
                new FikaStateExitPatch().Enable();
            }
            else
            {
                Logger.LogInfo("Skipping Fika patch because Fika is not loaded.");
            }
        }
    }
}
