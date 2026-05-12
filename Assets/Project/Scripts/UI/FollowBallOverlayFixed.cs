using UnityEngine;

public class FollowBallOverlayFixed : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform target;

    [Header("Fixed Visual")]
    [SerializeField] private Vector3 fixedLocalScale = new Vector3(0.75f, 0.75f, 1f);
    [SerializeField] private bool followRotation = false;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position;

        if (followRotation)
            transform.rotation = target.rotation;

        transform.localScale = fixedLocalScale;
    }
}