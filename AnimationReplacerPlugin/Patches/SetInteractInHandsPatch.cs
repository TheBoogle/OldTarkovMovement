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
using RootMotion;
using static RootMotion.FinalIK.InteractionTrigger.Range;
using System.Security.Cryptography.X509Certificates;

namespace OldTarkovMovement.Patches
{
    public class SetInteractInHandsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Replace with the target method's type and name
            return typeof(MovementContext).GetMethod("SetInteractInHands", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(MovementContext __instance, EInteraction interaction)
        {

            try
            {
                var playerField = typeof(MovementContext).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);

                if (playerField != null)
                {
                    Player player = playerField.GetValue(__instance) as Player;

                    if (__instance.IsInMountedState)
                    {
                        return false;
                    }
                    if (__instance.IsSprintEnabled)
                    {
                        return false;
                    }
                    if (__instance.IsStationaryWeaponInHands)
                    {
                        return false;
                    }
                    if (!player.HandsController.FirearmsAnimator.IsIdling() && interaction != EInteraction.DropBackpack)
                    {
                        return false;
                    }

                    int AnimationId = 1;

                    switch (interaction) // Honestly don't know the actual animation ID's and couldn't find em, just did the ones I 100% did know and guessed others.
                    {
                        case EInteraction.None:
                            return false;
                        case EInteraction.DropBackpack:
                            return false;
                        case EInteraction.TakeItem:
                            AnimationId = 1;
                            break;
                        case EInteraction.PullHingeLeft:
                            AnimationId = 2;
                            break;
                        case EInteraction.PullHingeRight:
                            AnimationId = 2;
                            break;
                        case EInteraction.PushHingeLeft:
                            AnimationId = 1;
                            break;
                        case EInteraction.PushHingeRight:
                            AnimationId = 1;
                            break;
                        case EInteraction.DoorKeyOpen:
                            AnimationId = 50;
                            break;
                    }

                    if (interaction != EInteraction.None)
                    {
                        player.HandsAnimator.Animator.SetLayerWeight(4, 1);

                        WeaponAnimationSpeedControllerClass.SetUseLeftHand(player.HandsAnimator.Animator, true);
                    }

                    WeaponAnimationSpeedControllerClass.SetLActionIndex(player.HandsAnimator.Animator, AnimationId);

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
