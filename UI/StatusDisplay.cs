using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterStats playerStats;

    [Space]
    [SerializeField] Slider hpBar;
    [SerializeField] TextMeshProUGUI hpBarText;

    [Space]
    [SerializeField] TextMeshProUGUI defValueDisplay;
    [SerializeField] TextMeshProUGUI atkValueDisplay;

    void Update()
    {
        hpBar.value = playerStats.Health;
        hpBarText.text = playerStats.Health.ToString();

        atkValueDisplay.text = playerStats.AttackPower.ToString();
        defValueDisplay.text = playerStats.DamageReduction.ToString();
    }
}