using UnityEngine;

public class FollowPositionOnly : MonoBehaviour
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private Transform targetToFollow;

    private void LateUpdate()
    {
        if (targetToFollow == null || settings == null) return;

        transform.position = targetToFollow.position + settings.pivotOffset;
        transform.rotation = Quaternion.identity;
    }
}