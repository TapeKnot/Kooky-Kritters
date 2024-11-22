using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    void Awake()
    {
        SceneManager.activeSceneChanged += UpdateText;
    }

    private void UpdateText(Scene current, Scene next)
    {
        text.text = next.name;
    }
}
