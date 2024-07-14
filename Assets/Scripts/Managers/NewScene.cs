using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    public string sceneName;
    public Transform spawnPoint; // Reference to the spawn point Transform

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollisions"))
        {
            GameManager.Instance.scenePlayerSpawnPosition = spawnPoint.position;
            GameManager.Instance.SaveAllData(isLocal: true);
            // // Send a message to PlayerManager with spawn point information
            // PlayerManager.Instance.SendMessage("SetSpawnPoint", spawnPoint.position);

            // Load the new scene
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}