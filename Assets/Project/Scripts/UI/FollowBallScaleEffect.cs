using UnityEngine;

public class FollowBallScaleEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform ballVisual;

    [Header("Settings")]
    [SerializeField] private float scaleMultiplier = 1.25f;
    [SerializeField] private float smoothSpeed = 16f;
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.5f;

    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void LateUpdate()
    {
        if (ballVisual == null)
            return;

        float ballScale = ballVisual.localScale.x;
        float targetScale = Mathf.Clamp(
            ballScale * scaleMultiplier,
            minScale,
            maxScale
        );

        Vector3 desiredScale = baseScale * targetScale;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            desiredScale,
            Time.deltaTime * smoothSpeed
        );
    }
}