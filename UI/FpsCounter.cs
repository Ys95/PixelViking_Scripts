using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI display;

    float fps;
    float averageFps;
    float displayedFps;
    int counter;

    void UpdateDisplay()
    {
        display.text = "FPS: " + displayedFps;
    }

    void Update()
    {
        fps = 1 / Time.unscaledDeltaTime;
        averageFps += fps;
        counter++;

        if (counter >= 10)
        {
            averageFps /= counter;
            displayedFps = Mathf.Round(averageFps);
            UpdateDisplay();

            averageFps = 0f;
            counter = 0;
        }
    }
}