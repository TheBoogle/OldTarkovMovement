using EFT.Interactive;
using EFT;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace OldTarkovMovement.Patches
{
    public class SendHandsInteractionStateChangedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Ensure the method exists and matches the correct binding
            return typeof(Player).GetMethod("SendHandsInteractionStateChanged", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [PatchPrefix]
        private static bool Prefix(Player __instance, bool value, int animationId)
        {
            Logger.LogInfo("SendHandsInteractionStateChanged Prefix invoked.");
            Logger.LogInfo($"Parameters: p={value}, actionIndex={animationId}");

            try
            {
                IAnimator animator = __instance.HandsAnimator.Animator;
                if (animator == null)
                {
                    Logger.LogError("Failed to retrieve Animator instance.");
                    return true;
                }

                // Call the delayed operation
                AddLayerWeightWithDelay(animator);

                // Modify behavior based on the 'p' parameter
                if (value)
                {
                    WeaponAnimationSpeedControllerClass.SetUseLeftHand(animator, true);
                }

                WeaponAnimationSpeedControllerClass.SetLActionIndex(animator, animationId);
                Logger.LogInfo($"Pickup LActionIndex: {value} {animationId}");

                // Prevent original method execution
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"SendHandsInteractionStateChanged Prefix failed: {ex}");
                return true; // Allow original method to execute on failure
            }
        }

        private static async void AddLayerWeightWithDelay(IAnimator animator)
        {
            await Task.Delay(100);
            animator.SetLayerWeight(4, 1f);
            Logger.LogInfo("SetLayerWeight called after delay.");
        }
    }
}
