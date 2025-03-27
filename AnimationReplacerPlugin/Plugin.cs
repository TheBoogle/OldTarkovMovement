using OldTarkovMovement;
using OldTarkovMovement.Patches;
using BepInEx;
using System;
using BepInEx.Bootstrap;
using EFT;
using System.Threading.Tasks;
using SPT.Common.Http;
using Newtonsoft.Json;

namespace OldTarkovMovement
{
    public interface IFuckingConfig {
    }

    public class OldTarkovMovementConfig : IFuckingConfig
    {
        public bool NostalgiaMode { get; set; }
        public bool DoesAimingSlowYouDown { get; set; }
        public bool QuickTilting { get; set; }
        public bool BotsUseOldMovement { get; set; }
        public bool DoBushesSlowYouDown { get; set; }
        public bool RemoveJitteryRotation { get; set; }
    }

    [BepInPlugin("com.boogle.oldtarkovmovement", "Old Tarkov Movement", "1.0.6")]
    public class Plugin : BaseUnityPlugin
    {
        public static OldTarkovMovementConfig ModConfig;

        private static T UpdateInfoFromServer<T>(string route) where T : class, IFuckingConfig
        {
            var json = RequestHandler.GetJson(route);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Awake()
        {
            ModConfig = UpdateInfoFromServer<OldTarkovMovementConfig>("/OldTarkovMovement/GetConfig");
            if (!ModConfig.NostalgiaMode)
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
                
                if (!ModConfig.DoBushesSlowYouDown)
                {
                    new BushSpeedStateRemover().Enable();
                }
                
                if (ModConfig.QuickTilting)
                {
                    new OldTiltingFix().Enable();
                }
                
                if (ModConfig.RemoveJitteryRotation)
                {
                    new RemoveJitteryRotationPatch().Enable();
                    new RemoveStupidMotionSicknessBSGMade().Enable();
                }

                new AimingSlowdownPatch().Enable();

                

                if (ModConfig.NostalgiaMode)
                {
                    //new DisableAnimatedBlindfirePatch().Enable();
                    new ProceduralBlindfire().Enable();
                    new SendHandsInteractionStateChangedPatch().Enable();
                    new SetInteractInHandsPatch().Enable();
                    new DisableFancyInteractions().Enable();
                    new DropbackpackFix().Enable();

                    //new BlindfireWhileRunning().Enable();

                    DelayFikaLoad();
                }
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
