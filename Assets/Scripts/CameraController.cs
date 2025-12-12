using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float followSpeed = 5f;
    public float rotateSpeed = 5f;

    [Header("FOV values")]
    public float diceFov = 45f;      // skatoties uz kauliņu
    public float playerFov = 60f;    // normālais player skats
    public float introFov = 35f;     // ļoti tuvs intro skats pie playeriem

    [Header("Offsets")]
    public Vector3 diceOffset = new Vector3(0, 6, -4);
    public Vector3 playerOffset = new Vector3(0, 8, -8);
    public Vector3 introOffset = new Vector3(0, 2f, -2f); // tuvs skats pie spēlētāja

    private Vector3 defaultPlayerOffset;

    private Transform target;
    private Camera cam;

    private enum CamMode { None, DiceFocus, DiceFollow, Player }
    private CamMode mode = CamMode.None;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        // saglabājam sākotnējo player offset
        defaultPlayerOffset = playerOffset;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // izvēlamies offset atkarībā no mode
        Vector3 currentOffset;

        if (mode == CamMode.Player)
        {
            currentOffset = playerOffset;    // var būt default vai intro, atkarībā no metodes
        }
        else
        {
            currentOffset = diceOffset;
        }

        Vector3 desiredPos = target.position + currentOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            Time.deltaTime * followSpeed
        );

        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRot,
            Time.deltaTime * rotateSpeed
        );
    }

    // ==========================
    // CAMERA MODES
    // ==========================

    public void FocusDice(Transform dice)
    {
        mode = CamMode.DiceFocus;
        target = dice;

        StopAllCoroutines();
        StartCoroutine(Zoom(cam.fieldOfView, diceFov, 0.4f));
    }

    public void FollowDiceFall(Transform dice)
    {
        mode = CamMode.DiceFollow;
        target = dice;

        StopAllCoroutines();
        StartCoroutine(Zoom(cam.fieldOfView, diceFov, 0.4f));
    }

    public void FocusPlayer(Transform player)
    {
        mode = CamMode.Player;
        target = player;

        // atjaunojam normālo player offset
        playerOffset = defaultPlayerOffset;

        StopAllCoroutines();
        StartCoroutine(Zoom(cam.fieldOfView, playerFov, 0.4f));
    }

    // intro close-up pie playera
    public void FocusPlayerCloseUp(Transform player)
    {
        mode = CamMode.Player;
        target = player;

        // uz laiku izmantojam intro offset
        playerOffset = introOffset;

        StopAllCoroutines();
        StartCoroutine(Zoom(cam.fieldOfView, introFov, 0.4f));
    }

    // ==========================
    // ZOOM SYSTEM
    // ==========================

    private IEnumerator Zoom(float from, float to, float time)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            cam.fieldOfView = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }

    // OPTIONAL: tev vēl stāv Shake, ja gribi paturēt
    public void Shake(float duration = 0.2f, float magnitude = 0.15f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float time = 0f;

        while (time < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void SetTopDownView()
{
    target = null;
    StopAllCoroutines();

    transform.position = new Vector3(0, 20f, 0); // pielāgo
    transform.rotation = Quaternion.Euler(90f, 0, 0);

    cam.fieldOfView = 45f;
}

}
