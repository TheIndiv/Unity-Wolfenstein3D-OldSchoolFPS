using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchScript : MonoBehaviour
{

    public float glitchChance = 0.1f;

    private Renderer holoRenderer;
    private WaitForSeconds glitchLoopWait = new WaitForSeconds(0.1f);
    private WaitForSeconds glitchDuration = new WaitForSeconds(0.1f);

    private void Awake()
    {
        holoRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            float glitchTest = Random.Range(0f, 1f);

            if (glitchTest <= glitchChance)
            {
                StartCoroutine(Glitch());
            }
            yield return glitchLoopWait;
        }
    }

    IEnumerator Glitch()
    {
        glitchDuration = new WaitForSeconds(Random.Range(0.05f, 0.25f));
        holoRenderer.material.SetFloat("_Amount", 1f);
        holoRenderer.material.SetFloat("_CutoutThreshold", 0.29f);
        holoRenderer.material.SetFloat("_Amplitude", Random.Range(100, 250));
        holoRenderer.material.SetFloat("_Speed", Random.Range(1, 10));
        yield return glitchDuration;
        holoRenderer.material.SetFloat("_Amount", 0f);
        holoRenderer.material.SetFloat("_CutoutThreshold", 0f);
    }
}
