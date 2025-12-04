using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Script para manejar diferentes tipos de fondo en el menú:
/// - Imagen 2D
/// - Escena 3D
/// - Video/Cinemática
/// </summary>
public class MenuBackground : MonoBehaviour
{
    public enum BackgroundType
    {
        Image2D,
        Scene3D,
        Video
    }

    [Header("Tipo de Fondo")]
    public BackgroundType backgroundType = BackgroundType.Image2D;

    [Header("Fondo 2D - Imagen")]
    public UnityEngine.UI.Image backgroundImage;
    public Sprite backgroundSprite;

    [Header("Fondo 3D - Escena")]
    public Camera backgroundCamera;
    public GameObject scene3DContainer; // Contenedor con objetos 3D
    public bool rotateScene = false;
    public float rotationSpeed = 10f;
    public Vector3 rotationAxis = Vector3.up;

    [Header("Fondo Video/Cinemática")]
    public VideoPlayer videoPlayer;
    public RenderTexture videoRenderTexture;
    public UnityEngine.UI.RawImage videoRawImage;

    [Header("Configuración General")]
    public bool fadeInOnStart = true;
    public float fadeInDuration = 1f;

    private CanvasGroup canvasGroup;
    private float currentRotation = 0f;
    private bool videoSetupComplete = false;

    private void Awake()
    {
        // Configurar el fondo lo antes posible
        if (backgroundType == BackgroundType.Video)
        {
            SetupVideoBackground();
        }
    }

    private void Start()
    {
        // Si no es video, configurar en Start
        if (backgroundType != BackgroundType.Video)
        {
            SetupBackground();
        }
        
        // No hacer fade in si es video, ya que puede ocultar el video
        if (fadeInOnStart && backgroundType != BackgroundType.Video)
        {
            StartFadeIn();
        }
        
        // Diagnóstico completo
        if (backgroundType == BackgroundType.Video)
        {
            DebugDiagnostics();
        }
    }
    
    /// <summary>
    /// Diagnóstico completo del sistema de video
    /// </summary>
    private void DebugDiagnostics()
    {
        Debug.Log("=== DIAGNÓSTICO DE VIDEO ===");
        
        if (videoPlayer == null)
        {
            Debug.LogError("❌ VideoPlayer: NO ASIGNADO");
        }
        else
        {
            Debug.Log($"✅ VideoPlayer: Asignado y activo: {videoPlayer.gameObject.activeSelf}");
            if (videoPlayer.clip == null)
            {
                Debug.LogError("❌ VideoClip: NO ASIGNADO en VideoPlayer");
            }
            else
            {
                Debug.Log($"✅ VideoClip: {videoPlayer.clip.name} ({videoPlayer.clip.width}x{videoPlayer.clip.height})");
            }
            Debug.Log($"   - Is Playing: {videoPlayer.isPlaying}");
            Debug.Log($"   - Is Prepared: {videoPlayer.isPrepared}");
            Debug.Log($"   - Is Looping: {videoPlayer.isLooping}");
            Debug.Log($"   - Play On Awake: {videoPlayer.playOnAwake}");
        }
        
        if (videoRenderTexture == null)
        {
            Debug.LogError("❌ RenderTexture: NO ASIGNADO");
        }
        else
        {
            Debug.Log($"✅ RenderTexture: Asignado ({videoRenderTexture.width}x{videoRenderTexture.height})");
            Debug.Log($"   - Is Created: {videoRenderTexture.IsCreated()}");
        }
        
        if (videoRawImage == null)
        {
            Debug.LogError("❌ RawImage: NO ASIGNADO");
        }
        else
        {
            Debug.Log($"✅ RawImage: Asignado y activo: {videoRawImage.gameObject.activeSelf}");
            Debug.Log($"   - Texture asignado: {videoRawImage.texture != null}");
            Debug.Log($"   - Color: {videoRawImage.color}");
            
            RectTransform rect = videoRawImage.GetComponent<RectTransform>();
            if (rect != null)
            {
                Debug.Log($"   - Anchors: Min={rect.anchorMin}, Max={rect.anchorMax}");
                Debug.Log($"   - Size Delta: {rect.sizeDelta}");
                Debug.Log($"   - Position: {rect.anchoredPosition}");
            }
        }
        
        Debug.Log("=== FIN DIAGNÓSTICO ===");
    }

