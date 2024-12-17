using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import Unity's UI system

public class ColorWheelPanel : MonoBehaviour
{
    public Image panelImage; // Reference to the Image component of the panel
    public float speed = 0.1f; // Speed at which the color transitions around the color wheel

    private float hue = 0f; // The current hue value, starting from 0 (red)

    void Start()
    {
        if (panelImage == null)
        {
            panelImage = GetComponent<Image>(); // Automatically get the Image component if not assigned
        }
    }

    void Update()
    {
        // Increase the hue value over time to transition around the color wheel
        hue += speed * Time.deltaTime;

        // If the hue exceeds 1, wrap it around to stay within the 0 to 1 range
        if (hue > 1f)
        {
            hue = 0f;
        }

        // Convert the hue to an RGB color (saturation = 1, value = 1 for full saturation and brightness)
        Color color = Color.HSVToRGB(hue, 1f, 1f);

        // Apply the new color to the panel
        panelImage.color = color;
    }
}