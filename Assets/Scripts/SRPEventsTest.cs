using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class SRPEventsTest : MonoBehaviour
{
	public CameraEvent CameraEvent;
	public Camera Camera;
	public Material ImageEffectMaterial;
	public Shader ImageEffectShader;

	public void Start()
	{
		CommandBuffer cmd = new CommandBuffer();
		cmd.Blit(Camera.activeTexture, Camera.activeTexture, ImageEffectMaterial);
		Camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, cmd);
		ScriptableRenderPass.OnPostProcessEvent += ScriptableRenderPass_OnPostProcessEvent;
	}

	private void ScriptableRenderPass_OnPostProcessEvent(CommandBuffer cmd, ref CameraData data, RenderTextureDescriptor sourceDescriptor, RenderTargetIdentifier sourceIdentifier, RenderTargetIdentifier destinationIdentifier, bool opaqOnly, bool flip)
	{
		//cmd.SetGlobalTexture("_MainTex", sourceIdentifier);
		PropertySheetFactory fact = new PropertySheetFactory();
		fact.Get(ImageEffectShader);
		cmd.BlitFullscreenTriangle(sourceIdentifier, destinationIdentifier, fact.Get(ImageEffectShader), 0);
		//cmd.BlitFullscreenTriangle()
		//print("ScriptableRenderPass_OnPostProcessEvent");
	}

	protected void OnEnable()
	{
		UnityEngine.Experimental.Rendering.RenderPipeline.beginCameraRendering += RenderPipeline_beginCameraRendering;
		UnityEngine.Experimental.Rendering.RenderPipeline.beginFrameRendering += RenderPipeline_beginFrameRendering;
	}

	private void LightweightRenderPipeline_OnRenderingSingleCamera(ScriptableRenderContext context, Camera camera, CommandBuffer cmd)
	{
	}

	[ContextMenu("Disable")]
	protected void OnDisable()
	{
		try
		{
			UnityEngine.Experimental.Rendering.RenderPipeline.beginCameraRendering -= RenderPipeline_beginCameraRendering;
			UnityEngine.Experimental.Rendering.RenderPipeline.beginFrameRendering -= RenderPipeline_beginFrameRendering;
		}
		catch { }
	}

	private void RenderPipeline_beginFrameRendering(Camera[] obj)
	{
	}

	private void RenderPipeline_beginCameraRendering(Camera camera)
	{
		print(string.Format("Camera: [{0}] has {1}", camera.name, camera.GetCommandBuffers(CameraEvent).Length));
	}
}
