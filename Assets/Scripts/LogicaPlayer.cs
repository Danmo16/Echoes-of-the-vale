using UnityEngine;

public class LogicaPleyer : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;
    private Animator anim;
    public float x, y; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Leer las teclas (W, A, S, D)
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        // Rotación y movimiento
        transform.Rotate(0, x * velocidadRotacion * Time.deltaTime, 0);
        transform.Translate(0, 0, y * velocidadMovimiento * Time.deltaTime);

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);
    }
}
