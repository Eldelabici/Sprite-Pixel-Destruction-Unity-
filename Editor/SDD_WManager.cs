using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// SDD_Window Manager
/*
 * Ventana que se usara para el manejo de los sprites en general
 * debera permitir un manejo mas comodo de lo que debe de ejecutarse en pantalla
 * 
 */

/*
 * Ideas Futuras
 * 26/10/24
 *  -Que permita crear arquetipos de deformadores con varias propiedades como
 *      >Forma 
 *      >Velocidad de deformacion o destruccion
 *      >Tamaño
 *     Debera poder guardarlos en carpetas y probablemente sea buena idea usar el formato JSON
 *  -Que permita enlistas los Gameobjects que posean las propiedades de deformacion y su script asignado
 *  -Que permita modificaciones futuas sobre el manejo de las deformaciones y destruccion
 *  
 *  -Que tenga una carpeta de sprites que automaticamente se modifiquen con caracteristicas que puede llegar a 
 *  necesitar el posible usuario
 *      >Auto darle el atributo de lectura escritura al sprite
 *      >Auto darle un formato que lo haga verse mejor con una opcion (quitarle el filtro biliniar o triliniar)
 */
public class SDD_WManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Método para abrir la ventana desde el menú de Unity Editor
    [MenuItem("Window/SDD Window Manager")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<SDD_Window>("SDD Window Manager");
        window.minSize = new Vector2(300, 200);
    }
}

public class SDD_Window : EditorWindow
{
    // Método que define el contenido de la ventana del editor
    private void OnGUI()
    {
        GUILayout.Label("SDD Window Manager", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // Aquí se podrán añadir futuros widgets y elementos gráficos
        GUILayout.Label("Ventana en desarrollo para manejo de sprites y deformaciones.");
    }
}
