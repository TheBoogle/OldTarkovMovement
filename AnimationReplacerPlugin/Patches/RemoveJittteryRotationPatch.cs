using EFT.Interactive;
using EFT;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Comfort.Common;
using UnityEngine;
using EFT.Animations;

namespace OldTarkovMovement.Patches
{
    public class RemoveJitteryRotationPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(MovementContext).GetMethod(nameof(MovementContext.PlayerAnimatorSetDeltaRotation), BindingFlags.Public | BindingFlags.Instance);

        public new void Enable()
        {
            base.Enable();

            EFTHardSettings.Instance.TRANSFORM_ROTATION_LERP_SPEED = 1000;
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance)
        {
            try
            {
                if (__instance.IsAI)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"GenericPatch Prefix failed: {ex}");
                return true;
            }
        }
    }

    public class RemoveStupidMotionSicknessBSGMade : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(ProceduralWeaponAnimation).GetMethod(nameof(ProceduralWeaponAnimation.InitTransforms), BindingFlags.Public | BindingFlags.Instance);

        [PatchPostfix]
        private static void Postfix(ProceduralWeaponAnimation __instance)
        {
            __instance.HandsContainer.CameraRotation.InputIntensity = 0;
        }
    }
}
