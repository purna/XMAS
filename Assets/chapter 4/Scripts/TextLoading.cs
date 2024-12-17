using UnityEngine;
using TMPro;  // Import the TextMesh Pro namespace
using System.Collections;

public class TypewriterEffectTMP : MonoBehaviour
{
    public float typingSpeed = 0.05f;  // Time between each character being typed
    public string fullText;  // The full text to display
    private TextMeshProUGUI textMeshPro;  // Reference to the TextMeshProUGUI component

    void Start()
    {
        // Get the TextMeshProUGUI component from the GameObject this script is attached to
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // Start the typing effect coroutine
        StartCoroutine(TypeText());
    }

    // Coroutine that types out the text one character at a time
    private IEnumerator TypeText()
    {
        // Clear any existing text
        textMeshPro.text = "";

        // Loop through each character in the fullText
        foreach (char letter in fullText)
        {
            // Add the next character to the text field
            textMeshPro.text += letter;

            // Wait for a short amount of time before typing the next letter
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
