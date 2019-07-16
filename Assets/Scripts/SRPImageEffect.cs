using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class BasicPipeInstance : RenderPipeline
{
	private Color m_ClearColor = Color.black;

	public BasicPipeInstance(Color clearColor)
	{
		m_ClearColor = clearColor;
	}

	protected override void Render(ScriptableRenderContext context, Camera[] cameras)
	{
		// does not so much yet :(
		//base.Render(context, cameras);
		Debug.Log("Called by SRP!!!");

		// clear buffers to the configured color
		var cmd = new CommandBuffer();
		cmd.ClearRenderTarget(true, true, m_ClearColor);
		context.ExecuteCommandBuffer(cmd);
		cmd.Release();
		context.Submit();
	}
}
