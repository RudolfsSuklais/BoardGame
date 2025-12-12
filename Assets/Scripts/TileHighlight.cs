using UnityEngine;

public class TileHighlight : MonoBehaviour
{
    Renderer rend;
    Color originalColor;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void Highlight(Color color)
    {
        rend.material.color = color;
    }

    public void ResetColor()
    {
        rend.material.color = originalColor;
    }
}
