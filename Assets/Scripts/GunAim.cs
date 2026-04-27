using UnityEngine;
using UnityEngine.InputSystem;

public class GunAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform hipPosition;
    [SerializeField] private Transform aimPosition;

    [Header("Transition")]
    [SerializeField] private float transitionSpeed = 10f;

    private InputAction aimAction;
    private bool isAiming;

    public bool IsAiming => isAiming;

    private void Awake()
    {
        aimAction = new InputAction("Aim", InputActionType.Button, "<Mouse>/rightButton");
    }

    private void Start()
    {
        if (hipPosition != null)
        {
            transform.localPosition = hipPosition.localPosition;
            transform.localRotation = hipPosition.localRotation;
        }
    }

    private void OnEnable()
    {
        aimAction.Enable();
    }

    private void OnDisable()
    {
        aimAction.Disable();
    }

    private void OnDestroy()
    {
        aimAction.Dispose();
    }

    private void Update()
    {
        isAiming = aimAction.IsPressed();

        Transform targetPosition = isAiming ? aimPosition : hipPosition;
        if (targetPosition == null)
        {
            return;
        }

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition.localPosition,
            transitionSpeed * Time.deltaTime
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetPosition.localRotation,
            transitionSpeed * Time.deltaTime
        );
    }
}
