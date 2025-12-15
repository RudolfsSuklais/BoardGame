using UnityEngine;

public class DiceRollScript : MonoBehaviour
{
    Rigidbody rBody;
    Vector3 startPosition;

    [SerializeField] private float maxRandForceVal, startRollingForce;
    float forceX, forceY, forceZ;

    public string diceFaceNum;
    public bool isLanded = false;
    public bool firstThrow = false;

    CameraController cam;
    GameManager gm;

    void Awake()
    {
        startPosition = transform.position;
        Initialize();

        cam = FindFirstObjectByType<CameraController>();
        gm  = FindFirstObjectByType<GameManager>();
    }

    private void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.isKinematic = true;

        transform.rotation = Quaternion.Euler(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            Random.Range(0f, 360f)
        );
    }

    private void RollDice()
    {
        rBody.isKinematic = false;

        forceX = Random.Range(0, maxRandForceVal);
        forceY = Random.Range(0, maxRandForceVal);
        forceZ = Random.Range(0, maxRandForceVal);

        rBody.AddForce(Vector3.up * Random.Range(800, startRollingForce));
        rBody.AddTorque(forceX, forceY, forceZ);
    }

    public void ResetDice()
    {
        transform.position = startPosition;
        firstThrow = false;
        isLanded = false;
        Initialize();
    }

  void Update()
{
    if (rBody == null) return;

    // ⛔ BLOĶĒ INTRO UN GAME OVER
    if (gm != null && (gm.introPlaying || gm.gameEnded))
        return;

    if (!firstThrow && !isLanded && cam != null)
    {
        cam.FocusDice(transform);
    }

    if (Input.GetMouseButtonDown(0) && (isLanded || !firstThrow))
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) &&
            hit.collider != null &&
            hit.collider.gameObject == gameObject)
        {
            firstThrow = true;
            cam?.FollowDiceFall(transform);
            RollDice();
        }
    }
}

}
