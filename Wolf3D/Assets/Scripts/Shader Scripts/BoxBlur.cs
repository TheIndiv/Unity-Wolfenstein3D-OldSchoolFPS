using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBlur : MonoBehaviour
{

    public Material BlurMaterial;
    [Range(0, 10)]
    public int iterations;
    [Range(0, 4)]
    public int DownRes;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Downres the source texture by bitshifting it. This way our downresed texture will always be sclaed down in powers of 2.
        int width = source.width >> DownRes;
        int height = source.height >> DownRes;

        //Create a temporary render texture the size of our source width and height to store our current screen content into rt.
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        //Store the screen content in the temporary texture. This temporary texture will then have some post processing (blur in this case) applies to it.
        Graphics.Blit(source, rt);

        //This loop will blur rt however many times specified in the iterations variable.
        for (int i = 0; i < iterations; i++)
        {
            //Create another temporary texutre.
            RenderTexture rt2 = RenderTexture.GetTemporary(rt.width, rt.height);
            //Store the contents of rt into rt2 like before but this time apply the blur material (blur shader) onto rt to blur the texture a bit.
            Graphics.Blit(rt, rt2, BlurMaterial);
            //Now that we have a blurred version of rt in rt2, we can now let go of rt by releasing it.
            RenderTexture.ReleaseTemporary(rt);
            //Now assign the blurred texture back to rt again in order to be able to re-apply the blur shader the next time around the loop. 
            rt = rt2;
        }

        //Once the temporary texture has been blurred as many times as we wanted, we can then output that image to the screen.
        Graphics.Blit(rt, destination);
        //Now release the temporary texture as we have now outputted it to the display.
        RenderTexture.ReleaseTemporary(rt);
    }
}
