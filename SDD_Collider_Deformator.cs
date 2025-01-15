using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
public class SDD_Collider_VertexLister : MonoBehaviour
{
    public List<Vector2> verticesList = new List<Vector2>(); // Lista para mostrar los vértices actuales (solo lectura)
    public List<Vector2> objetivesvectors = new List<Vector2>(); // Lista editable desde el inspector
    private PolygonCollider2D polygonCollider;

    private List<Vector2> previousObjetivesVectors = new List<Vector2>(); // Copia para detectar cambios

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Inicializar listas con los valores actuales del PolygonCollider2D
        ListVerticesFromCollider();
        objetivesvectors = new List<Vector2>(verticesList);
        previousObjetivesVectors = new List<Vector2>(objetivesvectors);
    }

    void Update()
    {
        // Verificar si la lista objetivesvectors ha cambiado
        if (!AreListsEqual(objetivesvectors, previousObjetivesVectors))
        {
            verticesList = new List<Vector2>(objetivesvectors);
            polygonCollider.SetPath(0, verticesList.ToArray());
            previousObjetivesVectors = new List<Vector2>(objetivesvectors);

            Debug.Log("Se han actualizado los vértices del PolygonCollider2D.");
        }
    }

    private void ListVerticesFromCollider()
    {
        verticesList.Clear();
        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            Vector2[] pathVertices = polygonCollider.GetPath(i);
            verticesList.AddRange(pathVertices);
        }
    }

    private bool AreListsEqual(List<Vector2> listA, List<Vector2> listB)
    {
        // Verificar si ambas listas tienen el mismo tamaño
        if (listA.Count != listB.Count)
            return false;

        // Comparar cada elemento
        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i] != listB[i])
                return false;
        }

        return true;
    }
}
