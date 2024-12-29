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

    public List<Vector2Int> corrected_World_Positions;

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
         *  o preferiblemente una comprobación de script de los distintos destroyables
         * */
        //destructor_position = gameObject.transform.position;
        //recordatorio de hacer comprobaciones por tag o por script aqui
        objetive_Object = collision.gameObject;

        objetive_SpriteRenderer = objetive_Object.GetComponent<SpriteRenderer>();
        objetive_Texture = objetive_SpriteRenderer.sprite.texture;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        corrected_World_Positions = PixelCorrector_decimalmargin(gameObject.transform.position);
        //destructor_position = gameObject.transform.position;
        Position_Based_Erasing(gameObject.transform.position);
        foreach(Vector2Int position in corrected_World_Positions)
        {
            Position_Based_Erasing(position);
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

        Vector2Int roundedPosition_local_position = World_to_local_position_Rounding(localErasePosition);

        // Verifica que las coordenadas estén dentro de los límites de la textura
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
     * para evitar los problemas con la tasa de actualización 
     * del borrado de pixeles y de poscisiones
     */

    private List<Vector2Int> PixelCorrector_decimalmargin(Vector2 world_position)
    {
        Vector2Int rounded_world_position = World_to_local_position_Rounding(world_position);
        List<Vector2Int> corrected_positions = new List<Vector2Int>();

        float decimal_x, decimal_y;

        decimal_x = world_position.x - math.trunc(world_position.x);
        decimal_y = world_position.y - math.trunc(world_position.y);

        int new_corrected_x, new_corrected_y;

        if(x_margin >= decimal_x)
        {
            new_corrected_x = Mathf.RoundToInt(world_position.x);

            corrected_positions.Add(new Vector2Int(new_corrected_x, rounded_world_position.y));
        }
        if(y_margin >= decimal_y)
        {
            new_corrected_y = Mathf.RoundToInt(world_position.y);

            corrected_positions.Add(new Vector2Int(rounded_world_position.y,new_corrected_y));
        }

        return corrected_positions;
    }
    private Vector2Int World_to_local_position_Rounding(Vector2 world_position)
    {
        // Redondea las posiciones a enteros usando Vector2Int
        return new Vector2Int(Mathf.RoundToInt(world_position.x), Mathf.RoundToInt(world_position.y));
    }
}
