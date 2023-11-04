using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterManager : MonoBehaviour
{
    public Slider pointer;
    public Image target;
    public float targetSize = 1.0f;
    public float sliderSpeed = 1.0f;

    private float initialTargetWidth;
    private bool sliding = false;

    private void Start()
    {
        initialTargetWidth = target.rectTransform.sizeDelta.x;
        StartSliderMotion();
    }

    private void Update()
    {
        pointer.value += Time.deltaTime * sliderSpeed;

        // player has attempted to hit the target
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopSliderMotion();
            CheckSuccess();
        }

        // reverse direction
        if (pointer.value >= 1 || pointer.value <= 0)
        {
            sliderSpeed = -sliderSpeed;
        }
    }

    void UpdateMeter(float newSize)
    {
        targetSize = newSize;

        float newTargetWidth = initialTargetWidth * targetSize;

        Vector2 newSizeDelta = target.rectTransform.sizeDelta;
        newSizeDelta.x = newTargetWidth;
        target.rectTransform.sizeDelta = newSizeDelta;
    }

    public void OnSuccess()
    {
        targetSize -= 0.1f;
        targetSize = Mathf.Max(0, targetSize);
        UpdateMeter(targetSize);
    }

    private void StartSliderMotion()
    {
        sliding = true;
    }

    private void StopSliderMotion()
    {
        sliding = false;
    }

    private void CheckSuccess()
    {
        float sliderValue = pointer.value;
        float targetZoneStart = 0.4f; // Adjust this value as needed
        float targetZoneEnd = 0.6f;   // Adjust this value as needed

        if (sliderValue >= targetZoneStart && sliderValue <= targetZoneEnd)
        {
            OnSuccess(); // The player succeeded
        } else
        {
            Debug.Log("FAIL");
        }
    }
}
