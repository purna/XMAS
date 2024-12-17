using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace to access the Panel and its components

public class ChangePanelColor : MonoBehaviour
{
    public Image panelImage; // The panel's Image component
    public Color targetColor = Color.red; // The target color to change to
    public float transitionSpeed = 1f; // Speed at which the color changes

    private Color currentColor; // The current color of the panel
    private bool isChangingColor = false; // Flag to control color transition

    void Start()
    {
        if (panelImage == null)
        {
            panelImage = GetComponent<Image>(); // Automatically get the Image component if not assigned
        }

        currentColor = panelImage.color; // Initialize the current color to the panel's current color
    }

    void Update()
    {
        // Smoothly change the panel's color from currentColor to targetColor
        if (isChangingColor)
        {
            panelImage.color = Color.Lerp(panelImage.color, targetColor, transitionSpeed * Time.deltaTime);

            // If the color is close enough to the target, stop the transition
            if (Mathf.Approximately(panelImage.color.r, targetColor.r) && Mathf.Approximately(panelImage.color.g, targetColor.g) && Mathf.Approximately(panelImage.color.b, targetColor.b))
            {
                isChangingColor = false;
            }
        }
    }

    // This function can be called to trigger the color change
    public void ChangeColor()
    {
        currentColor = panelImage.color; // Store the current color
        isChangingColor = true; // Begin transitioning the color
    }

    // Optionally, you can reset the panel color to the original color
    public void ResetColor()
    {
        targetColor = currentColor; // Set the target color back to the original color
        isChangingColor = true; // Start transitioning back to the original color
    }
}