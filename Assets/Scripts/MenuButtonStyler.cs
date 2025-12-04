using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script para personalizar el estilo visual de los botones del menú
/// Estilo: Botones gris oscuro con borde claro y texto blanco
/// </summary>
public class MenuButtonStyler : MonoBehaviour
{
    [Header("Referencias de Botones")]
    public Button[] buttonsToStyle;

    [Header("Configuración de Estilo")]
    [Tooltip("Color principal del botón (gris claro)")]
    public Color buttonColor = new Color(0.6f, 0.6f, 0.6f, 1f); // Gris claro (definitivamente gris, no negro)
    
    [Tooltip("Color cuando se pasa el mouse")]
    public Color highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f); // Gris más claro
    
    [Tooltip("Color al hacer clic")]
    public Color pressedColor = new Color(0.5f, 0.5f, 0.5f, 1f); // Gris medio
    
    [Tooltip("Color del borde del botón")]
    public Color borderColor = new Color(0.6f, 0.6f, 0.6f, 1f); // Gris claro
    
    [Tooltip("Grosor del borde")]
    [Range(1f, 5f)]
    public float borderWidth = 2f;
    
    [Tooltip("Color del texto")]
    public Color textColor = Color.white;
    
    [Tooltip("Tamaño de la fuente")]
    [Range(12, 48)]
    public int fontSize = 24;
    
    [Tooltip("Estilo de la fuente")]
    public FontStyle fontStyle = FontStyle.Normal;
    
    [Tooltip("Duración de la transición de color")]
    [Range(0f, 1f)]
    public float transitionDuration = 0.1f;

    private void Start()
    {
        // Aplicar estilos con un pequeño delay para asegurar que todo esté inicializado
        Invoke(nameof(ApplyStyles), 0.1f);
    }
    
    private void OnEnable()
    {
        // Reaplicar estilos cuando el objeto se activa
        if (Application.isPlaying)
        {
            Invoke(nameof(ApplyStyles), 0.1f);
        }
    }

    /// <summary>
    /// Aplica los estilos a todos los botones
    /// </summary>
    public void ApplyStyles()
    {
        if (buttonsToStyle == null || buttonsToStyle.Length == 0)
        {
            // Si no hay botones asignados, buscar todos los botones en el Canvas
            buttonsToStyle = FindObjectsOfType<Button>();
            Debug.Log($"MenuButtonStyler: Encontrados {buttonsToStyle.Length} botones automáticamente");
        }

        foreach (Button button in buttonsToStyle)
        {
            if (button != null)
            {
                StyleButton(button);
            }
        }

        Debug.Log($"MenuButtonStyler: Estilos aplicados a {buttonsToStyle.Length} botones");
    }

    /// <summary>
    /// Aplica el estilo a un botón específico
    /// </summary>
    private void StyleButton(Button button)
    {
        // IMPORTANTE: Primero configurar el Image a blanco para que ColorTint funcione
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            // El Image debe estar en blanco para que el ColorTint del Button funcione correctamente
            buttonImage.color = Color.white;
        }

        // Configurar colores del botón usando ColorBlock
        ColorBlock colors = button.colors;
        colors.normalColor = buttonColor;
        colors.highlightedColor = highlightedColor;
        colors.pressedColor = pressedColor;
        colors.selectedColor = highlightedColor;
        colors.disabledColor = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 0.5f);
        colors.fadeDuration = transitionDuration;
        button.colors = colors;

        // Configurar transición de color (ColorTint multiplica el color del Image)
        button.transition = Selectable.Transition.ColorTint;
        
        // Forzar actualización del color aplicando el normalColor inmediatamente
        if (buttonImage != null)
        {
            // Aplicar el color directamente también como respaldo
            buttonImage.color = buttonColor;
        }
        
        Debug.Log($"MenuButtonStyler: Botón '{button.name}' configurado - Color: {buttonColor}");

        // Configurar el texto del botón
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.color = textColor;
            buttonText.fontSize = fontSize;
            buttonText.fontStyle = fontStyle;
            // Asegurar que el texto esté en mayúsculas (opcional)
            // buttonText.text = buttonText.text.ToUpper();
        }
        else
        {
            // Intentar con TextMeshProUGUI si está disponible
            TMPro.TextMeshProUGUI tmpText = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.color = textColor;
                tmpText.fontSize = fontSize;
                tmpText.fontStyle = (TMPro.FontStyles)fontStyle;
            }
        }

        // Agregar borde
        AddBorder(button);
    }

    /// <summary>
    /// Agrega un borde al botón usando Outline
    /// </summary>
    private void AddBorder(Button button)
    {
        // Remover Outline existente si hay uno
        Outline existingOutline = button.GetComponent<Outline>();
        if (existingOutline != null)
        {
            DestroyImmediate(existingOutline);
        }

        // Agregar nuevo Outline para el borde
        Outline outline = button.gameObject.AddComponent<Outline>();
        outline.effectColor = borderColor;
        outline.effectDistance = new Vector2(borderWidth, borderWidth);
        outline.useGraphicAlpha = true;
    }

    /// <summary>
    /// Método público para aplicar estilos manualmente (útil para testing)
    /// </summary>
    [ContextMenu("Aplicar Estilos")]
    public void ApplyStylesManual()
    {
        ApplyStyles();
    }

    /// <summary>
    /// Aplica un estilo predefinido similar a la imagen
    /// </summary>
    [ContextMenu("Aplicar Estilo de Imagen")]
    public void ApplyImageStyle()
    {
        // Valores actualizados: botones gris claro (definitivamente gris, no negro)
        buttonColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        pressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        borderColor = new Color(0.8f, 0.8f, 0.8f, 1f); // Borde más claro
        borderWidth = 2f;
        textColor = Color.white;
        fontSize = 24;
        fontStyle = FontStyle.Normal;
        transitionDuration = 0.1f;
        
        ApplyStyles();
        Debug.Log("MenuButtonStyler: Estilo de imagen aplicado (gris claro)");
    }
}

