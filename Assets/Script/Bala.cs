using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private float radioExplosion;
    private int danioExplosion;

    public void ConfigurarExplosion(float radio, int danio)
    {
        radioExplosion = radio;
        danioExplosion = danio;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Explotar al colisionar
        Explotar();
        Destroy(gameObject);
    }

    private void Explotar()
    {
        // Afectar a todos los objetos dentro del radio de explosión
        Collider2D[] objetosAfectados = Physics2D.OverlapCircleAll(transform.position, radioExplosion);
        foreach (Collider2D colisionador in objetosAfectados)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                // Aplicar daño al enemigo
                colisionador.GetComponent<Enemigo>().RecibirDanio(danioExplosion);
            }
        }
    }

    private void OnBecameInvisible()
    {
        // Destruir la bala cuando ya no es visible por ninguna cámara
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de explosión en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}