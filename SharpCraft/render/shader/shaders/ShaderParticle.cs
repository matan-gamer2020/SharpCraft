﻿using OpenTK;
using SharpCraft.particle;
using SharpCraft.render.shader.uniform;

namespace SharpCraft.render.shader.shaders
{
	public class ShaderParticle : Shader<Particle>
	{
		private UniformVec2  UVmin;
		private UniformVec2  UVmax;
		private UniformFloat alpha;

		public ShaderParticle() : base("particle")
		{
		}

		protected override void registerUniforms()
		{
			base.registerUniforms();
			UVmin = new UniformVec2(GetUniformId("UVmin"));
			UVmax = new UniformVec2(GetUniformId("UVmax"));
			alpha = new UniformFloat(GetUniformId("alpha"));
		}

		public override void UpdateInstanceUniforms(Matrix4 transform, Particle instance)
		{
			base.UpdateInstanceUniforms(transform, instance);
			UVmin.Update(instance.UVmin);
			UVmax.Update(instance.UVmax);
			alpha.Update(instance.GetAlpha());
		}
	}
}