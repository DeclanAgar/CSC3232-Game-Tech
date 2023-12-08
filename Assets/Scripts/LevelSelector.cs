using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private Door door;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Return)) {
            LoadLevel();
        }
    }
    
    // Loop through all doors and load respective scene based on door player is overlapping with
    private void LoadLevel()
    {

        for(int i = 0; i < gameObject.transform.childCount; i++) //Iterate through all doors
        {
            GameObject door = gameObject.transform.GetChild(i).gameObject;
            if (door.GetComponentInChildren<Door>().GetPlayerOverlapping() == true) //If player is overlapping over current door
            {
                Data.ResetValues(); // Set player values to default
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + int.Parse(door.name.Substring(door.name.Length - 1))); //Load scene relative to door used
                return; //Avoid unneeded repetitions of loop

            }
        }
    }
}
