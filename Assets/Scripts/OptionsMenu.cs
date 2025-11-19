using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider masterVolumeSlider;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown; // Opcional

    [Header("Referencias")]
    public MainMenu mainMenu; // Para poder cerrar el panel de opciones

    [Header("Configuración")]
    private const string VOLUME_KEY = "MasterVolume";
    private const string FULLSCREEN_KEY = "Fullscreen";
    private const string RESOLUTION_WIDTH_KEY = "ResolutionWidth";
    private const string RESOLUTION_HEIGHT_KEY = "ResolutionHeight";

    private Resolution[] resolutions;

    private void Start()
    {
        // Cargar valores guardados
        LoadSettings();

        // Configurar listeners
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);

        // Configurar resolución (opcional)
        if (resolutionDropdown != null)
        {
            SetupResolutionDropdown();
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }
    }

    /// <summary>
    /// Configura el dropdown de resolución con las resoluciones disponibles
    /// </summary>
    private void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Maneja el cambio de volumen
    /// </summary>
    public void OnVolumeChanged(float value)
    {
        // Aplicar volumen al AudioListener
        AudioListener.volume = value;
        
        // Guardar en PlayerPrefs
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Maneja el toggle de pantalla completa
    /// </summary>
    public void OnFullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        // Guardar en PlayerPrefs
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Maneja el cambio de resolución
    /// </summary>
    public void OnResolutionChanged(int resolutionIndex)
    {
        if (resolutions != null && resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            
            // Guardar en PlayerPrefs
            PlayerPrefs.SetInt(RESOLUTION_WIDTH_KEY, resolution.width);
            PlayerPrefs.SetInt(RESOLUTION_HEIGHT_KEY, resolution.height);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Carga las configuraciones guardadas desde PlayerPrefs
    /// </summary>
    private void LoadSettings()
    {
        // Cargar volumen (por defecto 1.0 = 100%)
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1.0f);
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = volume;
            AudioListener.volume = volume;
        }

        // Cargar pantalla completa (por defecto true)
        bool fullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = fullscreen;
            Screen.fullScreen = fullscreen;
        }

        // Cargar resolución (opcional)
        if (resolutionDropdown != null)
        {
            int width = PlayerPrefs.GetInt(RESOLUTION_WIDTH_KEY, Screen.currentResolution.width);
            int height = PlayerPrefs.GetInt(RESOLUTION_HEIGHT_KEY, Screen.currentResolution.height);
            
            // Aplicar resolución guardada si existe
            if (width > 0 && height > 0)
            {
                Screen.SetResolution(width, height, Screen.fullScreen);
            }
        }
    }

    /// <summary>
    /// Cierra el panel de opciones y vuelve al menú principal
    /// </summary>
    public void CloseOptions()
    {
        if (mainMenu != null)
        {
            mainMenu.CloseOptionsPanel();
        }
        else
        {
            // Si no hay referencia al MainMenu, simplemente desactivar el panel
            gameObject.SetActive(false);
        }
    }
}

