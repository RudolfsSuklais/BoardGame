using UnityEngine;

public class DiceSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] hitSounds;

    [Header("Impact settings")]
    public float minImpactVelocity = 0.8f;
    public float maxVolume = 1f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // cik stiprs bija trieciens
        float impact = collision.relativeVelocity.magnitude;

        if (impact < minImpactVelocity)
            return;

        if (hitSounds.Length == 0)
            return;

        AudioClip clip = hitSounds[Random.Range(0, hitSounds.Length)];

        float volume = Mathf.Clamp01(impact / 5f) * maxVolume;

        audioSource.PlayOneShot(clip, volume);
    }
}
