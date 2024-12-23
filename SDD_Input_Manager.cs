using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Este Script maneja las formas de input de posiciones 
 * que se pueden basar en mouse, en touches o en objetos
 * 
 * 03/12/24
 *  Recordatorio de hacer que se tengan las posiciones en worldpositions
 *  pues se debe usar en conjunto directo con Pixel_Eraser
 */
namespace SDD
{
    public class SDD_Input_Manager : MonoBehaviour
    {
        // Recordatorio de usar otro script para manejar otras cámaras
        public Camera reference_Camera;

        public List<string> Objetives_tags = new List<string>();
        [SerializeField]
        public static List<Vector2> points_To_Destroy = new List<Vector2>();

        private void Start()
        {
            reference_Camera = Camera.main;
            Objetives_tags.Add("Everything");
        }

        private void Mouse_Input()
        {

            Vector2 currentMousePosition;
            if (Input.GetMouseButton(0))
            {
                currentMousePosition = reference_Camera.ScreenToWorldPoint(Input.mousePosition);
                points_To_Destroy.Add(currentMousePosition);
            }
            if (Input.GetMouseButton(1))
            {
                currentMousePosition = reference_Camera.ScreenToWorldPoint(Input.mousePosition);
                points_To_Destroy.Add(currentMousePosition);
            }
        }
        //  Para objetos con un script que le amerita poder destruir otros objetos
        public void Object_Input()
        {
            foreach (string tag in Objetives_tags)
            {
                foreach (GameObject gameobject_ObjetiveTag
                    in GameObject.FindGameObjectsWithTag(tag))
                {
                    points_To_Destroy.Add(gameobject_ObjetiveTag.transform.position);
                }

            }
        }
        private void Update()
        {

            Mouse_Input();
            Object_Input();
        }
        private void LateUpdate()
        {

        }

    }
}
