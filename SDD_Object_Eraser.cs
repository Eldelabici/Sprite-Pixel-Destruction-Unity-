using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SDD_Object_Eraser : MonoBehaviour
{
    public GameObject objetive_Object;
    public Texture2D objetive_Texture;
    public SpriteRenderer objetive_SpriteRenderer;
    public Vector2 objetive_position, destructor_position;
    public Vector2 localErasePosition;

    //Margenes de coordenadas para los redondeos
    public float y_margin, x_margin;

    /*
    *      Asuminedo que tenemos un collider trigger
    */
    private void Start()
    {
        y_margin = 0.5f;
        x_margin = 0.5f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        /*
         * Recordatorio de determinar si se posee una textura legible
         *  o preferiblemente una comprobaci�n de script de los distintos destroyables
         * */
        //destructor_position = gameObject.transform.position;
        //recordatorio de hacer comprobaciones por tag o por script aqui
        objetive_Object = collision.gameObject;

        objetive_SpriteRenderer = objetive_Object.GetComponent<SpriteRenderer>();
        objetive_Texture = objetive_SpriteRenderer.sprite.texture;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //destructor_position = gameObject.transform.position;
        Position_Based_Erasing(gameObject.transform.position);
    }
    private void Position_Based_Erasing(Vector2 erase_Position)
    {
        // Convierte la posici�n mundial a local
        Vector2 localPosition = objetive_Object.transform.InverseTransformPoint(erase_Position);
        Rect spriteRect = objetive_SpriteRenderer.sprite.rect;
        Vector2 pivotAdjustedPos = localPosition + (Vector2)objetive_SpriteRenderer.sprite.bounds.extents;

        // Calcula coordenadas en la textura
        localErasePosition.x = (pivotAdjustedPos.x / objetive_SpriteRenderer.sprite.bounds.size.x) * spriteRect.width;
        localErasePosition.y = (pivotAdjustedPos.y / objetive_SpriteRenderer.sprite.bounds.size.y) * spriteRect.height;

        Vector2Int roundedPosition_local_position = World_to_local_position_Rounding(localErasePosition);

        // Verifica que las coordenadas est�n dentro de los l�mites de la textura
        if (roundedPosition_local_position.x >= 0 && roundedPosition_local_position.x < objetive_Texture.width &&
            roundedPosition_local_position.y >= 0 && roundedPosition_local_position.y < objetive_Texture.height)
        {
            objetive_Texture.SetPixel(roundedPosition_local_position.x, roundedPosition_local_position.y, new Color(0, 0, 0, 0));
            objetive_Texture.Apply();
        }
    }
    /*
     * Funcion para devolver un vector de enteros
     * y poder usarlos en Position_Based_Erasing
     * 
     * En este metodo debemos poder rellenar pixeles vacios
     * para evitar los problemas con la tasa de actualizaci�n 
     * del borrado de pixeles y de poscisiones
     */

    private void PixelCorrector_decimalmargin(Vector2 world_position)
    {
        float decimal_x, decimal_y;

        decimal_x = world_position.x - math.trunc(world_position.x);
        decimal_y = world_position.y - math.trunc(world_position.y);

        int new_corrected_x, new_corrected_y;

        if(x_margin >= decimal_x)
        {
            new_corrected_x = Mathf.RoundToInt(world_position.x);
        }
        if(y_margin >= decimal_y)
        {
            new_corrected_y = Mathf.RoundToInt(world_position.y);
        }
    }
    private Vector2Int World_to_local_position_Rounding(Vector2 world_position)
    {
        // Redondea las posiciones a enteros usando Vector2Int
        return new Vector2Int(Mathf.RoundToInt(world_position.x), Mathf.RoundToInt(world_position.y));
    }
}
