using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [SerializeField]
    protected Sprite[] sprites;

    public void PlaceSprite(int which, Image target)
    {
        target.sprite = sprites[which];
    }
    public void PlaceSprite(int which, SpriteRenderer target)
    {
        target.sprite = sprites[which];
    }

    public Sprite Get(int index)
    {
        return sprites[index];
    }

    public int GetCount()
    {
        return sprites.Length;
    }


}
