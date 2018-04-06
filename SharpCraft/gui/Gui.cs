﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpCraft.block;
using SharpCraft.model;
using SharpCraft.render;
using SharpCraft.render.shader;
using SharpCraft.render.shader.shaders;
using SharpCraft.texture;
using SharpCraft.util;

namespace SharpCraft.gui
{
    internal class Gui
    {
        private static ModelGuiItem _item;

        public static ShaderGui Shader { get; }

        static Gui()
        {
            _item = new ModelGuiItem(new Shader<object>("gui_item"));

            Shader = new ShaderGui();
        }

        protected virtual void RenderTexture(Texture tex, float x, float y, int textureX, int textureY, int sizeX, int sizeY, float scale = 1, bool centered = false)
        {
            if (tex == null)
                return;

            RenderTexture(new GuiTexture(tex, new Vector2(textureX, textureY), new Vector2(sizeX, sizeY), scale), x, y, centered);
        }

        protected virtual void RenderTexture(GuiTexture tex, float x, float y, bool cenetered = false)
        {
            if (tex == null)
                return;

            float width = tex.Size.X * tex.Scale;
            float height = tex.Size.Y * tex.Scale;

            var ratio = new Vector2(width / SharpCraft.Instance.ClientSize.Width, height / SharpCraft.Instance.ClientSize.Height);

            var unit = new Vector2(1f / SharpCraft.Instance.ClientSize.Width, 1f / SharpCraft.Instance.ClientSize.Height);

            var pos = new Vector2(x, -y);

            if (!cenetered)
            {
                pos.X += width / 2;
                pos.Y -= height / 2;
            }

            var mat = MatrixHelper.CreateTransformationMatrix(pos * unit * 2 + Vector2.UnitY - Vector2.UnitX, ratio);

            Shader.Bind();

            GL.BindVertexArray(GuiRenderer.GuiQuad.vaoID);

            Shader.UpdateGlobalUniforms();
            Shader.UpdateModelUniforms();
            Shader.UpdateInstanceUniforms(mat, tex);

            GL.EnableVertexAttribArray(0);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex.textureID);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            GL.DisableVertexAttribArray(0);

            Shader.Unbind();

            /*var ratio = new Vector2((float)tex.textureSize.Width / SharpCraft.Instance.ClientSize.Width, (float)tex.textureSize.Height / SharpCraft.Instance.ClientSize.Height);

            var mat = MatrixHelper.CreateTransformationMatrix(new  * 2, scale * ratio);
            Shader.UpdateGlobalUniforms();
            Shader.UpdateModelUniforms();
            Shader.UpdateInstanceUniforms(mat, tex);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex.textureID);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);*/
        }
        /*
        protected virtual void RenderTexture(GuiTexture tex)
        {
            var ratio = new Vector2((float)tex.textureSize.Width / SharpCraft.Instance.ClientSize.Width, (float)tex.textureSize.Height / SharpCraft.Instance.ClientSize.Height);

            var mat = MatrixHelper.CreateTransformationMatrix(tex.pos * 2, tex.scale * ratio);
            Shader.UpdateGlobalUniforms();
            Shader.UpdateModelUniforms();
            Shader.UpdateInstanceUniforms(mat, tex);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex.textureID);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }

        protected virtual void RenderTexture(GuiTexture tex, Vector2 scale, int x, int y)
        {
            Shader.Bind();
            GL.BindVertexArray(GuiRenderer.GuiQuad.vaoID);

            var unit = new Vector2(1f / SharpCraft.Instance.ClientSize.Width, 1f / SharpCraft.Instance.ClientSize.Height);

            float width = tex.textureSize.Width;
            float height = tex.textureSize.Height;

            float scaledWidth = width * scale.X;
            float scaledHeight = height * scale.Y;

            float posX = x + scaledWidth / 2;
            float posY = -y - scaledHeight / 2;

            var pos = new Vector2(posX, posY) * unit;

            var mat = MatrixHelper.CreateTransformationMatrix(pos * 2 - Vector2.UnitX + Vector2.UnitY, scale * new Vector2(width, height) * unit);
            Shader.UpdateGlobalUniforms();
            Shader.UpdateModelUniforms();
            Shader.UpdateInstanceUniforms(mat, tex);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tex.textureID);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            GL.BindVertexArray(0);

            Shader.Unbind();
        }*/

        protected virtual void RenderBlock(EnumBlock block, float x, float y, float scale)
        {
            var UVs = TextureManager.GetUVsFromBlock(block);
            ModelManager.overrideModelUVsInVAO(_item.RawModel.bufferIDs[1], UVs.getUVForSide(FaceSides.South).ToArray());

            var unit = new Vector2(1f / SharpCraft.Instance.ClientSize.Width, 1f / SharpCraft.Instance.ClientSize.Height);

            float width = 16;
            float height = 16;

            float scaledWidth = 16 * scale;
            float scaledHeight = 16 * scale;

            float posX = x + scaledWidth / 2;
            float posY = -y - scaledHeight / 2;

            var pos = new Vector2(posX, posY) * unit;

            var mat = MatrixHelper.CreateTransformationMatrix(pos * 2 - Vector2.UnitX + Vector2.UnitY, scale * new Vector2(width, height) * unit);
            
            _item.Bind();

            _item.Shader.UpdateGlobalUniforms();
            _item.Shader.UpdateModelUniforms();
            _item.Shader.UpdateInstanceUniforms(mat, null);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureManager.TEXTURE_BLOCKS.textureID);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            _item.Unbind();
        }

        public virtual void Render(int mouseX, int mouseY)
        {

        }
    }
}