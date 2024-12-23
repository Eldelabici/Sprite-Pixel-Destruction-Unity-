using UnityEngine;

public class TestingB : MonoBehaviour
{
    public GameObject prefabToInstantiate; // Prefab que se instanciará

    private Camera mainCamera;

    private void Start()
    {
        // Obtener referencia a la cámara principal
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No se encontró una cámara principal en la escena.");
        }
    }

    private void Update()
    {
        // Detectar clic izquierdo del mouse
        if (Input.GetMouseButtonDown(0) && prefabToInstantiate != null)
        {
            // Obtener la posición del clic en el mundo (Vector2 para 2D)
            Vector2 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Instanciar el prefab en la posición del clic
            Instantiate(prefabToInstantiate, clickPosition, Quaternion.identity);
        }
    }
}
