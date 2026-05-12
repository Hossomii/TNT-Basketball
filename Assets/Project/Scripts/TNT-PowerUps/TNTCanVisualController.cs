using System.Collections.Generic;
using UnityEngine;

public class TNTCanVisualController : MonoBehaviour
{
    [Header("Small Queue")]
    public RectTransform[] queueCans;
    public CanvasGroup[] queueGroups;

    [Header("Queue Positions")]
    public Vector2 firstPos;
    public Vector2 secondPos;
    public Vector2 thirdPos;

    [Header("Scale")]
    public Vector3 highlightedScale = new Vector3(1.1f, 1.1f, 1f);
    public Vector3 normalScale = new Vector3(0.8f, 0.8f, 1f);

    [Header("Alpha")]
    public float highlightedAlpha = 1f;
    public float normalAlpha = 0.45f;

    [Header("Animation")]
    public float moveSpeed = 8f;
    public float scaleSpeed = 8f;
    public float alphaSpeed = 8f;

    [Header("Shake")]
    public float shakeSpeed = 5f;
    public float shakeAmount = 4f;

    private readonly List<int> canQueue = new List<int>();

    private bool isVisualLocked;

    private void OnEnable()
    {
        GameplayLockSystem.OnGameplayLocked += LockVisuals;
        GameplayLockSystem.OnGameplayUnlocked += UnlockVisuals;

        isVisualLocked = GameplayLockSystem.IsGameplayLocked;
    }

    private void OnDisable()
    {
        GameplayLockSystem.OnGameplayLocked -= LockVisuals;
        GameplayLockSystem.OnGameplayUnlocked -= UnlockVisuals;
    }

    private void Start()
    {
        InitializeQueue();
        ForceVisualRefresh();
    }

    private void Update()
    {
        if (isVisualLocked)
            return;

        UpdateQueueVisuals();
    }

    private void LockVisuals()
    {
        isVisualLocked = true;
    }

    private void UnlockVisuals()
    {
        isVisualLocked = false;
    }

    private void InitializeQueue()
    {
        canQueue.Clear();

        for (int i = 0; i < queueCans.Length; i++)
            canQueue.Add(i);
    }

    public int GetCurrentCanIndex()
    {
        if (canQueue.Count == 0)
            return 0;

        return canQueue[0];
    }

    public void MoveCurrentCanToEnd()
    {
        if (canQueue.Count <= 1)
            return;

        int usedCan = canQueue[0];

        canQueue.RemoveAt(0);
        canQueue.Add(usedCan);
    }

    private void ForceVisualRefresh()
    {
        for (int visualIndex = 0; visualIndex < canQueue.Count; visualIndex++)
        {
            int canIndex = canQueue[visualIndex];

            if (!IsValidCan(canIndex))
                continue;

            queueCans[canIndex].anchoredPosition = GetTargetPosition(visualIndex);
            queueCans[canIndex].localScale = GetTargetScale(visualIndex);
            queueCans[canIndex].localRotation = Quaternion.identity;

            if (IsValidGroup(canIndex))
                queueGroups[canIndex].alpha = GetTargetAlpha(visualIndex);
        }
    }

    private void UpdateQueueVisuals()
    {
        for (int visualIndex = 0; visualIndex < canQueue.Count; visualIndex++)
        {
            int canIndex = canQueue[visualIndex];

            if (!IsValidCan(canIndex))
                continue;

            RectTransform can = queueCans[canIndex];

            Vector2 targetPosition = GetTargetPosition(visualIndex);

            can.anchoredPosition = Vector2.Lerp(
                can.anchoredPosition,
                targetPosition,
                Time.deltaTime * moveSpeed
            );

            can.localScale = Vector3.Lerp(
                can.localScale,
                GetTargetScale(visualIndex),
                Time.deltaTime * scaleSpeed
            );

            ApplyRotation(can, visualIndex);
            ApplyAlpha(canIndex, visualIndex);
        }
    }

    private Vector2 GetTargetPosition(int visualIndex)
    {
        switch (visualIndex)
        {
            case 0:
                return firstPos;

            case 1:
                return secondPos;

            default:
                return thirdPos;
        }
    }

    private Vector3 GetTargetScale(int visualIndex)
    {
        return IsHighlightedCan(visualIndex)
            ? highlightedScale
            : normalScale;
    }

    private float GetTargetAlpha(int visualIndex)
    {
        return IsHighlightedCan(visualIndex)
            ? highlightedAlpha
            : normalAlpha;
    }

    private void ApplyRotation(RectTransform can, int visualIndex)
    {
        if (IsHighlightedCan(visualIndex))
        {
            float rotationShake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;

            can.localRotation = Quaternion.Euler(
                0f,
                0f,
                rotationShake
            );
        }
        else
        {
            can.localRotation = Quaternion.Lerp(
                can.localRotation,
                Quaternion.identity,
                Time.deltaTime * 8f
            );
        }
    }

    private void ApplyAlpha(int canIndex, int visualIndex)
    {
        if (!IsValidGroup(canIndex))
            return;

        queueGroups[canIndex].alpha = Mathf.Lerp(
            queueGroups[canIndex].alpha,
            GetTargetAlpha(visualIndex),
            Time.deltaTime * alphaSpeed
        );
    }

    private bool IsHighlightedCan(int visualIndex)
    {
        return visualIndex == 0;
    }

    private bool IsValidCan(int canIndex)
    {
        return queueCans != null &&
               canIndex >= 0 &&
               canIndex < queueCans.Length &&
               queueCans[canIndex] != null;
    }

    private bool IsValidGroup(int canIndex)
    {
        return queueGroups != null &&
               canIndex >= 0 &&
               canIndex < queueGroups.Length &&
               queueGroups[canIndex] != null;
    }
}