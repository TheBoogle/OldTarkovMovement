﻿using EFT;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OldTarkovMovement.MovementStates;

namespace OldTarkovMovement
{
    public class StateReplacer : ModulePatch
    {
        public static bool IsForModern = !OldTarkovMovement.Plugin.ModConfig.NostalgiaMode;

        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("GetNewState", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance, ref BaseMovementState __result, EPlayerState name, bool isAI = false)
        {
            if (isAI && !Plugin.ModConfig.BotsUseOldMovement)
            {
                return true;
            }

            try
            {
                switch (name)
                {
                    case EPlayerState.Idle:
                        __result = new OldIdleState(__instance);
                        return false;
                    case EPlayerState.ProneIdle:
                        if (isAI)
                        {
                            return true;
                        }
                        __result = new OldProneIdleState(__instance);
                        return false;
                    case EPlayerState.Run:
                        __result = new OldRunState(__instance);
                        return false;
                    case EPlayerState.Sprint:
                        if (IsForModern)
                        {
                            return true;
                        }
                        
                        __result = new OldSprintState(__instance);

                        return false;
                    case EPlayerState.Jump:
                        if (IsForModern)
                        {
                            return true;
                        }

                        __result = new OldJumpState(__instance);
                        return false;
                    case EPlayerState.Sidestep:
                        __result = new OldSidestepState(__instance);
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
