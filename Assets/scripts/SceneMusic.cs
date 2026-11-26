using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public enum MusicType
    {
        Menu,
        Gameplay
    }

    [SerializeField] private MusicType musicType;

    private void Start()
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogError(
                "SceneMusic: Place a MusicManager in this scene."
            );
            return;
        }

        if (musicType == MusicType.Menu)
            MusicManager.Instance.PlayMenuMusic();
        else
            MusicManager.Instance.PlayGameMusic();
    }
}
