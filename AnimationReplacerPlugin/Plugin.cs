using OldTarkovMovement;
using OldTarkovMovement.Patches;
using BepInEx;
using System;

namespace OldTarkovMovement
{
    [BepInPlugin("com.boogle.oldtarkovmovement", "Old Tarkov Movement", "1.0.3")]
    public class Plugin : BaseUnityPlugin
    {
        public static bool IsForModern = true;
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

                    if (!IsForModern)
                    {
                        try
                        {
                            new FikaStatePatch().Enable();
                        }
                        catch (Exception e)
                        {
                            Logger.LogInfo("Safe to assume the user doesn't have Fika");
                        }
                    } else
                    {
                        Logger.LogInfo("Skipping Fika patch because they are on modern variant.");
                    }
                }

                //new BlindfireWhileRunning().Enable();

                new TripwireInteractionPatch().Enable();
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
