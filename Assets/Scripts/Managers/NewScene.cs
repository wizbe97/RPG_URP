using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    public int sceneBuildIndex;
    public Transform spawnPoint; // Reference to the spawn point Transform

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollisions"))
        {
            // Send a message to PlayerManager with spawn point information
            PlayerManager.Instance.SendMessage("SetSpawnPoint", spawnPoint.position);

            // Load the new scene
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
