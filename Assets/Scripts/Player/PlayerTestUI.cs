using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTestUI : MonoBehaviour {
    public static PlayerTestUI instance;

    // Player UIs
    [Header("Player")] 
    public TextMeshProUGUI playerName;

    // Level UIs
    [Space] [Header("Level")] 
    public TextMeshProUGUI level;
    public Slider levelSlider;

    // CStats, Stats UIs
    [Space] [Header("Stats")] 
    public TextMeshProUGUI statsPoints;
    public CStatPanel hpPoints;
    public CStatPanel strPoints;
    public CStatPanel spdPoints;
    public CStatPanel itlPoints;

    // Commands UIs
    [Space] [Header("Commands")] 
    public Button resetLevelBtn;
    public Button addExpBtn;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void Start() {
        RefreshUI();
    }

    public void RefreshUI() {
        RefreshPlayerUI();
        RefreshLevelUI();
        RefreshCStatUI();
    }

    private void RefreshPlayerUI() {
        // playerName.text = Player.instance.playerName;
        playerName.text = Reference.name;
    }

    private void RefreshLevelUI() {
        // The Current Lvl of Player
        var lvl = Player.instance.playerStats.GetLevel();
        // Current Exp at this level = Current Exp of Player - Current Player Level to Exp
        var curExp = Player.instance.playerStats.GetExp() - LevelCalculator.instance.LevelToExp(lvl - 1);
        var reqExp = LevelCalculator.instance.LevelToExp(lvl) - LevelCalculator.instance.LevelToExp(lvl - 1);
        if (lvl == 0) {
            lvl = 1;
            curExp = 0;
            reqExp = 400;
        }
        level.text = $"Lvl.{lvl} : {curExp} / {reqExp}";
        levelSlider.value = (float)curExp / reqExp;
    }

    private void RefreshCStatUI() {
        var statsPoint = Player.instance.playerStats.GetStatPoint();
        statsPoints.text = $"STATS\npoints: {statsPoint}";
        hpPoints.pointText.text = Player.instance.playerStats.GetCStat(CStat.Health).ToString();
        strPoints.pointText.text = Player.instance.playerStats.GetCStat(CStat.Strength).ToString();
        spdPoints.pointText.text = Player.instance.playerStats.GetCStat(CStat.Agility).ToString();
        itlPoints.pointText.text = Player.instance.playerStats.GetCStat(CStat.Intelligence).ToString();
    }

    public void GoToMenu() {
        SceneManager.LoadScene("FirebaseLogin");
    }
}