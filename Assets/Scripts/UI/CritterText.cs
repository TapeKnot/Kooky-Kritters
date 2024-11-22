using TMPro;
using UnityEngine;

public class CritterText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    void Awake()
    {
        Player.OnCritterPlace += UpdateText;
    }

    private void UpdateText(int centipedeNum, int waterBugNum)
    {
        Debug.Log(waterBugNum + ", " + centipedeNum);
        text.text = "Water Bugs: " + waterBugNum + "\nCentipedes: " + centipedeNum;
    }
}
