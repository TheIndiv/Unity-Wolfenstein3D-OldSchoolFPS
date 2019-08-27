using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(DistortionRenderer), PostProcessEvent.BeforeStack, "Custom/Distortion")]
public class Distortion : PostProcessEffectSettings
{
    [Range(0f, 1.0f), Tooltip("The Magnitude in texels of distortion fx.")]
    public FloatParameter Magnitude = new FloatParameter { value = 1.0f };

    [Range(0, 4), Tooltip("The down-scale factor to apply to the generated texture.")]
    public IntParameter DownScaleFactor = new IntParameter { value = 0 };

    [Tooltip("Displays the Distortion Effects in debug view.")]
    public BoolParameter DebugView = new BoolParameter { value = false };
}

public class DistortionRenderer : PostProcessEffectRenderer<Distortion>
{
    private int _globalDistortionTexID;
    private Shader _distortionShader;

    public override DepthTextureMode GetCameraFlags()
    {
        return DepthTextureMode.Depth;
    }

    public override void Init()
    {
        _globalDistortionTexID = Shader.PropertyToID("_GlobalDistortionTex");
        _distortionShader = Shader.Find("Hidden/Custom/Distortion");
        base.Init();
    }

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(_distortionShader);
        sheet.properties.SetFloat("_Magnitude", settings.Magnitude);

        if (!settings.DebugView)
        {
            context.command.GetTemporaryRT(_globalDistortionTexID,
                context.camera.pixelWidth >> settings.DownScaleFactor,
                context.camera.pixelHeight >> settings.DownScaleFactor,
                0, FilterMode.Bilinear, RenderTextureFormat.RGFloat);
            context.command.SetRenderTarget(_globalDistortionTexID);
            context.command.ClearRenderTarget(false, true, Color.clear);
        }

        DistortionManager.Instance.PopulateCommandBuffer(context.command);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
