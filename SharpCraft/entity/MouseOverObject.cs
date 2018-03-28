﻿using OpenTK;
using SharpCraft.block;

namespace SharpCraft.entity
{
    internal class MouseOverObject
    {
        public EnumFacing sideHit;

        public Vector3 hitVec;

        public Vector3 normal;

        public BlockPos blockPos;

        public object hit;
    }
}