using UnityEngine;

using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	[PostProcess(typeof(MobileBloomRenderer), PostProcessEvent.BeforeStack, "SupGames/MobileBloomLWRP")]
	public sealed class MobileBloomLWRP : PostProcessEffectSettings
	{
		[Range(0f, 5f), Tooltip("Bloom Amount")]
		public FloatParameter bloom = new FloatParameter { value = 0.5f };
		[Range(0f, 5f), Tooltip("Blur Amount")]
		public FloatParameter blur = new FloatParameter { value = 0.5f };
		[Range(0f, 1f), Tooltip("Fade Amount")]
		public FloatParameter fade = new FloatParameter { value = 0.5f };
	}

	public sealed class MobileBloomRenderer : PostProcessEffectRenderer<MobileBloomLWRP>
	{

		static readonly int scrWidth = Screen.width / 4;
		static readonly int scrHeight = Screen.height / 4;
		static readonly int blAmountString = Shader.PropertyToID("_BloomAmount");
		static readonly int blurAmountString = Shader.PropertyToID("_BlurAmount");
		static readonly int fadeAmountString = Shader.PropertyToID("_FadeAmount");
		static readonly int temp = Shader.PropertyToID("_BloomTex");
		static readonly int bloomTexString = Shader.PropertyToID("_BloomTmp");
		static readonly int temp2 = Shader.PropertyToID("_BloomTmp2");
		static readonly Shader shader = Shader.Find("SupGames/MobileBloomLWRP");


		public override void Render(PostProcessRenderContext context)
		{
			try
			{
				var sheet = context.propertySheets.Get(shader);
				sheet.properties.SetFloat(blAmountString, settings.bloom);
				sheet.properties.SetFloat(blurAmountString, settings.blur);
				sheet.properties.SetFloat(fadeAmountString, settings.fade);

				context.GetScreenSpaceTemporaryRT(context.command, temp, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, scrWidth, scrHeight);
				context.GetScreenSpaceTemporaryRT(context.command, temp2, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, scrWidth / 2, scrHeight / 2);

				context.command.BlitFullscreenTriangle(context.source, temp, sheet, 0);
				context.command.BlitFullscreenTriangle(temp, temp2, sheet, 1);
				context.command.ReleaseTemporaryRT(temp);

				context.GetScreenSpaceTemporaryRT(context.command, bloomTexString, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, scrWidth, scrHeight);
				context.command.BlitFullscreenTriangle(temp2, bloomTexString, sheet, 1);
				context.command.ReleaseTemporaryRT(temp2);

				context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 2);
				context.command.ReleaseTemporaryRT(bloomTexString);
			}
			catch (Exception e)
			{
				Debug.Log("Injected error message: " + e.Message);
				throw;
			}
		}
	}
}