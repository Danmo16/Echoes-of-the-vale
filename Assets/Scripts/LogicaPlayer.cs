using UnityEngine;

public class LogicaPlayer : MonoBehaviour
{
    public int velCorrer = 8;

    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;

    private Animator anim;
    public float x, y; 

    public Rigidbody rb;
    public float fuerzaSalto = 8f;
    public bool puedoSaltar;

    public float velocidadInicial;
    public float velocidadAgachado;

    public bool estoyAtacando;
    public bool avanzoSolo;
    public float impulsoGolpe = 10f;

    public float dañoAlEnemigo = 10f;
    private Enemigo1 enemigoEnRango;

    // ====== SISTEMA DE AUDIO ======
    // AudioSource separados para mejor control
    public AudioSource audioSourceMovimiento;  // Para caminar/correr (loop)
    public AudioSource audioSourceAcciones;    // Para saltar/golpear (una vez)
    
    [Header("Sonidos")]
    public AudioClip sonidoCaminar;
    public AudioClip sonidoCorrer;
    public AudioClip sonidoSaltar;
    public AudioClip sonidoGolpear;

    void Start()
    {
        puedoSaltar = false;
        anim = GetComponent<Animator>();

        velocidadInicial = velocidadMovimiento;
        velocidadAgachado = velocidadMovimiento * 0.5f;

        estoyAtacando = false;

        // Crear AudioSources si no existen
        ConfigurarAudioSources();
    }

    void ConfigurarAudioSources()
    {
        // AudioSource para movimiento (loop)
        if (audioSourceMovimiento == null)
        {
            audioSourceMovimiento = gameObject.AddComponent<AudioSource>();
        }
        audioSourceMovimiento.loop = true;
        audioSourceMovimiento.playOnAwake = false;

        // AudioSource para acciones (una vez)
        if (audioSourceAcciones == null)
        {
            audioSourceAcciones = gameObject.AddComponent<AudioSource>();
        }
        audioSourceAcciones.loop = false;
        audioSourceAcciones.playOnAwake = false;
    }

    void FixedUpdate()
    {
        // Rotación y movimiento
        if (!estoyAtacando)
        {
            transform.Rotate(0, x * velocidadRotacion * Time.deltaTime, 0);
            transform.Translate(0, 0, y * velocidadMovimiento * Time.deltaTime);
        }

        if (avanzoSolo)
        {
            rb.linearVelocity = transform.forward * impulsoGolpe;
        }
    }

    void Update()
    {
        // Leer las teclas (W, A, S, D)
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // CORRER
        if (Input.GetKey(KeyCode.LeftShift) && y > 0 && puedoSaltar && !estoyAtacando)
        {
            velocidadMovimiento = 10.0f;
            anim.SetBool("correr", true);
            
            // Reproducir sonido de correr
            ReproducirSonidoMovimiento(sonidoCorrer);
        }
        else
        {
            anim.SetBool("correr", false);

            // Lógica de Agachado vs Caminar Normal
            if (Input.GetKey(KeyCode.LeftControl))
            {
                velocidadMovimiento = velocidadAgachado;
                anim.SetBool("agachado", true);
            }
            else
            {
                velocidadMovimiento = velocidadInicial;
                anim.SetBool("agachado", false);
            }

            // CAMINAR - Reproducir sonido solo si se está moviendo
            if (puedoSaltar && (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f) && !estoyAtacando)
            {
                ReproducirSonidoMovimiento(sonidoCaminar);
            }
            else
            {
                DetenerSonidoMovimiento();
            }
        }

        // Actualizar animaciones
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        // ATAQUE
        if (Input.GetKeyDown(KeyCode.Return) && puedoSaltar && !estoyAtacando)
        {
            anim.SetTrigger("golpeo");
            estoyAtacando = true;
            
            // Reproducir sonido de golpe
            ReproducirSonidoAccion(sonidoGolpear);
            
            // Llamar directamente para asegurar que el daño se inflige
            GolpearEnemigo();
        }

        // SALTO
        if (puedoSaltar)
        {
            if (!estoyAtacando)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetBool("salte", true);
                    rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
                    
                    // Reproducir sonido de salto
                    ReproducirSonidoAccion(sonidoSaltar);
                    DetenerSonidoMovimiento();
                }
                anim.SetBool("tocoSuelo", true);

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    anim.SetBool("agachado", true);
                }
                else
                {
                    anim.SetBool("agachado", false);
                }
            }

            anim.SetBool("tocoSuelo", true);
        }
        else
        {
            EstoyCayendo();
            DetenerSonidoMovimiento();
        }
    }

    // ====== MÉTODOS DE AUDIO ======
    
    // Para sonidos continuos (caminar, correr) - usa audioSourceMovimiento
    void ReproducirSonidoMovimiento(AudioClip clip)
    {
        if (clip == null || audioSourceMovimiento == null) return;

        // Si no está reproduciendo o es un clip diferente, cambiarlo
        if (!audioSourceMovimiento.isPlaying || audioSourceMovimiento.clip != clip)
        {
            audioSourceMovimiento.clip = clip;
            audioSourceMovimiento.Play();
        }
    }

    // Detener sonidos de movimiento
    void DetenerSonidoMovimiento()
    {
        if (audioSourceMovimiento != null && audioSourceMovimiento.isPlaying)
        {
            audioSourceMovimiento.Stop();
        }
    }

    // Para sonidos de una sola vez (saltar, golpear) - usa audioSourceAcciones
    void ReproducirSonidoAccion(AudioClip clip)
    {
        if (clip != null && audioSourceAcciones != null)
        {
            audioSourceAcciones.PlayOneShot(clip);
        }
    }

    // ====== MÉTODOS EXISTENTES ======

    public void EstoyCayendo()
    {
        anim.SetBool("tocoSuelo", false);
        anim.SetBool("salte", false);
    }

    public void DejoDeGolpear()
    {
        estoyAtacando = false;
    }

    public void AvanzoSolo()
    {
        avanzoSolo = true;
    }

    public void DejoDeAvanzar()
    {
        avanzoSolo = false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("enemigo"))
        {
            enemigoEnRango = coll.GetComponent<Enemigo1>();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("enemigo"))
        {
            enemigoEnRango = null;
        }
    }

    public void GolpearEnemigo()
    {
        if (enemigoEnRango != null && estoyAtacando)
        {
            enemigoEnRango.RecibirDaño(dañoAlEnemigo);
            print("Jugador golpeó al esqueleto");
        }
    }
}
