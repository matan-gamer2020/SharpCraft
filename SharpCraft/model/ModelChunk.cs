﻿using SharpCraft_Client.render.shader;

namespace SharpCraft_Client.model
{
    internal class ModelChunk : ModelBaked
    {
        public bool IsGenerated { get; private set; }

        public ModelChunk(float[] vertexes, float[] normals, float[] uvs, Shader shader) : base(null, shader)
        {
            IsGenerated = vertexes.Length > 0;
            RawModel = ModelManager.LoadModel3ToVao(vertexes, normals, uvs);
        }

        public void OverrideData(float[] vertexes, float[] normals, float[] uvs)
        {
            IsGenerated = vertexes.Length > 0;
            RawModel = ModelManager.OverrideModel3InVao(RawModel.VaoID, RawModel.BufferIDs, vertexes, normals, uvs);
        }
    }
}