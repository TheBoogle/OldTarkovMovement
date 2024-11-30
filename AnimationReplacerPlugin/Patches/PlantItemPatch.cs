using EFT.Interactive;
using EFT;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using static GetActionsClass;
using Comfort.Common;
using UnityEngine;

namespace OldTarkovMovement.Patches
{
    public class TripwireInteractionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(Class1598).GetMethod("method_0", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(Class1598 __instance)
        {
            if (__instance.owner.Player.CurrentState is OldIdleState)
            {
                __instance.owner.ShowObjectivesPanel("Tripwire/Interaction/DisablingText{0:F1}".Localized(null), __instance.plantTime);
                __instance.tripwire.OnStateChanged += __instance.method_2;
                __instance.owner.Player.PlayTripwireInteractionSound(__instance.plantTime, __instance.hasMultiTool);
                MovementState currentManagedState = __instance.owner.Player.CurrentManagedState;
                bool enabled = true;
                bool multitool = true;
                float num = __instance.plantTime;
                Action<bool> action;
                if ((action = __instance.action_0) == null)
                {
                    action = (__instance.action_0 = new Action<bool>(__instance.method_1));
                }
                currentManagedState.Plant(enabled, multitool, num, action);
                return false;
            }
            __instance.owner.DisplayPreloaderUiNotification("Tripwire/Interaction/MustBeIdle".Localized(null));
            return false;
        }
    }

    public class RepairObjectivePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(Class1604).GetMethod("method_0", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(Class1604 __instance)
        {
            if (__instance.class1602_0.owner.Player.CurrentState is OldIdleState)
            {
                __instance.class1602_0.owner.ShowObjectivesPanel(__instance.isMultitool ? "Repairing objective {0:F1}" : "Hiding objective {0:F1}", __instance.class1602_0.resultLeaveItem.plantTime);
                MovementState currentManagedState = __instance.class1602_0.owner.Player.CurrentManagedState;
                bool enabled = true;
                bool multitool = __instance.isMultitool;
                float plantTime = __instance.class1602_0.resultLeaveItem.plantTime;
                Action<bool> action;
                if ((action = __instance.class1602_0.action_0) == null)
                {
                    action = (__instance.class1602_0.action_0 = new Action<bool>(__instance.class1602_0.method_0));
                }
                currentManagedState.Plant(enabled, multitool, plantTime, action);
                return false;
            }
            __instance.class1602_0.owner.DisplayPreloaderUiNotification("You can't plant quest item while moving".Localized(null));

            return false;
        }
    }

    public class BeaconPlacePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(Class1605).GetMethod("method_0", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(Class1605 __instance)
        {
            GetActionsClass.Class1606 @class = new GetActionsClass.Class1606();
            @class.class1605_0 = __instance;
            if (!(__instance.class1602_0.owner.Player.CurrentState is OldIdleState))
            {
                __instance.class1602_0.owner.DisplayPreloaderUiNotification("You can't plant a beacon while moving".Localized(null));
                return false;
            }
            Vector3 position = __instance.class1602_0.owner.Player.PlayerBones.WeaponRoot.position + __instance.class1602_0.owner.Player.HandsRotation * new Vector3(0f, 0f, 1f);
            if (__instance.class1602_0.itemTrigger.BeaconDummy == null)
            {
                __instance.class1602_0.itemTrigger.SetBeaconDummy(__instance.class1602_0.owner.Player.CreateBeacon(__instance.class1602_0.resultItem, position));
                return false;
            }
            if (!__instance.class1602_0.owner.Player.AllowToPlantBeacon)
            {
                __instance.class1602_0.owner.DisplayPreloaderUiNotification("Can't place beacon here".Localized(null));
                return false;
            }
            @class.itemInHands = __instance.class1602_0.owner.Player.InventoryController.ItemInHands;
            __instance.class1602_0.itemTrigger.SetBeaconDummy(null);
            __instance.class1602_0.owner.Player.DestroyBeacon();
            __instance.class1602_0.owner.Player.UpdateInteractionCast();
            __instance.class1602_0.owner.ClearInteractionState();
            __instance.class1602_0.owner.Player.SetInHandsForQuickUse(__instance.class1602_0.resultItem, new Callback<IOnHandsUseCallback>(@class.method_1));


            return false;
        }
    }

    public class ExfilInteractPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(Class1609).GetMethod("method_1", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(Class1609 __instance)
        {
            if (__instance.player.CurrentState is OldIdleState)
            {
                if (__instance.exfiltrationPoint.TransferExitItem(__instance.player, __instance.foundItem))
                {
                    __instance.owner.DisplayPreloaderUiNotification(string.Concat(new object[]
                    {
                        "Has been transfered".Localized(null),
                        ": ",
                        __instance.shortName,
                        " (",
                        __instance.count,
                        ")"
                    }));
                    return false;
                }
            }
            else
            {
                __instance.owner.DisplayPreloaderUiNotification("You can't transfer item while moving".Localized(null));
                return false;
            }

            return false;
        }
    }
}
