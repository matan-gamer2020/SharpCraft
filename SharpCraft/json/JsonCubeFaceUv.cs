﻿using Newtonsoft.Json;

namespace SharpCraft_Client.json
{
    public class JsonCubeFaceUv
    {
        [JsonProperty] public float[] uv = { 0, 0, 16, 16 };
        [JsonProperty] public string texture;
    }
}