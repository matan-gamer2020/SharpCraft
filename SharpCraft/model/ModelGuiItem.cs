﻿using System.Collections.Generic;
using SharpCraft.shader;

namespace SharpCraft.model
{
    internal class ModelGuiItem : ModelBaked
    {
        public ModelGuiItem(ShaderProgram shader) : base(null, shader)
        {
            var rawQuad = new RawQuad(new float[] {
                -1,  1,
                -1, -1,
                1, -1,
                1, 1 },
                new float[] {
                0, 0,
                1, 0,
                1, 1,
                0, 1}, 2);

            rawModel = ModelManager.loadModelToVAO(new List<RawQuad> { rawQuad }, 2);
        }
    }
}