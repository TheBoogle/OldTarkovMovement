using EFT;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimationReplacerPlugin
{
    public class PreSprintAccelerationFix : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("PreSprintAcceleration", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance, float deltaTime)
        {
            try
            {
                Logger.LogInfo("Prefix executed for TargetMethod");

                var playerField = typeof(MovementContext).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);

                if (playerField != null)
                {
                    Player player = playerField.GetValue(__instance) as Player;

                    if (__instance.MovementDirection.y < 0.1f)
                    {
                        __instance.EnableSprint(false);
                        return false;
                    }
                    if (__instance.SmoothedCharacterMovementSpeed >= 1f)
                    {
                        return false;
                    }
                    __instance.CharacterMovementSpeed = (__instance.SmoothedCharacterMovementSpeed = Mathf.Clamp(__instance.SmoothedCharacterMovementSpeed + deltaTime * player.Physical.SprintAcceleration, 0f, 1f));
                    __instance.RaiseChangeSpeedEvent();

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"GenericPatch Prefix failed: {ex}");
                return true;
            }
        }
    }
}
