using UnityEngine;
using UnityEngine.Tilemaps;

public class GM : MonoBehaviour
{
    public static GM instance;
    public bool hasKey = false;

    public Tilemap[] hiddenLayers; // Referencia a los Tilemaps ocultos
    public GameObject felicitacionesUI; // Referencia a la UI de felicitaciones

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Asegurarse de que los Tilemaps empiecen ocultos
        SetTilemapsVisibility(false);

        // Asegurarse de que la UI esté apagada al inicio
        if (felicitacionesUI != null)
        {
            felicitacionesUI.SetActive(false);
        }
    }

    public void CollectKey()
    {
        hasKey = true;
        SetTilemapsVisibility(true); // Muestra los Tilemaps cuando se obtiene la llave
    }

    void SetTilemapsVisibility(bool isVisible)
    {
        foreach (Tilemap tilemap in hiddenLayers)
        {
            tilemap.gameObject.SetActive(isVisible);
        }
    }
}