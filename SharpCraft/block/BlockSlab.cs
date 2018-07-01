﻿using OpenTK;
using SharpCraft.entity;

namespace SharpCraft.block
{
    public class BlockSlab : Block
    {
        protected BlockSlab(Material mat) : base(mat) //TODO
        {
            IsFullCube = false;

            var size = Vector3.One;

            size.Y = 0.5f;

            BoundingBox = new AxisAlignedBb(size);

            Hardness = 64; //TODO - set based on the state
        }
    }
}