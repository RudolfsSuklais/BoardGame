using UnityEngine;
using TMPro;

public class NameScript : MonoBehaviour
{
    [SerializeField] private float heightOffset = 2.2f;

    private TextMeshPro tMP;
    private Transform target;

    public string PlayerName { get; private set; }

    void Awake()
    {
        Transform nameField = transform.Find("NameField");

        if (nameField == null)
        {
            Debug.LogError("NameField not found! Check hierarchy.");
            return;
        }

        tMP = nameField.GetComponent<TextMeshPro>();

        if (tMP == null)
        {
            Debug.LogError("TextMeshPro missing on NameField!");
            return;
        }

        // spēlētājs ir parent
        target = transform.parent;
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + Vector3.up * heightOffset;
    }

    public void SetName(string name)
    {
        PlayerName = name;
        tMP.text = name;

        // nejauša krāsa (ja gribi – vari izņemt)
        tMP.color = new Color32(
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            255
        );
    }

    public string GetPlayerName()
    {
        return PlayerName;
    }
}
