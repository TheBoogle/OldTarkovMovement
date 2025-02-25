using EFT.Interactive;
using EFT;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OldTarkovMovement.Patches
{
    public class AimingSlowdownPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("SetAimingSlowdown", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance)
        {
            try
            {
                if (!Plugin.ModConfig.DoesAimingSlowYouDown)
                {
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
