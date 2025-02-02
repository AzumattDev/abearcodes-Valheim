﻿using System;
using ABearCodes.Valheim.SimpleRecycling.Recycling;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ABearCodes.Valheim.SimpleRecycling.UI
{
    [HarmonyPatch]
    public static class InventoryGuiPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.OnTabCraftPressed))]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.OnTabUpgradePressed))]
        [HarmonyPriority(600)]
        static void OnTabCraftPressedAlsoEnableRecycling1(InventoryGui __instance)
        {
            Plugin.RecyclingTabButtonHolder.SetInteractable(true);
            // temporary fix for compatibility with EpicLoot
            __instance.UpdateCraftingPanel(false);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateCraftingPanel))]
        static bool UpdateCraftingPanelDetourOnRecyclingTab(InventoryGui __instance)
        {
            if (Plugin.RecyclingTabButtonHolder.InRecycleTab()) return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Hide))]
        static void OnHideSetToCraftingTab(InventoryGui __instance)
        {
            if (Plugin.RecyclingTabButtonHolder == null || !Plugin.RecyclingTabButtonHolder.InRecycleTab()) return;
            InventoryGui.instance.OnTabCraftPressed();
            Plugin.RecyclingTabButtonHolder.SetActive(false);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateCraftingPanel))]
        static void UpdateCraftingPanelDetourOnOtherTabsEnableRecyclingButton(InventoryGui __instance)
        {
            if (Plugin.RecyclingTabButtonHolder.InRecycleTab()) return;
            var player = Player.m_localPlayer;
            Plugin.RecyclingTabButtonHolder.SetInteractable(true);
            if (!player.GetCurrentCraftingStation() && !player.NoCostCheat())
            {
                Plugin.RecyclingTabButtonHolder.SetActive(false);
                return;
            }

            Plugin.RecyclingTabButtonHolder.SetActive(true);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateRecipe), typeof(Player), typeof(float))]
        static bool UpdateRecipeOnRecyclingTab(InventoryGui __instance, Player player, float dt)
        {
            if (!Plugin.RecyclingTabButtonHolder.InRecycleTab()) return true;
            Plugin.RecyclingTabButtonHolder.UpdateRecipe(player, dt);
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.Changed))]
        static void InventorySave()
        {
            if (Plugin.RecyclingTabButtonHolder == null || !Plugin.RecyclingTabButtonHolder.InRecycleTab()) return;
            Plugin.RecyclingTabButtonHolder.UpdateRecyclingList();
            InventoryGui.instance.SetRecipe(-1, false);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.EquipItem), typeof(ItemDrop.ItemData), typeof(bool))]
        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UnequipItem), typeof(ItemDrop.ItemData), typeof(bool))]
        static void HumanoidEquip(Humanoid __instance)
        {
            if (Plugin.RecyclingTabButtonHolder == null || __instance != Player.m_localPlayer || !Plugin.RecyclingTabButtonHolder.InRecycleTab()) return;
            Plugin.RecyclingTabButtonHolder.UpdateRecyclingList();
            InventoryGui.instance.SetRecipe(-1, false);
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.SetRecipe), typeof(int), typeof(bool))]
        public static void SetRecipe(this InventoryGui __instance, int index, bool center)
        {
            throw new NotImplementedException("stub");
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.GetSelectedRecipeIndex))]
        public static int GetSelectedRecipeIndex(this InventoryGui __instance, bool acceptOneLevelHigher)
        {
            throw new NotImplementedException("stub");
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.OnSelectedRecipe), typeof(GameObject))]
        public static void OnSelectedRecipe(this InventoryGui __instance, GameObject button)
        {
            throw new NotImplementedException("stub");
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.FindSelectedRecipe), typeof(GameObject))]
        public static int FindSelectedRecipe(this InventoryGui __instance, GameObject button)
        {
            throw new NotImplementedException("stub");
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateCraftingPanel), typeof(bool))]
        public static void UpdateCraftingPanel(this InventoryGui __instance, bool focusView)
        {
            throw new NotImplementedException("stub");
        }
    }
}