using UnityEngine;
using UnityEngine.InputSystem;

public class ShootRaycast : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private GunAim gunAim;

    [Header("Shoot")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float maxDistance = 100f;

    [Header("Visual")]
    [SerializeField] private bool showShotLine = true;
    [SerializeField] private float shotLineDuration = 0.05f;
    [SerializeField] private float shotLineWidth = 0.03f;
    [SerializeField] private Color shotLineColor = new Color(0.1f, 0.8f, 1f, 1f);

    private InputAction shootAction;
    private float nextFireTime;
    private LineRenderer shotLineRenderer;
    private Material shotLineMaterial;
    private float shotLineDisableTime;

    private void Awake()
    {
        shootAction = new InputAction("Shoot", InputActionType.Button, "<Mouse>/leftButton");

        if (shootOrigin == null)
        {
            shootOrigin = transform;
        }

        if (showShotLine)
        {
            ConfigureShotLine();
        }
    }

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    private void OnDestroy()
    {
        shootAction.Dispose();

        if (shotLineMaterial == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(shotLineMaterial);
            return;
        }

        DestroyImmediate(shotLineMaterial);
    }

    private void Update()
    {
        UpdateShotLine();

        if (!shootAction.IsPressed())
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        if (gunAim != null && !gunAim.IsAiming)
        {
            return;
        }

        Shoot();
    }

    private void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        Debug.Log("Disparo realizado");

        if (Physics.Raycast(
            shootOrigin.position,
            shootOrigin.forward,
            out RaycastHit hit,
            maxDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            Health targetHealth = hit.collider.GetComponentInParent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }

            ShowShotLine(hit.point);
            Debug.DrawRay(shootOrigin.position, shootOrigin.forward * hit.distance, Color.red, 1f);
            Debug.Log("Hit: " + hit.collider.name + " | Point: " + hit.point + " | Distance: " + hit.distance);
            return;
        }

        ShowShotLine(shootOrigin.position + (shootOrigin.forward * maxDistance));
        Debug.DrawRay(shootOrigin.position, shootOrigin.forward * maxDistance, Color.white, 1f);
    }

    private void ConfigureShotLine()
    {
        shotLineRenderer = GetComponent<LineRenderer>();

        if (shotLineRenderer == null)
        {
            shotLineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        shotLineRenderer.enabled = false;
        shotLineRenderer.useWorldSpace = true;
        shotLineRenderer.positionCount = 2;
        shotLineRenderer.startWidth = shotLineWidth;
        shotLineRenderer.endWidth = shotLineWidth;
        shotLineRenderer.startColor = shotLineColor;
        shotLineRenderer.endColor = shotLineColor;
        shotLineRenderer.numCapVertices = 4;

        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
        {
            shader = Shader.Find("Unlit/Color");
        }

        if (shader != null)
        {
            shotLineMaterial = new Material(shader);

            if (shotLineMaterial.HasProperty("_Color"))
            {
                shotLineMaterial.color = shotLineColor;
            }

            shotLineRenderer.material = shotLineMaterial;
        }
    }

    private void ShowShotLine(Vector3 endPoint)
    {
        if (!showShotLine || shotLineRenderer == null)
        {
            return;
        }

        shotLineRenderer.startWidth = shotLineWidth;
        shotLineRenderer.endWidth = shotLineWidth;
        shotLineRenderer.startColor = shotLineColor;
        shotLineRenderer.endColor = shotLineColor;
        shotLineRenderer.SetPosition(0, shootOrigin.position);
        shotLineRenderer.SetPosition(1, endPoint);
        shotLineRenderer.enabled = true;
        shotLineDisableTime = Time.time + shotLineDuration;
    }

    private void UpdateShotLine()
    {
        if (shotLineRenderer == null || !shotLineRenderer.enabled)
        {
            return;
        }

        if (Time.time >= shotLineDisableTime)
        {
            shotLineRenderer.enabled = false;
        }
    }
}