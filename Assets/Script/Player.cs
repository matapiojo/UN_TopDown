﻿using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI; // Necesario para trabajar con UI

public class Player : MonoBehaviour
{
    private bool tieneLlave = false;
    private float inputH;
    private float inputV;
    private bool moviendose;
    private Vector3 puntoDestino;
    private Vector3 ultimoInput;
    private Vector3 puntoInteraccion;
    private Collider2D colliderDelante;

    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float radioInteraccion;
    [SerializeField] private LayerMask esColisionable;

    [Header("Tilemap Layers")]
    public Tilemap[] defaultLayers;
    public Tilemap[] alternateLayers;

    [Header("UI")]
    public GameObject felicitacionesUI; // Interfaz de felicitaciones
    public GameObject mensajeSinLlaveUI; // Interfaz de mensaje sin llave

    private bool hasSwitchedLayers = false;

    private void Start()
    {
        SetTilemapsVisibility(defaultLayers, true);
        SetTilemapsVisibility(alternateLayers, false);

        // Asegurarse de que las UIs estén ocultas al inicio
        if (felicitacionesUI != null)
        {
            felicitacionesUI.SetActive(false);
        }
        if (mensajeSinLlaveUI != null)
        {
            mensajeSinLlaveUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (inputV == 0)
        {
            inputH = Input.GetAxis("Horizontal");
        }
        if (inputH == 0)
        {
            inputV = Input.GetAxis("Vertical");
        }

        if (!moviendose && (inputH != 0 || inputV != 0))
        {
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();
            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }
        }
    }

    private IEnumerator Mover()
    {
        moviendose = true;

        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        puntoInteraccion = transform.position + ultimoInput;
        moviendose = false;
    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion, esColisionable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }

    private void SetTilemapsVisibility(Tilemap[] tilemaps, bool isVisible)
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.gameObject.SetActive(isVisible);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LayerTrigger"))
        {
            if (!hasSwitchedLayers)
            {
                SetTilemapsVisibility(defaultLayers, false);
                SetTilemapsVisibility(alternateLayers, true);
                hasSwitchedLayers = true;
            }
            else if (tieneLlave)
            {
                SetTilemapsVisibility(defaultLayers, true);
                SetTilemapsVisibility(alternateLayers, false);
                hasSwitchedLayers = false;
            }
        }

        // ⚡ Si el jugador entra en la zona de la casa
        if (other.CompareTag("Casa"))
        {
            if (tieneLlave)
            {
                // Si tiene la llave, mostrar la UI de felicitaciones y cerrar el juego
                if (felicitacionesUI != null)
                {
                    felicitacionesUI.SetActive(true); // Mostrar la UI
                    StartCoroutine(CerrarJuegoDespuesDeSegundos(3)); // Cerrar el juego después de 3 segundos
                }
            }
            else
            {
                // Si no tiene la llave, mostrar el mensaje
                if (mensajeSinLlaveUI != null)
                {
                    mensajeSinLlaveUI.SetActive(true); // Mostrar el mensaje
                    StartCoroutine(OcultarMensajeDespuesDeSegundos(3)); // Ocultar el mensaje después de 3 segundos
                }
            }
        }
    }

    // Corrutina para cerrar el juego después de un tiempo
    private IEnumerator CerrarJuegoDespuesDeSegundos(int segundos)
    {
        yield return new WaitForSeconds(segundos); // Esperar X segundos
        Application.Quit(); // Cerrar el juego
    }

    // Corrutina para ocultar el mensaje después de un tiempo
    private IEnumerator OcultarMensajeDespuesDeSegundos(int segundos)
    {
        yield return new WaitForSeconds(segundos); // Esperar X segundos
        if (mensajeSinLlaveUI != null)
        {
            mensajeSinLlaveUI.SetActive(false); // Ocultar el mensaje
        }
    }

    public void PickUpKey()
    {
        tieneLlave = true;
    }
}