using System;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(CharacterValues))]
public class HealthUIUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text HeartText;
    private CharacterValues PlayerValues;
    private void OnPotentialHealthChange(uint health)
    {
        HeartText.text = health.ToString();
    }
    private void Awake()
    {
        PlayerValues = GetComponent<CharacterValues>();
        PlayerValues.SubscribeToOnPotentialHealthChange(OnPotentialHealthChange);
    }
    private void Start()
    {
        if (HeartText == null) throw new Exception($"{nameof(HeartText)} was null in {nameof(HealthUIUpdater)}, please assing it.");
    }
}