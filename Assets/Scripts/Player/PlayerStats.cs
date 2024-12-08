using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    // Player level sys
    private int _level;
    private int _exp;
    private int _statPoint;

    private Dictionary<CStat, int> _stats;

    private void Awake() {
        // Initialize stat values of player
        _stats = new Dictionary<CStat, int> {
            { CStat.Health, 0 },
            { CStat.Strength, 0 },
            { CStat.Agility, 0 },
            { CStat.Intelligence, 0 }
        };
    }

    // Minus CStat points
    public void MinusStat(int points, CStat cStat) {
        if (_stats[cStat] == 0) {
            Debug.LogError("Point of this stat is zero");
        }
        else {
            _stats[cStat] -= points;
            _statPoint += points;
        }

        PlayerTestUI.instance.RefreshUI();
    }
    
    // Add CStats points
    public void AddStat(int points, CStat cStat) {
        if (_statPoint < points) {
            Debug.LogError("Not enough points to add stat to player");
        }
        else {
            _stats[cStat] += points;
            _statPoint -= points;
        }

        PlayerTestUI.instance.RefreshUI();
    }

    // Add Exp
    public void AddExp(int exp) {
        _exp += exp;
        var previousLevel = _level;
        var newLevel = LevelCalculator.instance.ExpToLevel(_exp);

        if (newLevel != previousLevel) {
            _level = newLevel;
            _statPoint += (newLevel - previousLevel) * 3;
        }

        Debug.Log($"Lvl.{_level} : Added {exp} exp");
        PlayerTestUI.instance.RefreshUI();
    }

    // Reset Level to Zero
    public void ResetLevel() {
        _level = 0;
        _exp = 0;
        _statPoint = 0;
        for (CStat i = 0; i < (CStat)_stats.Count; i++) _stats[i] = 0;
        Debug.Log("Reset level");
        PlayerTestUI.instance.RefreshUI();
    }

    public int GetLevel() {
        return _level;
    }

    public int GetExp() {
        return _exp;
    }

    public int GetStatPoint() {
        return _statPoint;
    }

    public int GetCStat(CStat cStat) {
        return _stats[cStat];
    }
}