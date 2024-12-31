using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<Vector2> corrected_World_Positions;

    public bool margin_corrector;

    //Margenes de coordenadas para los redondeos
    public float y_margin, x_margin;

    /*
    *      Asuminedo que tenemos un collider trigger
    */
    private void Start()
    {
        y_margin = 0.5f;
        x_margin = 0.5f;

        margin_corrector = false;
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
        Position_Based_Erasing(gameObject.transform.position);
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
        // Corrige márgenes y obtiene posiciones adicionales
        List<Vector2> correctedPositions = new List<Vector2>();
            
        correctedPositions.Add(localErasePosition);

        if (margin_corrector)
        {
            correctedPositions.AddRange(PixelCorrector_decimalmargin(localErasePosition));
                
        }
        foreach (Vector2 pos in correctedPositions)
        {
            Vector2Int correctedPos = World_to_local_position_Rounding(pos);
            if (correctedPos.x >= 0 && correctedPos.x < objetive_Texture.width &&
                correctedPos.y >= 0 && correctedPos.y < objetive_Texture.height)
            {
                objetive_Texture.SetPixel(correctedPos.x, correctedPos.y, new Color(0, 0, 0, 0));
            }
        }
        objetive_Texture.Apply();
        correctedPositions.Clear();
    }

    private List<Vector2> PixelCorrector_decimalmargin(Vector2 local_position)
    {
        List<Vector2> corrected_positions = new List<Vector2>();

        float decimal_x, decimal_y;

        decimal_x = local_position.x - math.trunc(local_position.x);
        decimal_y = local_position.y - math.trunc(local_position.y);

        int new_corrected_x, new_corrected_y;

        if (x_margin > decimal_x)
        {
            new_corrected_x = Mathf.RoundToInt(local_position.x) - 1;
            corrected_positions.Add(new Vector2(new_corrected_x, local_position.y));
        }
        if (y_margin > decimal_y)
        {
            new_corrected_y = Mathf.RoundToInt(local_position.y) - 1;
            corrected_positions.Add(new Vector2(local_position.x, new_corrected_y));
        }

        return corrected_positions;
    }

    private Vector2Int World_to_local_position_Rounding(Vector2 world_position)
    {
        // Redondea las posiciones a enteros usando Vector2Int
        return new Vector2Int(
            Mathf.RoundToInt(world_position.x),
            Mathf.RoundToInt(world_position.y));
    }
}
