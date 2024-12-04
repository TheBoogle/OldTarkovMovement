using EFT;
using Fika.Core.Coop.ObservedClasses;
using SPT.Reflection.Patching;
using System;
using System.Reflection;

namespace Fika_OldTarkovMovement
{
    public class FikaStatePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(ObservedMovementContext).GetMethod("GetNewState", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(ObservedMovementContext __instance, ref BaseMovementState __result, EPlayerState name, bool isAI = false)
        {
            try
            {
                switch (name)
                {
                    case EPlayerState.Sprint:
                        __result = new FikaSprintState(__instance);

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
