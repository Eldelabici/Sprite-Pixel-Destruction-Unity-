using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Static_Destroyable : MonoBehaviour
{
    /*
     * Sugerencia futura:
     *  Desarrollar en la ventana del GUI principal (SDD_WManager) una forma de copiar en una lista
     *  permanentemente actualizada la lista de todos los vectores2D que se usarán en X escena para tenerlos
     *  precargados. (Sugiero usar IDs para los vectores o guardarlos en objetos con su respectiva referencia aparte).
     *  Esto se hará puramente en el GUI.
     */

    public SpriteRenderer local_Sprite;
    public Texture2D local_Texture;

    private void Start()
    {
        // Obtener el SpriteRenderer y validar si tiene un sprite asignado
        local_Sprite = gameObject.GetComponent<SpriteRenderer>();

        if (local_Sprite.sprite == null)
        {
            Debug.LogError("No hay sprite asignado al objeto.");
            return;
        }

        // Crear una copia de la textura
        local_Texture = Instantiate(local_Sprite.sprite.texture);

        // Crear un nuevo sprite basado en la copia de la textura
        Sprite newSprite = Sprite.Create(
            local_Texture,
            local_Sprite.sprite.rect,
            local_Sprite.sprite.pivot / local_Sprite.sprite.rect.size
        );

        // Asignar el nuevo sprite al SpriteRenderer
        local_Sprite.sprite = newSprite;
    }
}

