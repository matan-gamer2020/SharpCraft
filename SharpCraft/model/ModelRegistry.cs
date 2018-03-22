﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCraft
{
    class ModelRegistry
    {
        private static Dictionary<EnumBlock, List<BlockState>> models = new Dictionary<EnumBlock, List<BlockState>>();

        public static void registerBlockModel(ModelBlock model, int meta)
        {
            List<BlockState> states;

            //if already contains state with this meta tag, remove and set a new one
            if (models.TryGetValue(model.block, out states))
            {
                for (int i = 0; i < states.Count; i++)
                {
                    var state = states[i];

                    if (state.meta == meta)
                    {
                        states.Remove(state);
                        break;
                    }
                }
            }
            else
            {
                models.Add(model.block, states = new List<BlockState>());
            }

            states.Add(new BlockState(model, meta));
        }

        public static ModelBlock getModelForBlock(EnumBlock blockType, int meta)
        {
            if (blockType == EnumBlock.AIR)
                return null;

            models.TryGetValue(blockType, out var states);

            for (int i = 0; i < states.Count; i++)
            {
                var state = states[i];

                if (state.meta == meta)
                    return state.Model;
            }

            return null;
        }
    }

    class BlockState
    {
        public ModelBlock Model { get; }
        public int meta { get; }

        public BlockState(ModelBlock model, int meta)
        {
            Model = model;
            this.meta = meta;
        }
    }
}