    private void OnEnable()
    {
        // Asegurar que el video se reproduzca cuando el objeto se active
        if (backgroundType == BackgroundType.Video && videoPlayer != null && !videoPlayer.isPlaying)
        {
            if (videoSetupComplete)
            {
                videoPlayer.Play();
            }
        }
    }

    private void Update()
    {
        // Rotar escena 3D si está habilitado
        if (backgroundType == BackgroundType.Scene3D && rotateScene && scene3DContainer != null)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            scene3DContainer.transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
        }

        // Verificar que el video siga reproduciéndose (por si se detiene)
        if (backgroundType == BackgroundType.Video && videoPlayer != null && videoSetupComplete)
        {
            if (!videoPlayer.isPlaying && videoPlayer.clip != null)
            {
                Debug.LogWarning("MenuBackground: Video se detuvo, reiniciando...");
                videoPlayer.Play();
            }
            
            // Verificar que el RawImage tenga la textura asignada
            if (videoRawImage != null && videoRawImage.texture == null && videoRenderTexture != null)
            {
                Debug.LogWarning("MenuBackground: RawImage perdió la textura, reasignando...");
                videoRawImage.texture = videoRenderTexture;
            }
        }
    }

    /// <summary>
    /// Configura el fondo según el tipo seleccionado
    /// </summary>
    private void SetupBackground()
    {
        switch (backgroundType)
        {
            case BackgroundType.Image2D:
                SetupImageBackground();
                break;
            case BackgroundType.Scene3D:
                Setup3DBackground();
                break;
            case BackgroundType.Video:
                SetupVideoBackground();
                break;
        }
    }

    /// <summary>
    /// Configura el fondo de imagen 2D
    /// </summary>
    private void SetupImageBackground()
    {
        if (backgroundImage != null && backgroundSprite != null)
        {
            backgroundImage.sprite = backgroundSprite;
            backgroundImage.gameObject.SetActive(true);
        }
        else if (backgroundImage != null)
        {
            Debug.LogWarning("MenuBackground: backgroundSprite no está asignado para fondo 2D");
        }
        else
        {
            Debug.LogWarning("MenuBackground: backgroundImage no está asignado para fondo 2D");
        }

        // Desactivar otros tipos de fondo
        if (backgroundCamera != null)
            backgroundCamera.gameObject.SetActive(false);
        if (videoPlayer != null)
            videoPlayer.gameObject.SetActive(false);
    }

    /// <summary>
    /// Configura el fondo 3D
    /// </summary>
    private void Setup3DBackground()
    {
        if (backgroundCamera != null)
        {
            // Configurar cámara para renderizar solo el fondo
            backgroundCamera.depth = -1; // Renderizar antes que la cámara principal
            backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
            backgroundCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("MenuBackground: backgroundCamera no está asignado para fondo 3D");
        }

        if (scene3DContainer != null)
        {
            scene3DContainer.SetActive(true);
        }

        // Desactivar otros tipos de fondo
        if (backgroundImage != null)
            backgroundImage.gameObject.SetActive(false);
        if (videoPlayer != null)
            videoPlayer.gameObject.SetActive(false);
    }

    /// <summary>
    /// Configura el fondo de video
    /// </summary>
    private void SetupVideoBackground()
    {
        if (videoPlayer == null)
        {
            Debug.LogWarning("MenuBackground: videoPlayer no está asignado para fondo de video");
            return;
        }

        // Asegurar que el VideoPlayer esté activo
        videoPlayer.gameObject.SetActive(true);

        // Configurar propiedades del video
        videoPlayer.isLooping = true; // Reproducir en loop
        videoPlayer.playOnAwake = true; // Reproducir automáticamente
        
        // CONFIGURACIÓN CRÍTICA: Asegurar que el VideoPlayer renderice al RenderTexture
        if (videoRenderTexture != null)
        {
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = videoRenderTexture;
            Debug.Log("MenuBackground: VideoPlayer configurado para renderizar al RenderTexture");
        }
        else
        {
            Debug.LogError("MenuBackground: RenderTexture no está asignado - el video no se mostrará!");
        }
        
        // Ajustar RenderTexture al tamaño del video si es necesario
        if (videoPlayer.clip != null && videoRenderTexture != null)
        {
            uint width = videoPlayer.clip.width;
            uint height = videoPlayer.clip.height;
            
            // Solo ajustar si el tamaño es diferente
            if (videoRenderTexture.width != (int)width || videoRenderTexture.height != (int)height)
            {
                Debug.Log($"Ajustando RenderTexture a tamaño del video: {width}x{height}");
                videoRenderTexture.Release();
                videoRenderTexture.width = (int)width;
                videoRenderTexture.height = (int)height;
                videoRenderTexture.Create();
                
                // Reasignar el RenderTexture al VideoPlayer después de recrearlo
                videoPlayer.targetTexture = videoRenderTexture;
            }
        }

        // Configurar RawImage
        if (videoRawImage != null && videoRenderTexture != null)
        {
            // Asegurar que el RawImage esté activo PRIMERO
            videoRawImage.gameObject.SetActive(true);
            
            // Asignar la textura
            videoRawImage.texture = videoRenderTexture;
            
            // Asegurar que el RawImage cubra toda la pantalla
            RectTransform rectTransform = videoRawImage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.localScale = Vector3.one;
            }
            
            // Asegurar que el color sea opaco y visible
            videoRawImage.color = Color.white;
            videoRawImage.raycastTarget = false; // No necesario para el fondo
            
            // Asegurar que esté en la posición correcta del Canvas
            videoRawImage.transform.SetAsFirstSibling(); // Mover al principio
            
            Debug.Log("MenuBackground: RawImage configurado correctamente");
        }
        else
        {
            if (videoRawImage == null)
                Debug.LogError("MenuBackground: videoRawImage no está asignado para fondo de video");
            if (videoRenderTexture == null)
                Debug.LogError("MenuBackground: videoRenderTexture no está asignado para fondo de video");
        }

        // Desactivar otros tipos de fondo
        if (backgroundImage != null)
            backgroundImage.gameObject.SetActive(false);
        if (backgroundCamera != null)
            backgroundCamera.gameObject.SetActive(false);

        // Forzar reproducción del video
        StartCoroutine(StartVideoPlayback());
    }

    /// <summary>
    /// Corrutina para asegurar que el video se reproduzca correctamente
    /// </summary>
    private System.Collections.IEnumerator StartVideoPlayback()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("MenuBackground: VideoPlayer no está asignado");
            yield break;
        }
        
        if (videoPlayer.clip == null)
        {
            Debug.LogError("MenuBackground: VideoClip no está asignado en el VideoPlayer");
            yield break;
        }

        Debug.Log($"MenuBackground: Preparando video '{videoPlayer.clip.name}'...");

        // Esperar a que el video esté preparado
        videoPlayer.Prepare();
        float timeout = 10f; // Timeout de 10 segundos
        float elapsed = 0f;
        
        while (!videoPlayer.isPrepared && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!videoPlayer.isPrepared)
        {
            Debug.LogError("MenuBackground: El video no se pudo preparar. Verifica el formato del video.");
            yield break;
        }

        Debug.Log("MenuBackground: Video preparado, iniciando reproducción...");

        // Reproducir el video
        videoPlayer.Play();
        videoSetupComplete = true;
        
        // Verificar que el video esté reproduciéndose
        yield return new WaitForSeconds(0.5f);
        if (videoPlayer.isPlaying)
        {
            Debug.Log("MenuBackground: Video iniciado correctamente en loop");
        }
        else
        {
            Debug.LogWarning("MenuBackground: El video no se está reproduciendo, intentando de nuevo...");
            videoPlayer.Play();
            
            yield return new WaitForSeconds(0.5f);
            if (videoPlayer.isPlaying)
            {
                Debug.Log("MenuBackground: Video iniciado en segundo intento");
            }
            else
            {
                Debug.LogError("MenuBackground: No se pudo iniciar el video. Verifica la configuración del VideoPlayer.");
            }
        }
    }

    /// <summary>
    /// Inicia el efecto de fade in
    /// </summary>
    private void StartFadeIn()
    {
        // Buscar CanvasGroup en el Canvas o crear uno
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
            }

            StartCoroutine(FadeInCoroutine());
        }
    }

    /// <summary>
    /// Corrutina para el fade in
    /// </summary>
    private System.Collections.IEnumerator FadeInCoroutine()
    {
        if (canvasGroup == null) yield break;

        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// Cambia el tipo de fondo en tiempo de ejecución
    /// </summary>
    public void ChangeBackgroundType(BackgroundType newType)
    {
        backgroundType = newType;
        SetupBackground();
    }

    /// <summary>
    /// Cambia la imagen del fondo 2D
    /// </summary>
    public void ChangeBackgroundImage(Sprite newSprite)
    {
        backgroundSprite = newSprite;
        if (backgroundType == BackgroundType.Image2D && backgroundImage != null)
        {
            backgroundImage.sprite = newSprite;
        }
    }
}

