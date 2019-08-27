using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupFlash : MonoBehaviour
{

    public Color pickupColor;
    public Image pickupImage;
    public float pickupFlashSpeed;
    private bool fade = false;

    private void Update()
    {
        if (fade)
        {
            pickupImage.color = Color.Lerp(pickupImage.color, Color.clear, pickupFlashSpeed * Time.deltaTime);
        }
    }

	public void flashScreen()
    {
        pickupImage.color = pickupColor;
        fade = true;
    }
}
