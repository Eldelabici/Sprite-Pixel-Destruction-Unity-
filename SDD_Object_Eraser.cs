using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDD_Object_Eraser : MonoBehaviour
{
    private List<GameObject> objetive_Objects = new List<GameObject>();
    private List<Texture2D> objetive_Textures = new List<Texture2D>();
    private List<SpriteRenderer> objetive_SpriteRenderers = new List<SpriteRenderer>();

    private Vector2 localErasePosition;

    private bool margin_corrector;

    // Márgenes de coordenadas para los redondeos
    private float y_margin, x_margin;

    private void Start()
    {
        y_margin = 0.5f;
        x_margin = 0.5f;

        margin_corrector = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject newObject = collision.gameObject;
        SpriteRenderer newSpriteRenderer = newObject.GetComponent<SpriteRenderer>();

        if (newSpriteRenderer != null)
        {
            Texture2D newTexture = newSpriteRenderer.sprite.texture;

            // Agrega el objeto, sprite y textura a las listas
            objetive_Objects.Add(newObject);
            objetive_SpriteRenderers.Add(newSpriteRenderer);
            objetive_Textures.Add(newTexture);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Position_Based_Erasing(gameObject.transform.position);
    }

    private void Position_Based_Erasing(Vector2 erase_Position)
    {
        for (int i = 0; i < objetive_Objects.Count; i++)
        {
            GameObject obj = objetive_Objects[i];
            SpriteRenderer spriteRenderer = objetive_SpriteRenderers[i];
            Texture2D texture = objetive_Textures[i];

            // Convierte la posición mundial a local
            Vector2 localPosition = obj.transform.InverseTransformPoint(erase_Position);
            Rect spriteRect = spriteRenderer.sprite.rect;
            Vector2 pivotAdjustedPos = localPosition + (Vector2)spriteRenderer.sprite.bounds.extents;

            // Calcula coordenadas en la textura
            localErasePosition.x = (pivotAdjustedPos.x / spriteRenderer.sprite.bounds.size.x) * spriteRect.width;
            localErasePosition.y = (pivotAdjustedPos.y / spriteRenderer.sprite.bounds.size.y) * spriteRect.height;

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
                if (correctedPos.x >= 0 && correctedPos.x < texture.width &&
                    correctedPos.y >= 0 && correctedPos.y < texture.height)
                {
                    texture.SetPixel(correctedPos.x, correctedPos.y, new Color(0, 0, 0, 0));
                }
            }

            texture.Apply();
        }
    }

    private List<Vector2> PixelCorrector_decimalmargin(Vector2 local_position)
    {
        List<Vector2> corrected_positions = new List<Vector2>();

        float decimal_x = local_position.x - Mathf.Floor(local_position.x);
        float decimal_y = local_position.y - Mathf.Floor(local_position.y);

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
