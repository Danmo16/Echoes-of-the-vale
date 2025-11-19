using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public CharacterController controller;
    public float velocidad = 6f;
    public float gravedad = -9.81f;
    public float alturaSalto = 1.5f;

    Vector3 velocidadCaida;
    bool estaEnSuelo;

    // Update is called once per frame
    void Update()
    {
        // Verificar si estamos tocando el piso
        estaEnSuelo = controller.isGrounded;

        // Si tocamos el suelo y la velocidad de caída es baja, la reseteamos
        if (estaEnSuelo && velocidadCaida.y < 0)
        {
            velocidadCaida.y = -2f; // Pequeña fuerza para asegurar que toca el suelo
        }

        // Leer las teclas (W, A, S, D)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calcular la dirección hacia donde queremos ir
        Vector3 movimiento = transform.right * x + transform.forward * z;

        // Mover al personaje
        controller.Move(movimiento * velocidad * Time.deltaTime);

        // SALTO: Solo si está en el suelo y presiona Espacio
        if (Input.GetButtonDown("Jump") && estaEnSuelo)
        {
            // Fórmula física para saltar la altura exacta
            velocidadCaida.y = Mathf.Sqrt(alturaSalto * -2f * gravedad);
        }

        // Aplicar gravedad (para que no flote)
        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);
    }
}
