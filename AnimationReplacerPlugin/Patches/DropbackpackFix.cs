using EFT.Interactive;
using EFT;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using Comfort.Common;
using EFT.InventoryLogic;
using System.Threading.Tasks;

namespace OldTarkovMovement.Patches
{
    public class DropbackpackFix : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Get the exact method by specifying parameter types
            return typeof(Player.FirearmController.GClass2026)
                .GetMethod("Start", BindingFlags.Public | BindingFlags.Instance, null,
                           new Type[] { typeof(Item), typeof(Callback) }, null);
        }

        [PatchPrefix]
        private static void Prefix(Player.FirearmController.GClass2026 __instance)
        {
            // Call the asynchronous method for delay
            DelayedFastForward(__instance);
        }

        private static async void DelayedFastForward(Player.FirearmController.GClass2026 instance)
        {
            await Task.Delay(250);

            // Call the FastForward method
            instance.FastForward();
        }
    }
}
