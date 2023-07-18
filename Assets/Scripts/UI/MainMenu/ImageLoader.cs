using UnityEngine;
using UnityEngine.UI;
using System;

public class ImageLoader : MonoBehaviour
{
    public Image imageComponent;

    public void LoadImageFromBase64(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageBytes);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        imageComponent.sprite = sprite;
    }
}
