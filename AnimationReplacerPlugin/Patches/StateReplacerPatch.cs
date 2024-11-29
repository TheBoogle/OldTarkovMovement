using EFT;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnimationReplacerPlugin
{
    public class StateReplacer : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("GetNewState", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance, ref BaseMovementState __result, EPlayerState name)
        {
            try
            {
                Logger.LogInfo("Prefix executed for TargetMethod");

                switch (name)
                {
                    case EPlayerState.Idle:
                        __result = new OldIdleState(__instance);
                        return false;
                    case EPlayerState.Run:
                        __result = new OldRunState(__instance);
                        return false;
                    case EPlayerState.Sprint:
                        __result = new TempSprintState(__instance);
                        return false;
                    // Disabled for now due to issues with sprinting and jumping.
                    // Seems to just make you stop running...
                    //case EPlayerState.Sprint:
                    //    __result = new OldSprintState(__instance);
                    //    return false;
                    //case EPlayerState.Jump:
                    //    __result = new OldJumpState(__instance);
                    //    return false;
                    //case EPlayerState.Transition:
                    //    __result = new OldTransitionState(__instance);
                    //    return false;
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
