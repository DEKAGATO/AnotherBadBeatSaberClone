﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    private const float MAX_HEALTH = 100f;

    private float health = MAX_HEALTH;
    private TextMeshPro _hitPointsText;

    public void takeDamage(float damagePoints)
    {
        if (_hitPointsText == null) _hitPointsText = GameObject.Find("HitPointsText").GetComponent<TextMeshPro>();
        health -= damagePoints;
        ScoreTracker.Instance.resetComboCounter();
        _hitPointsText.SetText(Mathf.CeilToInt(health).ToString() + " HP");

        if (health <= 0f)
        {
            _gameOver();
        }
    }

    void Update()
    {
        health = Mathf.Max(health + 0.1f, MAX_HEALTH);
    }

    private void _gameOver()
    {
        SceneManager.LoadScene("ScoreMenu");
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
}