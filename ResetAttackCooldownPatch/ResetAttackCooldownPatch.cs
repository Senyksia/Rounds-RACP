using System;
using BepInEx;
using HarmonyLib;

namespace ResetAttackCooldownPatch
{
    [BepInPlugin(ModID, ModName, ModVersion)]
    [BepInProcess("Rounds.exe")]
    public class ResetAttackCooldownPatch : BaseUnityPlugin
    {
        private const string ModID = "senyksia.rounds.plugins.resetattackcooldownpatch";
        private const string ModName = "Reset Attack-Cooldown Patch";
        private const string ModVersion = "1.0.0";

        private void Awake()
        {
            new Harmony(ModID).PatchAll();
        }

        private void Start()
        {
        }
    }

    // On death
    [HarmonyPatch(typeof(PlayerManager), "PlayerDied")]
    internal class PlayerManagerPatchPlayerDied
    {
        private static void Postfix(PlayerManager __instance, Player player)
        {
            Gun gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            gun.sinceAttack = gun.attackSpeed + 1f; // Reset attack cooldown (see Gun.IsReady())
        }
    }

    // On round start
    [HarmonyPatch(typeof(PlayerManager), "RevivePlayers")]
    internal class PlayerManagerPatchRevivePlayers
    {
        private static void Prefix(PlayerManager __instance)
        {
            for (int i = 0; i < __instance.players.Count; i++) // Loop through all players
            {
                Gun gun = __instance.players[i].GetComponent<Holding>().holdable.GetComponent<Gun>();
                gun.sinceAttack = gun.attackSpeed + 1f; // Reset attack cooldown (see Gun.IsReady())
            }
        }
    }
}
