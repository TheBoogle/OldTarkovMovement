﻿using EFT;
using EFT.Animations;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace OldTarkovMovement.Patches
{
    public class ProceduralBlindfire : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(ProceduralWeaponAnimation).GetMethod("ZeroAdjustments", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(ProceduralWeaponAnimation __instance)
        {
            try
            {
                // Access private fields
                var blindFirePositionField = __instance.GetType().GetField("_blindFirePosition", BindingFlags.NonPublic | BindingFlags.Instance);
                var blindFireRotationField = __instance.GetType().GetField("_blindFireRotation", BindingFlags.NonPublic | BindingFlags.Instance);
                var blindFireStrengthField = __instance.GetType().GetField("_blindfireStrength", BindingFlags.NonPublic | BindingFlags.Instance);

                if (blindFirePositionField == null || blindFireRotationField == null || blindFireStrengthField == null)
                {
                    return true; // Continue to the original method if fields are not found
                }

                // Update PositionZeroSum and RotationZeroSum
                __instance.PositionZeroSum.y = (__instance._shouldMoveWeaponCloser ? 0.05f : 0f);
                __instance.RotationZeroSum.y = __instance.SmoothedTilt * __instance.PossibleTilt;

                float value = __instance.BlindfireBlender.Value;
                float num = Mathf.Abs(value);

                if (num > 0f)
                {
                    // Calculate blindfire strength
                    float blindfireStrengthNew = (Mathf.Abs(__instance.Pitch) < 45f)
                        ? 1f
                        : ((90f - Mathf.Abs(__instance.Pitch)) / 45f);

                    blindFireStrengthField.SetValue(__instance, blindfireStrengthNew);

                    // Update blindfire position
                    Vector3 newPosition = (value > 0f)
                        ? (__instance.BlindFireOffset * num)
                        : (__instance.SideFireOffset * num);
                    
                    Vector3 newRotation = (value > 0f)
                        ? (__instance.BlindFireRotation * num)
                        : (__instance.SideFireRotation * num);


                    blindFirePositionField.SetValue(__instance, newPosition);
                    blindFireRotationField.SetValue(__instance, newRotation);

                    __instance.BlindFireEndPosition = (value > 0f)
                        ? __instance.BlindFireOffset
                        : __instance.SideFireOffset;

                    __instance.BlindFireEndPosition *= blindfireStrengthNew;
                }
                else
                {
                    // Reset blindfire position and rotation
                    blindFireRotationField.SetValue(__instance, Vector3.zero);
                    blindFirePositionField.SetValue(__instance, Vector3.zero);
                }

                // Cast the blindfire position to Vector3
                Vector3 position = (Vector3)blindFirePositionField.GetValue(__instance);

                // Update hands container positions
                __instance.HandsContainer.HandsPosition.Zero =
                    __instance.PositionZeroSum +
                    __instance.Single_3 * position;

                __instance.HandsContainer.HandsRotation.Zero = __instance.RotationZeroSum;

                return false; // Skip the original method
            }
            catch (Exception ex)
            {
                Logger.LogError($"SetBlindfireFix Prefix failed: {ex}");
                return true; // Continue to the original method in case of error
            }
        }
    }

    public class DisableAnimatedBlindfirePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("PlayerAnimatorSetBlindFire",
                BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance)
        {
            try
            {
                return false; // Skip the original method
            }
            catch (Exception ex)
            {
                Logger.LogError($"SetBlindfireFix Prefix failed: {ex}");
                return true; // Continue to the original method in case of error
            }
        }
    }

    public class BlindfireWhileRunning : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("SetBlindFire",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(int) }, // Specify the parameter type(s) explicitly
                null);
                }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance, int b)
        {
            try
            {
                var playerField = typeof(MovementContext).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);

                if (playerField != null)
                {
                    Player player = playerField.GetValue(__instance) as Player;

                    if (player.HandsController)
                    {
                        player.HandsController.BlindFire(b);
                    }

                    return false; // Skip the original method
                }

                return true; // Skip the original method
            }
            catch (Exception ex)
            {
                Logger.LogError($"SetBlindfireFix Prefix failed: {ex}");
                return true; // Continue to the original method in case of error
            }
        }
    }
}
