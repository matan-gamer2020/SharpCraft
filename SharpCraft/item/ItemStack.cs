﻿using OpenTK;
using System;
using SharpCraft_Client.util;

namespace SharpCraft_Client.item
{
    [Serializable]
    public class ItemStack
    {
        public Item Item;

        private int _count;

        public int Count
        {
            get => _count;
            set
            {
                if (Item == null)
                {
                    _count = 0;
                    return;
                }

                _count = MathHelper.Clamp(value, 0, Item.GetMaxStackSize());

                if (_count == 0)
                    Item = null;
            }
        }

        public short Meta;

        public bool IsEmpty => Count == 0 || Item == null;

        public ItemStack(Item item, int count = 1, short meta = 0)
        {
            Item = item;
            Meta = meta;

            Count = count;
        }

        public ItemStack Copy()
        {
            return Copy(Count);
        }

        public ItemStack Copy(int size)
        {
            return new ItemStack(Item, size, Meta);
        }

        public bool ItemSame(ItemStack other)
        {
            if (other == null) return false;
            return !IsEmpty && !other.IsEmpty && Meta == other.Meta && Item == other.Item;
        }

        public override string ToString() => Item != null ? LangUtil.GetLocalized(Item.UnlocalizedName) : "";

        public ItemStack Combine(ItemStack other)
        {
            ItemStack remainingStack = null;

            // Copy item if an item isn't present here
            if (Item == null)
            {
                if (other.Item == null)
                    return null; // error

                Item = other.Item;
            }

            // Combine stacks if enough space
            if (Count + other.Count <= Item.GetMaxStackSize())
            {
                Count += other.Count;
                //remainingStack = null;
            }
            // otherwise, combine as much as possible
            else
            {
                int difference = Item.GetMaxStackSize() - Count;
                Count += difference;

                remainingStack = other.Copy();
                remainingStack.Count -= difference;
            }

            return remainingStack;
        }
    }
}