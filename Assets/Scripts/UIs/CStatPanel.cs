using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CStatPanel : MonoBehaviour {
    
    // CStats Panel Elements
    public CStat cStat;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI pointText;
    public Button minusButton;
    public Button addButton;

    private void Awake() {
        nameText.text = cStat.ToString();
    }
    
    private void Start() {
        // Add Listener for onClick event of each Button
        minusButton.onClick.AddListener(() => { Player.instance.playerStats.MinusStat(1, cStat); });
        addButton.onClick.AddListener(() => { Player.instance.playerStats.AddStat(1, cStat); });
    }
}