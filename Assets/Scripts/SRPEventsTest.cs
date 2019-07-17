using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;

[ExecuteInEditMode]
public class SRPEventsTest : MonoBehaviour
{
	public CameraEvent CameraEvent;
	public Camera Camera;
	public Material ImageEffectMaterial;

	public void Start()
	{
		CommandBuffer cmd = new CommandBuffer();
		//Camera.activeTexture
		cmd.Blit(Camera.activeTexture, Camera.activeTexture, ImageEffectMaterial);
		Camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, cmd);
	}

	protected void OnEnable()
	{
		UnityEngine.Experimental.Rendering.RenderPipeline.beginCameraRendering += RenderPipeline_beginCameraRendering;
		UnityEngine.Experimental.Rendering.RenderPipeline.beginFrameRendering += RenderPipeline_beginFrameRendering;
		//LightweightRenderPipeline.OnRenderingSingleCamera += LightweightRenderPipeline_OnRenderingSingleCamera;
	}

	private void LightweightRenderPipeline_OnRenderingSingleCamera(ScriptableRenderContext context, Camera camera, CommandBuffer cmd)
	{
		//print("asdagg");
		//context.
		print(cmd.name);
		//cmd.Blit()
		//cmd.Blit
	}

	[ContextMenu("Disable")]
	protected void OnDisable()
	{
		try
		{
			UnityEngine.Experimental.Rendering.RenderPipeline.beginCameraRendering -= RenderPipeline_beginCameraRendering;
			UnityEngine.Experimental.Rendering.RenderPipeline.beginFrameRendering -= RenderPipeline_beginFrameRendering;
			//LightweightRenderPipeline.OnRenderingSingleCamera -= LightweightRenderPipeline_OnRenderingSingleCamera;
		}
		catch { }
	}

	private void RenderPipeline_beginFrameRendering(Camera[] obj)
	{
		//print("beginFrameRendering");
	}

	private void RenderPipeline_beginCameraRendering(Camera camera)
	{
		//print("beginCameraRendering");obj.
		//var cmds = camera.GetCommandBuffers(CameraEvent);
		//CommandBuffer newcmd = new CommandBuffer();
		//camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, newcmd);
		//camera.AddCommandBuffer(CameraEvent.AfterEverything, newcmd);
		print(string.Format("Camera: [{0}] has {1}", camera.name, camera.GetCommandBuffers(CameraEvent).Length));
		//print()
		//print(obj.activeTexture.width);
		//newcmd.Blit(obj.activeTexture, )
		//print("cmds count: " + cmds.Length);
	}
}
