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
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("OnEnterObstacle", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance, ObstacleCollider obstacle)
        {
            try
            {
                var obstaclesField = typeof(MovementContext).GetField("_enteredObstacles", BindingFlags.NonPublic | BindingFlags.Instance);

                if (obstaclesField != null)
                {
                    List<ObstacleCollider> Obstacles = obstaclesField.GetValue(__instance) as List<ObstacleCollider>;

                    if (Obstacles.IndexOf(obstacle) != -1)
                    {
                        return false;
                    }

                    // Check if is a bush
                    if (obstacle.gameObject.transform.parent.name.StartsWith("filbert"))
                    {
                        return false;
                    }

                    Obstacles.Add(obstacle);
                    __instance.method_0();

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
