using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Referencias de Botones")]
    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button optionsButton;
    public UnityEngine.UI.Button quitButton;
    public UnityEngine.UI.Button creditsButton; // Opcional

    [Header("Paneles")]
    public GameObject optionsPanel;

    [Header("Configuración")]
    public string gameSceneName = "SampleSceneJuanDavid"; // Nombre de la escena de juego

    private void Start()
    {
        // Configurar listeners de botones
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            Debug.Log("PlayButton configurado");
        }
        else
        {
            Debug.LogWarning("PlayButton no está asignado en MainMenu!");
        }

        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            Debug.Log("OptionsButton configurado");
        }
        else
        {
            Debug.LogWarning("OptionsButton no está asignado en MainMenu!");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            Debug.Log("QuitButton configurado");
        }
        else
        {
            Debug.LogWarning("QuitButton no está asignado en MainMenu!");
        }

        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(OnCreditsButtonClicked);
            Debug.Log("CreditsButton configurado");
        }

        // Asegurar que el panel de opciones esté desactivado al inicio
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            Debug.Log("OptionsPanel desactivado al inicio");
        }
        else
        {
            Debug.LogWarning("OptionsPanel no está asignado en MainMenu!");
        }
    }

    /// <summary>
    /// Carga la escena del juego cuando se presiona el botón Play
    /// </summary>
    public void OnPlayButtonClicked()
    {
        Debug.Log($"Cargando escena: {gameSceneName}");
        
        // Cargar la escena en modo Single para reemplazar completamente la escena actual
        // LoadSceneMode.Single descarga automáticamente la escena anterior, incluyendo su cámara
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Muestra el panel de opciones
    /// </summary>
    public void OnOptionsButtonClicked()
    {
        Debug.Log("Botón de Opciones presionado");
        
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            Debug.Log("Panel de opciones activado");
        }
        else
        {
            Debug.LogError("OptionsPanel no está asignado en MainMenu!");
        }
    }

    /// <summary>
    /// Cierra el panel de opciones (llamado desde OptionsMenu)
    /// </summary>
    public void CloseOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Maneja el botón de créditos (opcional)
    /// </summary>
    public void OnCreditsButtonClicked()
    {
        // Aquí puedes agregar lógica para mostrar créditos
        Debug.Log("Créditos del juego");
    }

    /// <summary>
    /// Cierra la aplicación cuando se presiona el botón Quit
    /// </summary>
    public void OnQuitButtonClicked()
    {
        Debug.Log("Saliendo del juego...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

