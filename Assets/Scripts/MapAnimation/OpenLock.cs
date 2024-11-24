using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenLock : MonoBehaviour
{
    public RectTransform panel;
    public Vector2 targetPosition;
    public Vector2 expandedSize;
    public Image image;
    public float imageMoveDistanceY;
    public float animationDuration = 1f;
    public float imageRotationAngle = 30f;

    private Vector2 initialPosition;
    private Vector2 initialSize;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = panel.anchoredPosition;
        initialSize = panel.sizeDelta;
        initialRotation = image.rectTransform.localRotation;
    }

    public void StartAnimation()
    {
        StartCoroutine(AnimatePanel());
    }

    IEnumerator AnimatePanel()
    {
        var Canvas = gameObject.AddComponent<Canvas>();
        Canvas.overrideSorting = true;
        Canvas.sortingOrder = 2;
        // 1. Move the panel to the target position
        yield return StartCoroutine(MovePanel(panel, initialPosition, targetPosition, animationDuration));

        // 2. Expand the panel
        yield return StartCoroutine(ResizePanel(panel, initialSize, expandedSize, animationDuration));

        // 3. Rotate the image
        yield return StartCoroutine(RotateImage(image, -imageRotationAngle, animationDuration / 3));
        yield return StartCoroutine(RotateImage(image, imageRotationAngle, animationDuration / 3));
        yield return StartCoroutine(RotateImage(image, 0, animationDuration / 3));

        // 4. Move the image up and down
        yield return StartCoroutine(MoveImage(image.rectTransform, Vector2.zero, new Vector2(0, imageMoveDistanceY), animationDuration / 2));
        yield return StartCoroutine(MoveImage(image.rectTransform, new Vector2(0, imageMoveDistanceY), Vector2.zero, animationDuration / 2));

        // 5. Return the panel to its initial size
        yield return StartCoroutine(ResizePanel(panel, expandedSize, initialSize, animationDuration));

        DataManager.InstanceData.mapNextLevel.OpenLevel();
        yield return StartCoroutine(MovePanel(panel, targetPosition, initialPosition, animationDuration));

        Destroy(Canvas);
        // Log the message
        Debug.Log("All animations completed.");
    }

    IEnumerator MovePanel(RectTransform rectTransform, Vector2 start, Vector2 end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = end;
    }

    IEnumerator ResizePanel(RectTransform rectTransform, Vector2 start, Vector2 end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.sizeDelta = end;
    }

    IEnumerator RotateImage(Image img, float targetZRotation, float duration)
    {
        float time = 0f;
        Quaternion startRotation = img.rectTransform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, targetZRotation);
        while (time < duration)
        {
            img.rectTransform.localRotation = Quaternion.Lerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        img.rectTransform.localRotation = endRotation;
    }

    IEnumerator MoveImage(RectTransform rectTransform, Vector2 start, Vector2 end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = end;
    }
}
