﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpCraft.block;
using SharpCraft.model;
using SharpCraft.texture;
using SharpCraft.util;
using SharpCraft.world;
using System;
using System.Linq;
using SharpCraft.item;
using SharpCraft.render.shader;

namespace SharpCraft.entity
{
    public class EntityItem : Entity
    {
        private static Shader<EntityItem> _shader;

        private ItemStack stack;

        private int entityAge;

        private long tick;
        private long lastTick;

        static EntityItem()
        {
            _shader = new Shader<EntityItem>("entity_item");
        }

        public EntityItem(World world, Vector3 pos, Vector3 motion, ItemStack stack) : base(world, pos, motion)
        {
            this.stack = stack;

            collisionBoundingBox = new AxisAlignedBB(0.25f);
            boundingBox = collisionBoundingBox.offset(pos - Vector3.One * collisionBoundingBox.size / 2);

            gravity = 1.425f;
            isAlive = stack != null && !stack.IsEmpty;
        }

        public override void Update()
        {
            base.Update();

            lastTick = tick++;

            if (++entityAge >= 20 * 50 * 60 + 10) //stay on ground for a minute, 20 ticks as a pick up delay
            {
                SetDead();
                return;
            }

            if (entityAge < 5)
                return;

            var inAttractionArea = world.Entities.OfType<EntityItem>()
                .OrderBy(entity => MathUtil.Distance(entity.pos, pos + motion) - entity.stack.Count)
                .Where(
                    e => MathUtil.Distance(e.pos, pos + motion) <= 1.85f &&
                         e != this &&
                         e.isAlive &&
                         e.stack.Count <= stack.Count &&
                         e.stack?.Item?.item == stack?.Item?.item &&
                         e.stack?.Meta == stack?.Meta).ToList();

            foreach (var entity in inAttractionArea)
            {
                if (!entity.isAlive)
                    continue;

                var ammountToTake = Math.Min(64 - stack.Count, entity.stack.Count);

                if (ammountToTake > 0 && stack.Count + ammountToTake <= 64)
                {
                    entity.motion -= (entity.pos - pos).Normalized() * 0.15f;

                    if (MathUtil.Distance(entity.pos, pos) <= 0.25f)
                    {
                        entity.stack.Count -= ammountToTake;
                        stack.Count += ammountToTake;

                        motion = entity.motion * 0.25f;
                        entity.motion *= 0.35f;

                        entityAge = 3;
                        entity.entityAge = 1;

                        if (entity.stack.IsEmpty)
                            entity.SetDead();
                    }
                }
            }

            if (entityAge < 15 || !isAlive)
                return;

            //TODO change this for multiplayer
            var players = world.Entities.OfType<EntityPlayerSP>()
                               .OrderBy(entity => MathUtil.Distance(entity.pos, pos))
                               .Where(e => MathUtil.Distance(e.pos, pos) <= 2);

            foreach (var player in players)
            {
                if (player.OnPickup(stack))
                    SetDead();
            }
        }

        public override void Render(float particalTicks)
        {
            var partialPos = lastPos + (pos - lastPos) * particalTicks;
            var partialTime = lastTick + (tick - lastTick) * particalTicks;

            if (stack?.Item?.item is EnumBlock block)
            {
                var model = ModelRegistry.getModelForBlock(block, stack.Meta);

                if (model.RawModel == null)
                    return;

                var time = onGround ? (float)((Math.Sin(partialTime / 8) + 1) / 16) : 0;

                _shader.Bind();

                GL.BindVertexArray(model.RawModel.vaoID);

                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, TextureManager.blockTextureAtlasID);

                var itemsToRender = 1;

                if (stack.Count > 1)
                    itemsToRender = 2;
                if (stack.Count >= 32)
                    itemsToRender = 3;
                if (stack.Count == 64)
                    itemsToRender = 4;

                for (int i = 0; i < itemsToRender; i++)
                {
                    var rot = Vector3.UnitY * partialTime * 3;
                    var pos = partialPos - (Vector3.UnitX * 0.125f + Vector3.UnitZ * 0.125f) + Vector3.UnitY * time;
                    var pos_o = Vector3.One * (i / 8f);

                    var x = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rot.X));
                    var y = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rot.Y));
                    var z = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rot.Z));

                    var vec = Vector3.One * 0.5f;

                    var s = Matrix4.CreateScale(0.25f);
                    var t = Matrix4.CreateTranslation(pos + vec * 0.25f);
                    var t2 = Matrix4.CreateTranslation(-vec);
                    var t3 = Matrix4.CreateTranslation(pos_o);

                    var mat = t3 * t2 * (z * y * x * s) * t;

                    _shader.UpdateGlobalUniforms();
                    _shader.UpdateModelUniforms(model.RawModel);
                    _shader.UpdateInstanceUniforms(mat, this);
                    model.RawModel.Render(PrimitiveType.Quads);
                }

                GL.BindVertexArray(0);

                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);

                _shader.Unbind();
            }
        }
    }
}