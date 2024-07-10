using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private UpdateAnimationState animationState;
    public void StartRespawnCoroutine(GameObject playerObject, float delay)
    {
        StartCoroutine(RespawnPlayerAfterDelay(playerObject, delay));
    }

    private IEnumerator RespawnPlayerAfterDelay(GameObject playerObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerObject.SetActive(true);
        Player player = playerObject.GetComponent<Player>();
        if (player != null)
        {
            animationState = FindObjectOfType<UpdateAnimationState>();
            player.ResetCharacter();
            animationState.stateLock = false;
        }
    }
}
