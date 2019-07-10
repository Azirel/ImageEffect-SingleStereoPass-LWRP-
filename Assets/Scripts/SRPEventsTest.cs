using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SRPEventsTest : MonoBehaviour
{
	public void Start()
	{
		float f = 0;
		RenderPipeline.beginCameraRendering += RenderPipeline_beginCameraRendering;
		RenderPipeline.beginFrameRendering += RenderPipeline_beginFrameRendering;
	}

	private void RenderPipeline_beginFrameRendering(Camera[] obj)
	{
		//print("beginFrameRendering");
	}

	private void RenderPipeline_beginCameraRendering(Camera obj)
	{
		//print("beginCameraRendering");
	}
}
