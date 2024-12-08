using UnityEngine;

public class Player : MonoBehaviour {
    public static Player instance;
    public string playerName = "Player Name";
    public PlayerStats playerStats;

    private void Awake() {
        if (instance == null) instance = this;
    }
}