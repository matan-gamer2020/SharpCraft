﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpCraft.gui;
using SharpCraft.model;
using SharpCraft.texture;
using System.Collections.Generic;
using SharpCraft.render.shader;
using SharpCraft.render.shader.shaders;

namespace SharpCraft.render
{
    internal class GuiRenderer
    {
        public static ModelRaw GUIquad;

        private GuiCrosshair crosshairGui;
        private GuiHUD hudGui;

        static GuiRenderer()
        {
            var rawQuad = new RawQuad(new float[] {
                -1,  1,
                -1, -1,
                1, -1,
                1, 1 }, 2);

            GUIquad = ModelManager.loadModelToVAO(new List<RawQuad> { rawQuad }, 2);
        }

        public GuiRenderer()
        {
            crosshairGui = new GuiCrosshair();
            hudGui = new GuiHUD();
        }

        public void render(Gui gui)
        {
            if (gui == null)
                return;

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.DepthTest);

            GL.BindVertexArray(GUIquad.vaoID);

            var state = OpenTK.Input.Mouse.GetCursorState();
            var mouse = SharpCraft.Instance.PointToClient(new Point(state.X, state.Y));
            
            gui.Render(mouse.X, mouse.Y);

            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
        }

        public void renderCrosshair()
        {
            render(crosshairGui);
        }

        public void renderHUD()
        {
            render(hudGui);
        }
    }
}