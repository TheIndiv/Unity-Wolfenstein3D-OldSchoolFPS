using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{

	public Color color;
	public Image image;
	public float flashSpeed;
    private bool fade = false;

    private void Update()
    {
        if (fade)
        {
            image.color = Color.Lerp(image.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

	public void flashScreen()
    {
        image.color = color;
        fade = true;
    }
}
