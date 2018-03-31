﻿using OpenTK;
using SharpCraft.block;
using SharpCraft.world.chunk;

namespace SharpCraft.util
{
    internal class MatrixHelper
    {
        public static Matrix4 createTransformationMatrixOrtho(Vector3 translation, Vector3 rot, float scale)
        {
            var x = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rot.X));
            var y = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rot.Y));
            var z = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rot.Z));

            var vec = Vector3.One * 0.5f;

            var s = Matrix4.CreateScale(scale, scale, 0);
            var t = Matrix4.CreateTranslation(translation);

            var t2 = Matrix4.CreateTranslation(-vec);

            return t2 * (x * z * y * s) * t;
        }

        public static Matrix4 createTransformationMatrix(Vector3 translation, Vector3 rot, float scale)
        {
            var x = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rot.X));
            var y = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rot.Y));
            var z = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rot.Z));

            var vec = Vector3.One * 0.5f;

            var s = Matrix4.CreateScale(scale);
            var t = Matrix4.CreateTranslation(translation + vec * scale);
            var t2 = Matrix4.CreateTranslation(-vec);

            return t2 * (x * z * y * s) * t;
        }

        public static Matrix4 createTransformationMatrix(Vector2 translation, Vector2 scale)
        {
            var s = Matrix4.CreateScale(scale.X, scale.Y, 1);
            var t = Matrix4.CreateTranslation(translation.X, translation.Y, 0);

            return s * t;
        }

        public static Matrix4 createTransformationMatrix(Vector3 translation, Vector3 scale)
        {
            var s = Matrix4.CreateScale(scale.X, scale.Y, scale.Z);
            var t = Matrix4.CreateTranslation(translation.X, translation.Y, translation.Z);

            return s * t;
        }

        public static Matrix4 createTransformationMatrix(Vector3 translation)
        {
            return Matrix4.CreateTranslation(translation);
        }

        public static Matrix4 createTransformationMatrix(BlockPos translation)
        {
            return Matrix4.CreateTranslation(translation.X, translation.Y, translation.Z);
        }

        public static Matrix4 createTransformationMatrix(ChunkPos translation)
        {
            return Matrix4.CreateTranslation(translation.x * Chunk.ChunkSize, 0, translation.z * Chunk.ChunkSize);
        }
    }
}