using UnityEngine;

public class Credits : MonoBehaviour
{
    // Quit runs when player presses quit button on credits scene
    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();//Close game if in application
    }
}
