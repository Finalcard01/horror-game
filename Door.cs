using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;
    [SerializeField]
    private bool IsRotatingDoor = true;
    [SerializeField]
    private float Speed = 1f;
    [Header("Rotation Configs")]
    [SerializeField]
    private float RotationAmount = 90f;
    [SerializeField]
    private float ForwardDirection = 0;

    private UnityEngine.Vector3 StartRotation;
    private UnityEngine.Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        // Since "forward" actually is pointing into the door frame, choose a direction to think about as "forward"
        Forward = transform.right;
    }

    public void Open(UnityEngine.Vector3 UserPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            if (IsRotatingDoor)
            {
                float dot = UnityEngine.Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
                Debug.Log($"Dot:{dot.ToString("N3")}");
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));

            }
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        UnityEngine.Quaternion startRotation = transform.rotation;
        UnityEngine.Quaternion endRotation;

        if (ForwardAmount >= ForwardDirection)
        {
            endRotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, StartRotation.y + RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = UnityEngine.Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;

        }

    }
    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            if (IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }
    private IEnumerator DoRotationClose()
    {
        UnityEngine.Quaternion startRotation = transform.rotation;
        UnityEngine.Quaternion endRotation = UnityEngine.Quaternion.Euler(StartRotation);

        IsOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = UnityEngine.Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }
}
