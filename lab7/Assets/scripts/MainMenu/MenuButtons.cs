using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public void Play(){
        SceneManager.LoadScene("Level1");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting the game");
        #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        #endif
    }
    public GameObject settingsPanel;   // inside the class, next to the methods

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }

    public void Back()
    {
        settingsPanel.SetActive(false);
    }
    
    public AudioSource bgSound;    // inside the class

    public void SetVolume(float value)
    {
        bgSound.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
