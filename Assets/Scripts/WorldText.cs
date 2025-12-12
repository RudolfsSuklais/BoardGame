using UnityEngine;
using TMPro;

public class WorldText : MonoBehaviour
{
    public TMP_Text text;

    void Awake()
    {
        if (text == null)
            text = GetComponentInChildren<TMP_Text>();

        Destroy(gameObject, 2.5f); // auto pazūd
    }

    public void SetText(string value)
    {
        text.text = value;
    }
}
