using System.Collections.Generic;
using UnityEngine;

public class TNTCanVisualController : MonoBehaviour
{
    [Header("References")]
    public RectTransform[] cans;
    public CanvasGroup[] canGroups;

    [Header("Queue Positions")]
    public Vector2 firstPosition;
    public Vector2 secondPosition;
    public Vector2 thirdPosition;

    [Header("Visual")]
    public Vector3 nextScale = new Vector3(1.12f, 1.12f, 1f);
    public Vector3 inactiveScale = new Vector3(0.82f, 0.82f, 1f);

    public float nextAlpha = 1f;
    public float inactiveAlpha = 0.35f;

    public float nextBaseRotation = -6f;

    [Header("Shake")]
    public float shakeSpeed = 2f;
    public float shakeAmount = 1.2f;

    [Header("Smooth")]
    public float smoothSpeed = 10f;

    private List<int> queue = new List<int>();

    private void Start()
    {
        InitializeQueue();
    }

    private void Update()
    {
        UpdateVisuals();
    }

    private void InitializeQueue()
    {
        queue.Clear();

        for (int i = 0; i < cans.Length; i++)
        {
            queue.Add(i);
        }
    }

    public int GetCurrentCanIndex()
    {
        if (queue.Count == 0)
            return 0;

        return queue[0];
    }

    public void MoveCurrentCanToEnd()
    {
        if (queue.Count <= 1)
            return;

        int current = queue[0];

        queue.RemoveAt(0);
        queue.Add(current);
    }

    private void UpdateVisuals()
    {
        if (cans == null || canGroups == null)
            return;

        for (int queuePosition = 0; queuePosition < queue.Count; queuePosition++)
        {
            int canIndex = queue[queuePosition];

            if (canIndex < 0 || canIndex >= cans.Length)
                continue;

            bool isNext = queuePosition == 0;

            ApplyPosition(canIndex, queuePosition);
            ApplyScale(canIndex, isNext);
            ApplyRotation(canIndex, isNext);
            ApplyAlpha(canIndex, isNext);
        }
    }

    private void ApplyPosition(int canIndex, int queuePosition)
    {
        Vector2 targetPosition = firstPosition;

        if (queuePosition == 1)
            targetPosition = secondPosition;
        else if (queuePosition == 2)
            targetPosition = thirdPosition;

        cans[canIndex].anchoredPosition = Vector2.Lerp(
            cans[canIndex].anchoredPosition,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );
    }

    private void ApplyScale(int canIndex, bool isNext)
    {
        Vector3 targetScale = isNext ? nextScale : inactiveScale;

        cans[canIndex].localScale = Vector3.Lerp(
            cans[canIndex].localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );
    }

    private void ApplyRotation(int canIndex, bool isNext)
    {
        float targetRotationZ = 0f;

        if (isNext)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            targetRotationZ = nextBaseRotation + shake;
        }

        cans[canIndex].localRotation = Quaternion.Lerp(
            cans[canIndex].localRotation,
            Quaternion.Euler(0f, 0f, targetRotationZ),
            Time.deltaTime * smoothSpeed
        );
    }

    private void ApplyAlpha(int canIndex, bool isNext)
    {
        float targetAlpha = isNext ? nextAlpha : inactiveAlpha;

        canGroups[canIndex].alpha = Mathf.Lerp(
            canGroups[canIndex].alpha,
            targetAlpha,
            Time.deltaTime * smoothSpeed
        );
    }
}