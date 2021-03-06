﻿using System;
using SharpCraft_Client.block;
using SharpCraft_Client.item;

namespace SharpCraft_Client
{
    internal class RegistryEventArgs : EventArgs
    {
        private readonly Action<Item> _funcRegisterItem;
        private readonly Action<Block> _funcRegisterBlock;
        private readonly Action<Item[], ItemStack, bool> _funcRegisterRecipe;

        public RegistryEventArgs(BlockRegistry blockRegistry, ItemRegistry itemRegistry, RecipeRegistry recipeRegistry)
        {
            _funcRegisterBlock = blockRegistry.Put;
            _funcRegisterItem = itemRegistry.Put;
            _funcRegisterRecipe = recipeRegistry.RegisterRecipe;
        }

        public void Register(Block block)
        {
            _funcRegisterBlock(block);
        }

        public void Register(Item item)
        {
            _funcRegisterItem(item);
        }

        public void Register(Item[] items, ItemStack product, bool shapeless)
        {
            _funcRegisterRecipe(items, product, shapeless);
        }

        public void Register(Item[] items, Item product, bool shapeless)
        {
            _funcRegisterRecipe(items, new ItemStack(product), shapeless);
        }
    }
}