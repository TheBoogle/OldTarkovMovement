using OldTarkovMovement;
using OldTarkovMovement.Patches;
using BepInEx;
using System;

namespace OldTarkovMovement
{
    [BepInPlugin("com.boogle.oldtarkovmovement", "Old Tarkov Movement", "1.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static bool IsForModern = true;
        public void Awake()
        {
            Logger.LogInfo("Loading: Old Tarkov Movement");

            try
            {
                new StateReplacer().Enable();
                new FixForSmoothMotherfuckingSpeed().Enable();
                new BushSpeedStateRemover().Enable();
                new OldTiltingFix().Enable();
                if (!IsForModern)
                {
                    new ProceduralBlindfire().Enable();
                    //new BlindfireWhileRunning().Enable();
                }

                new RepairObjectivePatch().Enable();
                new BeaconPlacePatch().Enable();
                new ExfilInteractPatch().Enable();
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
    }
}
