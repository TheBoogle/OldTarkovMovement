using EFT;
using EFT.Interactive;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OldTarkovMovement
{
    public class BushSpeedStateRemover : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(MovementContext).GetMethod(nameof(MovementContext.OnEnterObstacle), BindingFlags.Public | BindingFlags.Instance);

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance, ObstacleCollider obstacle)
        {
            try
            {
                var Obstacles = __instance._enteredObstacles;

                if (Obstacles.IndexOf(obstacle) != -1)
                {
                    return false;
                }

                // Check if is a bush (fuck u nikita for using SWAMP on bushes, and fuck me for being too lazy to properly check if its water or not)
                if (obstacle.HasSwampSpeedLimit)
                {
                    return false;
                }

                Obstacles.Add(obstacle);
                __instance.method_0();

                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError($"GenericPatch Prefix failed: {ex}");
                return true;
            }
        }
    }
}
