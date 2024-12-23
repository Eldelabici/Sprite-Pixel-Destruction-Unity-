using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SDD_Object_Eraser : MonoBehaviour
{
    public GameObject objetive_Object;
    public Texture2D objetive_Texture;
    public SpriteRenderer objetive_SpriteRenderer;
    public Vector2 objetive_position, destructor_position;
    public Vector2 localErasePosition; 

    /*
    *      Asuminedo que tenemos un collider trigger
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {

        /*
         * Recordatorio de determinar si se posee una textura legible
         *  o preferiblemente una comprobación de script de los distintos destroyables
         * */
        destructor_position = gameObject.transform.position;
        //recordatorio de hacer comprobaciones por tag o por script aqui
        objetive_Object = collision.gameObject;
        
        objetive_SpriteRenderer = objetive_Object.GetComponent<SpriteRenderer>();
        objetive_Texture = objetive_SpriteRenderer.sprite.texture;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        destructor_position = gameObject.transform.position;
        if (objetive_Texture != null && collision.gameObject == objetive_Object)
        {
            Position_Based_Erasing(destructor_position);
        }
    }
    private void Position_Based_Erasing(Vector2 erase_Position)
    {
        // Convierte la posición mundial a local
        Vector2 localPosition = objetive_Object.transform.InverseTransformPoint(erase_Position);
        Rect spriteRect = objetive_SpriteRenderer.sprite.rect;
        Vector2 pivotAdjustedPos = localPosition + (Vector2)objetive_SpriteRenderer.sprite.bounds.extents;

        // Calcula coordenadas en la textura
        localErasePosition.x = (pivotAdjustedPos.x / objetive_SpriteRenderer.sprite.bounds.size.x) * spriteRect.width;
        localErasePosition.y = (pivotAdjustedPos.y / objetive_SpriteRenderer.sprite.bounds.size.y) * spriteRect.height;

        int erase_position_x = Mathf.RoundToInt(localErasePosition.x);
        int erase_position_y = Mathf.RoundToInt(localErasePosition.y);

        // Verifica que las coordenadas estén dentro de los límites de la textura
        if (erase_position_x >= 0 && erase_position_x < objetive_Texture.width &&
            erase_position_y >= 0 && erase_position_y < objetive_Texture.height)
        {
            objetive_Texture.SetPixel(erase_position_x, erase_position_y, new Color(0, 0, 0, 0));
            objetive_Texture.Apply();
        }
    }
}
