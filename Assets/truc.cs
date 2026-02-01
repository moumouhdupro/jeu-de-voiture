using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class truc : MonoBehaviour
{
    public string sceneName = "MainMenu";

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Escape press detected"); // Vérifie que le script fonctionne
            SceneManager.LoadScene(0);
        }
    }
}

