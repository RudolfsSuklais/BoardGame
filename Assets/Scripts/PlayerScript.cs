using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;


public class PlayerScript : MonoBehaviour
{

    public GameObject[] playerPrefabs;
    int characterIndex;
    public GameObject spawnPoint;
    int[] otherPlayers;
    int index;
    private const string textFileName = "PlayerNames";


void Start()
{
    GameManager gm = FindFirstObjectByType<GameManager>();

    characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

    GameObject mainCharacter = Instantiate(
        playerPrefabs[characterIndex],
        spawnPoint.transform.position,
        Quaternion.identity
    );

    mainCharacter.GetComponent<NameScript>()
        .SetName(PlayerPrefs.GetString("PlayerName", "John Doe"));

     CameraController cam = FindFirstObjectByType<CameraController>();
if (cam != null)
{
    // kamera piezoomojas pie tava spēlētāja uzreiz spēles sākumā
  cam.FocusPlayer(mainCharacter.transform);

}
   

    gm.RegisterPlayer(mainCharacter);
    Debug.Log("REGISTERED MAIN: " + mainCharacter.name);

    otherPlayers = new int[PlayerPrefs.GetInt("PlayerCount")];
    string[] nameArray = ReadLinesFromFile(textFileName);

   Vector3 basePos = spawnPoint.transform.position;
float spacing = 0.5f; // <-- maini šo, ja gribi vēl tālāk

for (int i = 0; i < otherPlayers.Length - 1; i++)
{
    index = Random.Range(0, playerPrefabs.Length);

    Vector3 spawnPos = basePos + new Vector3(
        (i + 1) * spacing,
        0,
        0
    );

    GameObject otherPlayer = Instantiate(
        playerPrefabs[index],
        spawnPos,
        Quaternion.identity
    );

    string randomName = nameArray[Random.Range(0, nameArray.Length)];
    otherPlayer.GetComponent<NameScript>().SetName(randomName);

    gm.RegisterPlayer(otherPlayer);
    Debug.Log("REGISTERED NPC: " + otherPlayer.name);
}

}


  
    string[] ReadLinesFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if(textAsset != null)
        {
            return textAsset.text.Split(new[] {'\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            Debug.LogWarning("File not found: " + fileName);
            return new string[0];
        }
    }
}


