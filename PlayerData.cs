using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int armor;
    public int wave;
    public int maxWave;
    public int highscore;
    public int score;

    public PlayerData(Player player)
    {
        health = player.health;
        armor = player.armor;
        wave = player.wave;
        maxWave = player.maxWave;
        highscore = player.highscore;
        score = player.score;
    }
}
