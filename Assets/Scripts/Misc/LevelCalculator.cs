using System;
using UnityEngine;

public class LevelCalculator : MonoBehaviour {
    public static LevelCalculator instance;
    [SerializeField] private float x;
    [SerializeField] private int y;

    private void Awake() {
        if (instance == null) instance = this;
    }

    public int ExpToLevel(int exp) {
        var level = 0;
        while (LevelToExp(level) <= exp) level++;
        return level;
    }

    public int LevelToExp(int level) {
        var exp = Math.Pow(level / x, y);
        return (int)Math.Ceiling(exp);
    }
}