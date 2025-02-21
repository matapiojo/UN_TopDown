using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 5f;

    [Header("Disparo")]
    public GameObject balaPrefab;
    public float velocidadBala = 10f;
    public Color colorBalaJugador = Color.blue; // Color de la bala del jugador
    public float radioExplosion = 2f; // Radio de la explosi�n de la bala
    public int danioExplosion = 10; // Da�o de la explosi�n
    public float tiempoRecarga = 1f; // Tiempo de recarga despu�s de 10 disparos

    private int balasRestantes = 10; // Cargador de 10 balas
    private bool recargando = false; // Estado de recarga

    void Update()
    {
        Movimiento();
        Disparo();
    }

    private void Movimiento()
    {
        // Movimiento en 8 direcciones
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");

        Vector2 movimiento = new Vector2(movX, movY).normalized;
        transform.Translate(movimiento * velocidadMovimiento * Time.deltaTime);
    }

    private void Disparo()
    {
        if (Input.GetButtonDown("Fire1") && balasRestantes > 0 && !recargando)
        {
            // Disparar una bala
            GameObject bala = Instantiate(balaPrefab, transform.position, Quaternion.identity);
            bala.GetComponent<SpriteRenderer>().color = colorBalaJugador; // Cambiar color de la bala

            // Direcci�n del disparo (hacia el mouse)
            Vector2 direccion = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            bala.GetComponent<Rigidbody2D>().linearVelocity = direccion * velocidadBala;

            // Configurar la explosi�n de la bala
            Bala balaScript = bala.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.ConfigurarExplosion(radioExplosion, danioExplosion);
            }

            // Reducir balas restantes
            balasRestantes--;

            // Iniciar recarga si no quedan balas
            if (balasRestantes == 0)
            {
                StartCoroutine(Recargar());
            }
        }
    }

    private IEnumerator Recargar()
    {
        recargando = true;
        yield return new WaitForSeconds(tiempoRecarga);
        balasRestantes = 10; // Recargar el cargador
        recargando = false;
    }
}
