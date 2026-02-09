using System.Collections.Generic;
using System.Linq;
using Korven.GamePlay.Controllers;
using Korven.GamePlay.Units.BowlingPin;
using Korven.GamePlay.Units.FirePoint;
using Korven.GamePlay.Units.PointScore;
using UnityEngine;


public class GameController : MonoBehaviour
{
    [SerializeField]
    private FirePoint firePoint;
    [SerializeField]
    private PointScore pointScore;
    [SerializeField]
    private SceneController sceneController;

    private List<BowlingPin> _activePins;

    private void Start()
    {
        _activePins = FindObjectsOfType<BowlingPin>().ToList();
    }

    private void Update()
    {
        for (int i = _activePins.Count - 1; i >= 0; i--)
        {
            if (_activePins[i].GetIsFallen())
            {
                _activePins.RemoveAt(i);
            }
        }

        if (_activePins.Count == 0)
        {
            HandleWin();
            sceneController.OpenGameScene();
        }
    }

    private void HandleWin()
    {
        int shots = firePoint.GetLaunchCount();
        if (shots == 1)
        {
            Debug.Log("Strike!");
            pointScore.SetScoreText(10);
        }
        else if (shots == 2)
        {
            Debug.Log("Spare!");
            pointScore.SetScoreText(10);
        }
    }
}