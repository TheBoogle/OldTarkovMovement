﻿using EFT;
using Fika.Core.Coop.ObservedClasses;
using SPT.Reflection.Patching;
using System;
using System.Reflection;

namespace OldTarkovMovement
{
    public class FikaStateEnterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(SprintState).GetMethod("Enter", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        private static void Prefix(SprintState __instance)
        {
            try
            {
                var MovementContextField = typeof(SprintState).GetField("MovementContext", BindingFlags.NonPublic | BindingFlags.Instance);
                MovementContext MovementContext = (MovementContext)MovementContextField.GetValue(__instance);

                MovementContext.SetPatrol(true);
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError($"GenericPatch Prefix failed: {ex}");
                return;
            }
        }
    }

    public class FikaStateExitPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(SprintState).GetMethod("Exit", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        private static void Prefix(SprintState __instance)
        {
            try
            {
                var MovementContextField = typeof(SprintState).GetField("MovementContext", BindingFlags.NonPublic | BindingFlags.Instance);
                MovementContext MovementContext = (MovementContext)MovementContextField.GetValue(__instance);

                MovementContext.SetPatrol(false);
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
