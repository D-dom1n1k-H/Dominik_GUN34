using TMPro;
using UnityEngine;

namespace Korven.GamePlay.Units.PointScore
{
    public sealed class PointScore : MonoBehaviour
    {
        [SerializeField]
        private GameObject scoreText;
        public int GetScoreText() => int.Parse(scoreText.GetComponent<TextMeshPro>().text);

        public void AddScore(int count) => SetScoreText(GetScoreText() + count);

        public void SetScoreText(int value) => scoreText.GetComponent<TextMeshPro>().text = value.ToString();

        private void ResetScoreText() => scoreText.GetComponent<TextMeshPro>().text = "0";

        private void OnEnable()
        {
            ResetScoreText();
        }
    }
}