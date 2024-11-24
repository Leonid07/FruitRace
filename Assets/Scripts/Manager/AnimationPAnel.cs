using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPAnel : MonoBehaviour
{
    public Image parentImage;
    public RectTransform panelDoor;
    public float duration = 1.0f;
    public float delay = 1.0f;
    public float rotationSpeed = 90f; // Скорость вращения в градусах в секунду
    public float delayBeforeReturn = 2f;

    private Vector2 initialPosition;
    private bool hasLoggedMessage = false;

    void Start()
    {
        initialPosition = panelDoor.anchoredPosition;
    }

    public void StartAnimation(GameObject panel, bool isActive = false)
    {
        StartCoroutine(MovePanelOutAndBack(panel, isActive));
    }
    //public void StartAnimationUnLockLevel(GameObject panel)
    //{
    //StartCoroutine(AnimatePanelUnlock(panel));
    //}
    //private IEnumerator AnimatePanelUnlock(GameObject panel)
    //{
    //    panel.SetActive(true);

    //    yield return new WaitForSeconds(delay);
    //}

    public IEnumerator MovePanelOutAndBack(GameObject panel, bool isActive = false, GameObject[] panels = null)
    {
        float startRotation = 90f;
        float endRotation = 0f;
        float currentRotation = startRotation;
        while (currentRotation > endRotation)
        {
            currentRotation -= rotationSpeed * Time.deltaTime;
            if (currentRotation < endRotation)
                currentRotation = endRotation;
            panelDoor.localRotation = Quaternion.Euler(0, currentRotation, 0);
            yield return null;
        }

        // Вывод сообщения в консоль
        Debug.Log("Панель повернута на 0 градусов по оси Y");

        // Задержка перед возвратом
        yield return new WaitForSeconds(delayBeforeReturn);

        if (!isActive)
        {
            panel.SetActive(false);
            Debug.Log(panel.name);
            Debug.Log(isActive);
            if (panels != null)
            {
                foreach (var p in panels)
                {
                    p.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log(panel.name);
            Debug.Log(isActive);
            panel.SetActive(true);
            if (panels != null)
            {
                foreach (var p in panels)
                {
                    p.SetActive(true);
                }
            }
        }
        hasLoggedMessage = true;

        // Поворот от 0 до 90 градусов
        startRotation = 0f;
        endRotation = 90f;
        currentRotation = startRotation;
        while (currentRotation < endRotation)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            if (currentRotation > endRotation)
                currentRotation = endRotation;
            panelDoor.localRotation = Quaternion.Euler(0, currentRotation, 0);
            yield return null;
        }
        hasLoggedMessage = false; // Сбрасываем флаг для следующей анимации
    }
}