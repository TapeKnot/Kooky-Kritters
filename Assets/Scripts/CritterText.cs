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
        Debug.Log(centipedeNum + ", " + waterBugNum);
        text.text = "Centipedes: " + centipedeNum + "\nWater Bugs: " + waterBugNum;
    }
}
