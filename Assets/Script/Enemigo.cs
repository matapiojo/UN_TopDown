using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Persecución")]
    public float velocidadMovimiento = 3f;
    public float radioPersecucion = 5f; // Radio para detectar al jugador

    [Header("Disparo")]
    public GameObject balaPrefab;
    public float velocidadBala = 7f;
    public float tiempoEntreDisparos = 1f; // Tiempo entre cada disparo
    public Color colorBalaEnemigo = Color.red; // Color de la bala del enemigo

    [Header("Salud")]
    public int salud = 30; // Salud del enemigo

    private Transform jugador;
    private bool persiguiendoJugador = false;
    private bool descansando = false;
    private float tiempoUltimoDisparo;

    private void Awake()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        tiempoUltimoDisparo = Time.time;
    }

    void Update()
    {
        DetectarJugador();

        if (persiguiendoJugador)
        {
            PerseguirJugador();
        }
        else if (descansando)
        {
            Disparar();
        }
    }

    private void DetectarJugador()
    {
        // Detectar al jugador dentro del radio de persecución
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);
        persiguiendoJugador = distanciaAlJugador <= radioPersecucion;
        descansando = !persiguiendoJugador; // Descansa si no está persiguiendo
    }

    private void PerseguirJugador()
    {
        // Moverse hacia el jugador
        Vector2 direccion = (jugador.position - transform.position).normalized;
        transform.Translate(direccion * velocidadMovimiento * Time.deltaTime);
    }

    private void Disparar()
    {
        // Disparar en intervalos regulares
        if (Time.time - tiempoUltimoDisparo >= tiempoEntreDisparos)
        {
            // Crear la bala
            GameObject bala = Instantiate(balaPrefab, transform.position, Quaternion.identity);
            bala.GetComponent<SpriteRenderer>().color = colorBalaEnemigo; // Cambiar color de la bala

            // Dirección del disparo (hacia el jugador)
            Vector2 direccion = (jugador.position - transform.position).normalized;
            bala.GetComponent<Rigidbody2D>().linearVelocity = direccion * velocidadBala;

            // Actualizar el tiempo del último disparo
            tiempoUltimoDisparo = Time.time;
        }
    }

    public void RecibirDanio(int danio)
    {
        // Reducir la salud del enemigo
        salud -= danio;
        if (salud <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        // Lógica para la muerte del enemigo
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de persecución en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioPersecucion);
    }
}