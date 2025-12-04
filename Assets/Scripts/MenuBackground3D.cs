using UnityEngine;

/// <summary>
/// Script avanzado para fondos 3D en el menú.
/// Permite tener una cámara separada que renderiza solo el fondo 3D,
/// mientras la cámara principal renderiza la UI.
/// </summary>
public class MenuBackground3D : MonoBehaviour
{
    [Header("Referencias")]
    public Camera backgroundCamera;
    public Camera mainCamera; // Cámara principal (UI)
    
    [Header("Objetos 3D del Fondo")]
    public GameObject[] backgroundObjects;
    public Transform rotationPivot; // Punto de rotación (opcional)

    [Header("Animación")]
    public bool enableRotation = true;
    public Vector3 rotationSpeed = new Vector3(0, 10, 0); // Grados por segundo
    public bool enableOrbit = false;
    public float orbitRadius = 5f;
    public float orbitSpeed = 1f;
    public Vector3 orbitAxis = Vector3.up;

    [Header("Iluminación")]
    public Light[] backgroundLights;
    public bool animateLights = false;
    public float lightIntensityVariation = 0.2f;
    public float lightSpeed = 1f;

    [Header("Configuración de Cámara")]
    public bool autoSetupCamera = true;
    public int backgroundCameraDepth = -1; // Renderizar antes que la cámara principal
    public LayerMask backgroundLayer = -1; // Layer para objetos del fondo

    private Vector3 initialCameraPosition;
    private float orbitAngle = 0f;
    private float[] initialLightIntensities;

    private void Start()
    {
        SetupCameras();
        SetupBackgroundObjects();
        SetupLights();
    }

    private void Update()
    {
        if (enableRotation)
        {
            RotateBackground();
        }

        if (enableOrbit && backgroundCamera != null)
        {
            OrbitCamera();
        }

        if (animateLights)
        {
            AnimateLights();
        }
    }

    /// <summary>
    /// Configura las cámaras para renderizar correctamente
    /// </summary>
    private void SetupCameras()
    {
        // Buscar cámara principal si no está asignada
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }

        // Configurar cámara de fondo
        if (backgroundCamera != null && autoSetupCamera)
        {
            backgroundCamera.depth = backgroundCameraDepth;
            backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
            backgroundCamera.cullingMask = backgroundLayer;
            
            // Asegurar que la cámara principal renderice solo UI
            if (mainCamera != null)
            {
                mainCamera.clearFlags = CameraClearFlags.Depth;
                mainCamera.cullingMask = ~backgroundLayer; // Todo excepto el fondo
            }
        }

        if (backgroundCamera != null)
        {
            initialCameraPosition = backgroundCamera.transform.position;
        }
    }

    /// <summary>
    /// Configura los objetos del fondo
    /// </summary>
    private void SetupBackgroundObjects()
    {
        // Asignar objetos a la layer del fondo
        if (backgroundObjects != null)
        {
            foreach (GameObject obj in backgroundObjects)
            {
                if (obj != null)
                {
                    obj.layer = GetLayerFromMask(backgroundLayer);
                }
            }
        }
    }

    /// <summary>
    /// Configura las luces del fondo
    /// </summary>
    private void SetupLights()
    {
        if (backgroundLights != null && backgroundLights.Length > 0)
        {
            initialLightIntensities = new float[backgroundLights.Length];
            for (int i = 0; i < backgroundLights.Length; i++)
            {
                if (backgroundLights[i] != null)
                {
                    initialLightIntensities[i] = backgroundLights[i].intensity;
                }
            }
        }
    }

    /// <summary>
    /// Rota los objetos del fondo
    /// </summary>
    private void RotateBackground()
    {
        if (backgroundObjects == null) return;

        Vector3 rotation = rotationSpeed * Time.deltaTime;

        if (rotationPivot != null)
        {
            // Rotar alrededor de un punto específico
            foreach (GameObject obj in backgroundObjects)
            {
                if (obj != null)
                {
                    obj.transform.RotateAround(rotationPivot.position, rotationPivot.up, rotation.y);
                    obj.transform.Rotate(rotation, Space.Self);
                }
            }
        }
        else
        {
            // Rotar cada objeto individualmente
            foreach (GameObject obj in backgroundObjects)
            {
                if (obj != null)
                {
                    obj.transform.Rotate(rotation, Space.World);
                }
            }
        }
    }

    /// <summary>
    /// Hace orbitar la cámara alrededor del punto de rotación
    /// </summary>
    private void OrbitCamera()
    {
        if (backgroundCamera == null || rotationPivot == null) return;

        orbitAngle += orbitSpeed * Time.deltaTime;

        Vector3 offset = new Vector3(
            Mathf.Cos(orbitAngle) * orbitRadius,
            0,
            Mathf.Sin(orbitAngle) * orbitRadius
        );

        // Rotar el offset según el eje de órbita
        if (orbitAxis != Vector3.up)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, orbitAxis);
            offset = rotation * offset;
        }

        backgroundCamera.transform.position = rotationPivot.position + offset;
        backgroundCamera.transform.LookAt(rotationPivot.position);
    }

    /// <summary>
    /// Anima las luces del fondo
    /// </summary>
    private void AnimateLights()
    {
        if (backgroundLights == null || initialLightIntensities == null) return;

        float time = Time.time * lightSpeed;

        for (int i = 0; i < backgroundLights.Length; i++)
        {
            if (backgroundLights[i] != null && i < initialLightIntensities.Length)
            {
                float variation = Mathf.Sin(time + i) * lightIntensityVariation;
                backgroundLights[i].intensity = initialLightIntensities[i] + variation;
            }
        }
    }

    /// <summary>
    /// Obtiene el número de layer desde un LayerMask
    /// </summary>
    private int GetLayerFromMask(LayerMask mask)
    {
        int layerNumber = 0;
        int layer = mask.value;
        while (layer > 1)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber;
    }

    /// <summary>
    /// Resetea la posición de la cámara de fondo
    /// </summary>
    public void ResetCameraPosition()
    {
        if (backgroundCamera != null)
        {
            backgroundCamera.transform.position = initialCameraPosition;
            orbitAngle = 0f;
        }
    }

    /// <summary>
    /// Activa/desactiva la rotación
    /// </summary>
    public void SetRotationEnabled(bool enabled)
    {
        enableRotation = enabled;
    }

    /// <summary>
    /// Activa/desactiva la órbita
    /// </summary>
    public void SetOrbitEnabled(bool enabled)
    {
        enableOrbit = enabled;
    }
}

