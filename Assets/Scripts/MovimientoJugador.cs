using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public CharacterController controller;
    public float velocidad = 6f;
    public float gravedad = -9.81f;

    Vector3 velocidadCaida;

    // Update is called once per frame
    void Update()
    {
        // 1. Leer las teclas (W, A, S, D)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 2. Calcular la dirección hacia donde queremos ir
        Vector3 movimiento = transform.right * x + transform.forward * z;

        // 3. Mover al personaje
        controller.Move(movimiento * velocidad * Time.deltaTime);

        // 4. Aplicar gravedad (para que no flote)
        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);
    }
}
