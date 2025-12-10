using UnityEngine;

/// <summary>
/// Script para manejar eventos de animación
/// Permite que la animación del ataque llame a métodos de daño en el momento preciso
/// </summary>
public class AnimationEventHandler : MonoBehaviour
{
    private LogicaPlayer logicaPlayer;
    private Enemigo1 enemigo1;

    void Start()
    {
        // Obtener referencias a los scripts
        logicaPlayer = GetComponent<LogicaPlayer>();
        enemigo1 = GetComponent<Enemigo1>();
    }

    /// <summary>
    /// Llamado desde la animación de ataque del jugador
    /// Añade un "Animation Event" en el frame donde la espada golpea
    /// </summary>
    public void OnAttackHit()
    {
        if (logicaPlayer != null)
        {
            logicaPlayer.GolpearEnemigo();
        }
    }

    /// <summary>
    /// Llamado desde la animación de ataque del esqueleto
    /// Añade un "Animation Event" en el frame donde el esqueleto golpea
    /// </summary>
    public void OnEnemyAttackHit()
    {
        if (enemigo1 != null)
        {
            enemigo1.DañarAlJugador();
        }
    }
}
