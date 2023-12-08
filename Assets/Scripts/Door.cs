using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    private Animator animator;

    private bool playerOverlapping;


    #region Getters and Setters

    public bool GetPlayerOverlapping()
    {
        return playerOverlapping;
    }

    public void SetPlayerOverlapping(bool playerOverlapping)
    {
        this.playerOverlapping = playerOverlapping;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Door code to be ran if player is in the level select scene
        if (collision.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().name == "LevelSelect") // If player enters door box collider, run open door animation and set playerOverlapping to true
        {
            animator.SetBool("DoorOpen", true);
            playerOverlapping = true;
            
        }// Door code to be ran if player is in a level
        else if (collision.gameObject.CompareTag("Player") && gameManager.GetHasKey()) // If player enters a door box collider and has a key, run open door animation and set playerOverlapping to true
        {
            animator.SetBool("DoorOpen", true);
            playerOverlapping = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // If player exits door box collider, run close door animation and set playerOverlapping to false
        {
            animator.SetBool("DoorOpen", false);
            playerOverlapping = false;
        }  
    }
}
