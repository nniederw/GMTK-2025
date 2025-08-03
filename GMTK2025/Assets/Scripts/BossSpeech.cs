using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class BossSpeech : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private TMP_Text UIText;
    [SerializeField] private List<string> Speech = new List<string>();
    private int SpeechIndex = -1;
    private void Start()
    {
        if (Canvas == null) { throw new System.Exception($"{nameof(Canvas)} was null on {nameof(BossSpeech)}, please assign."); }
        if (UIText == null) { throw new System.Exception($"{nameof(UIText)} was null on {nameof(BossSpeech)}, please assign."); }
        if (!GameManager.BossHasBeenDefeatedBefore())
        {
            StartSpeech();
        }
    }
    public void StartSpeech()
    {
        Canvas.SetActive(true);
        GameManager.PauseGame();
        SpeechIndex = 0;
        UIText.text = Speech[SpeechIndex];
    }
    private void Update()
    {
        if (SpeechIndex != -1 && Input.GetKeyDown(KeyCode.Space))
        {
            SpeechIndex++;
            if (SpeechIndex >= Speech.Count)
            {
                SpeechIndex = -1;
                Canvas.SetActive(false);
                GameManager.UnpauseGame();
            }
            else
            {
                UIText.text = Speech[SpeechIndex];
            }
        }
    }
}