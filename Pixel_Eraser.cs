using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Pixel Erasing es un namespace que engloba métodos que permiten borrar solo píxeles
 * no puede hacer nada más, las posiciones importadas deben ser en posiciones del mundo
 */
namespace Pixel_Erasing
{
    public class Pixel_Eraser : MonoBehaviour
    /*
     * Este código se basa en la destrucción de píxeles, básicamente es su única función
     */
    {
        public GameObject objetive_GameObject; // GameObject objetivo a ser destruido
        public LayerMask layer_To_Destroy; // La capa objetivo a destruir
        public Vector2 Input_Position; // Posición en el mundo
        public SpriteRenderer objetive_Sprite; // Sprite del GameObject
        public Texture2D objetive_Sprite_Texture; // Textura del sprite

        public int radius;

        void Start()
        {
            layer_To_Destroy = -1; // Temporal, aplica a todas las capas
            radius = 0; // Radio inicializado en 0
            objetive_GameObject = gameObject;

            // Crear una copia editable de la textura del sprite original
            objetive_Sprite_Texture = Instantiate(objetive_Sprite.sprite.texture);
            objetive_Sprite_Texture.Apply();
            objetive_Sprite.sprite = Sprite.Create
                (objetive_Sprite_Texture,
                objetive_Sprite.sprite.rect,
                objetive_Sprite.sprite.pivot / objetive_Sprite.sprite.rect.size);
        }

        private void Position_Based_Erasing(Vector2 erase_Position)
        {
            Vector2 localPosition = 
                objetive_GameObject.transform.InverseTransformPoint(erase_Position);

            Rect spriteRect = 
                objetive_Sprite.sprite.rect;

            Vector2 pivotAdjustedPos = 
                localPosition + (Vector2)objetive_Sprite.sprite.bounds.extents;

            // Calcula coordenadas de la textura
            float textureX = (pivotAdjustedPos.x / objetive_Sprite.sprite.bounds.size.x) * spriteRect.width;
            float textureY = (pivotAdjustedPos.y / objetive_Sprite.sprite.bounds.size.y) * spriteRect.height;

            int local_position_to_erase_x = Mathf.RoundToInt(textureX);
            int local_position_to_erase_y = Mathf.RoundToInt(textureY);

            // Para simple borrado en posiciones
            if (radius == 0)
            {
                //  05/12/24
                /* Comprobacion de las dimensiones de la textura a borrar
                 * Si no se realiza, se borran los bordes mas cercanos a la posición dada
                */
                if (local_position_to_erase_x >= 0 && local_position_to_erase_x < objetive_Sprite_Texture.width &&
                    local_position_to_erase_y >= 0 && local_position_to_erase_y < objetive_Sprite_Texture.height)
                {
                    objetive_Sprite_Texture.SetPixel
                        (local_position_to_erase_x,
                        local_position_to_erase_y,
                        new Color(0, 0, 0, 0));
                    objetive_Sprite_Texture.Apply(); 
                }
            }
        }

        private void Update()
        {
            Input_Position = //Temporal, recordatorio de usar SDD_Input_Manager para las entradas
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (Vector2 worldpoint in SDD.SDD_Input_Manager.points_To_Destroy)
            {
                Position_Based_Erasing(worldpoint);
            }
            SDD.SDD_Input_Manager.points_To_Destroy.Clear();
        }
    }
}
