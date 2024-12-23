using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDD_Copia : MonoBehaviour
{
    public GameObject destructor_GameObject;
    public Collider2D destructor_Collider;

    public GameObject objetive_Object;
    public SpriteRenderer objetive_Sprite;
    public Texture2D objetive_Texture;

    private Vector2 worldposition_of_destructor;

    /*
    * Recodatorio, de interpolar los puntos para rellenar los puntos faltantes, si es posible, tomar otro enfoque menos costoso
    * Hay que uitar los redondeos de pixeles para evitar trazas gruesas
    * Buscar una forma de evitar los espacios entre borrados
    */
    private void Update()
    {
        worldposition_of_destructor = destructor_GameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objetive_Object == null)
        {
            objetive_Object = collision.gameObject;
            objetive_Sprite = objetive_Object.GetComponent<SpriteRenderer>();

            if (objetive_Sprite != null)
            {
                objetive_Texture = Instantiate(objetive_Sprite.sprite.texture);
                objetive_Texture.Apply();

                objetive_Sprite.sprite = Sprite.Create(
                    objetive_Texture,
                    objetive_Sprite.sprite.rect,
                    objetive_Sprite.sprite.pivot / objetive_Sprite.sprite.rect.size
                );
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (objetive_Texture != null && collision.gameObject == objetive_Object)
        {
            Position_Based_Erasing(worldposition_of_destructor);
        }
    }

    private void Position_Based_Erasing(Vector2 erase_Position)
    {
        // Convierte la posición mundial a local
        Vector2 localPosition = objetive_Object.transform.InverseTransformPoint(erase_Position);
        Rect spriteRect = objetive_Sprite.sprite.rect;
        Vector2 pivotAdjustedPos = localPosition + (Vector2)objetive_Sprite.sprite.bounds.extents;

        float textureX = (pivotAdjustedPos.x / objetive_Sprite.sprite.bounds.size.x) * spriteRect.width;
        float textureY = (pivotAdjustedPos.y / objetive_Sprite.sprite.bounds.size.y) * spriteRect.height;

        // Aplica el método que incluye píxeles adyacentes
        MinimumMarginErase(textureX, textureY);
    }

    private void MinimumMarginErase(float originalX, float originalY)
    {
        // Calcular coordenadas redondeadas enteras
        int baseX = Mathf.FloorToInt(originalX);
        int baseY = Mathf.FloorToInt(originalY);

        // Borrar el píxel base y píxeles adyacentes (margen mínimo)
        for (int x = baseX; x <= baseX + 1; x++)
        {
            for (int y = baseY; y <= baseY + 1; y++)
            {
                if (x >= 0 && x < objetive_Texture.width && y >= 0 && y < objetive_Texture.height)
                {
                    objetive_Texture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
        }

        // Aplicar cambios a la textura
        objetive_Texture.Apply();
    }
}
