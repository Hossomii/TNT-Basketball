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

    private readonly List<int> queue = new List<int>();

    private void Start()
    {
        InitializeQueue();
        UpdateVisualsInstant();
    }

    private void Update()
    {
        if (GameplayLockSystem.IsGameplayLocked)
            return;

        UpdateVisuals();
    }

    private void InitializeQueue()
    {
        queue.Clear();

        if (cans == null)
            return;

        for (int i = 0; i < cans.Length; i++)
            queue.Add(i);
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

        UpdateVisualsInstant();
    }

    private void UpdateVisuals()
    {
        if (!HasValidReferences())
            return;

        for (int queuePosition = 0; queuePosition < queue.Count; queuePosition++)
        {
            int canIndex = queue[queuePosition];

            if (!IsValidCanIndex(canIndex))
                continue;

            bool isNext = queuePosition == 0;

            ApplyPosition(canIndex, queuePosition);
            ApplyScale(canIndex, isNext);
            ApplyRotation(canIndex, isNext);
            ApplyAlpha(canIndex, isNext);
        }
    }

    private void UpdateVisualsInstant()
    {
        if (!HasValidReferences())
            return;

        for (int queuePosition = 0; queuePosition < queue.Count; queuePosition++)
        {
            int canIndex = queue[queuePosition];

            if (!IsValidCanIndex(canIndex))
                continue;

            bool isNext = queuePosition == 0;

            cans[canIndex].anchoredPosition = GetTargetPosition(queuePosition);
            cans[canIndex].localScale = GetTargetScale(isNext);
            cans[canIndex].localRotation = Quaternion.Euler(0f, 0f, GetTargetRotationWithoutShake(isNext));
            canGroups[canIndex].alpha = GetTargetAlpha(isNext);
        }
    }

    private bool HasValidReferences()
    {
        return cans != null &&
               canGroups != null &&
               cans.Length > 0 &&
               canGroups.Length >= cans.Length;
    }

    private bool IsValidCanIndex(int canIndex)
    {
        return canIndex >= 0 &&
               canIndex < cans.Length &&
               cans[canIndex] != null &&
               canGroups[canIndex] != null;
    }

    private Vector2 GetTargetPosition(int queuePosition)
    {
        if (queuePosition == 0)
            return firstPosition;

        if (queuePosition == 1)
            return secondPosition;

        return thirdPosition;
    }

    private Vector3 GetTargetScale(bool isNext)
    {
        return isNext ? nextScale : inactiveScale;
    }

    private float GetTargetRotation(bool isNext)
    {
        if (!isNext)
            return 0f;

        float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        return nextBaseRotation + shake;
    }

    private float GetTargetRotationWithoutShake(bool isNext)
    {
        return isNext ? nextBaseRotation : 0f;
    }

    private float GetTargetAlpha(bool isNext)
    {
        return isNext ? nextAlpha : inactiveAlpha;
    }

    private void ApplyPosition(int canIndex, int queuePosition)
    {
        Vector2 targetPosition = GetTargetPosition(queuePosition);

        cans[canIndex].anchoredPosition = Vector2.Lerp(
            cans[canIndex].anchoredPosition,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );
    }

    private void ApplyScale(int canIndex, bool isNext)
    {
        Vector3 targetScale = GetTargetScale(isNext);

        cans[canIndex].localScale = Vector3.Lerp(
            cans[canIndex].localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );
    }

    private void ApplyRotation(int canIndex, bool isNext)
    {
        float targetRotationZ = GetTargetRotation(isNext);

        cans[canIndex].localRotation = Quaternion.Lerp(
            cans[canIndex].localRotation,
            Quaternion.Euler(0f, 0f, targetRotationZ),
            Time.deltaTime * smoothSpeed
        );
    }

    private void ApplyAlpha(int canIndex, bool isNext)
    {
        float targetAlpha = GetTargetAlpha(isNext);

        canGroups[canIndex].alpha = Mathf.Lerp(
            canGroups[canIndex].alpha,
            targetAlpha,
            Time.deltaTime * smoothSpeed
        );
    }
}