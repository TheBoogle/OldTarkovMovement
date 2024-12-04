using EFT;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace OldTarkovMovement
{
    public class FixForSmoothMotherfuckingSpeed : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("ManualUpdate", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        private static void Prefix(MovementContext __instance, float deltaTime)
        {
            try
            {
                var playerField = typeof(MovementContext).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);

                if (playerField != null)
                {
                    Player player = playerField.GetValue(__instance) as Player;

                    if (!player.Physical.Sprinting)
                    {
                        float clampedSpeed = __instance.ClampedSpeed;
                        float num = Math.Abs(__instance.SmoothedCharacterMovementSpeed - clampedSpeed);
                        if (num < 1E-45f)
                        {
                            return;
                        }
                        if (num > 0.001f)
                        {
                            __instance.SmoothedCharacterMovementSpeed = Mathf.Lerp(__instance.SmoothedCharacterMovementSpeed, clampedSpeed, deltaTime * EFTHardSettings.Instance.CHARACTER_SPEED_CHANGING_SPEED);
                            return;
                        }
                        __instance.SmoothedCharacterMovementSpeed = clampedSpeed;
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                Logger.LogError($"GenericPatch Prefix failed: {ex}");
                return;
            }
        }
    }
}
