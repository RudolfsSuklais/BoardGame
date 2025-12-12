using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUI : MonoBehaviour
{
    public TMP_Text text;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip tickSound;
    public AudioClip goSound; // optional

    void Awake()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        gameObject.SetActive(false);
    }

    public IEnumerator PlayCountdown()
    {
        gameObject.SetActive(true);

        for (int i = 3; i >= 1; i--)
        {
            text.text = i.ToString();

            if (tickSound != null)
                audioSource.PlayOneShot(tickSound);

            yield return new WaitForSeconds(1f);
        }

        // Optional "GO!"
        if (goSound != null)
        {
            text.text = "GO!";
            audioSource.PlayOneShot(goSound);
            yield return new WaitForSeconds(0.6f);
        }

        gameObject.SetActive(false);
    }
}
