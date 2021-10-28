using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSlotsDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Sprite attackBuffSprite;
    [SerializeField] Sprite defenseBuffSprite;
    [SerializeField] Sprite regenerationBuffSprite;
    [SerializeField] Image foodBuffIcon;
    [SerializeField] Image drinkBuffIcon;
    [SerializeField] TextMeshProUGUI foodBuffDurationDisplay;
    [SerializeField] TextMeshProUGUI drinkBuffDurationDisplay;

    bool foodBuffActive = false;
    bool drinkBuffActive = false;

    TimeSpan foodBuffDurationTimeSpan;
    float foodBuffDuration;

    TimeSpan drinkBuffDurationTimeSpan;
    float drinkBuffDuration;

    IEnumerator foodDurationDisplayUpdate;
    IEnumerator drinkDurationDisplayUpdate;

    void Start()
    {
        foodDurationDisplayUpdate = UpdateFoodBuffDuration();
        drinkDurationDisplayUpdate = UpdateDrinkBuffDuration();

        foodBuffIcon.canvasRenderer.SetAlpha(0f);
        drinkBuffIcon.canvasRenderer.SetAlpha(0f);

        drinkBuffDurationDisplay.text = ("");
        foodBuffDurationDisplay.text = ("");
    }

    IEnumerator UpdateFoodBuffDuration()
    {
        while (foodBuffDuration > 0f)
        {
            foodBuffDurationDisplay.text = foodBuffDurationTimeSpan.ToString("mm':'ss");
            foodBuffDuration--;
            foodBuffDurationTimeSpan = TimeSpan.FromSeconds(foodBuffDuration);
            yield return new WaitForSeconds(1f);
        }
        foodBuffDurationDisplay.text = ("");
        foodBuffActive = false;
        ChangeBuffIcon(ConsumableType.Food, null);
        yield return 0;
    }

    IEnumerator UpdateDrinkBuffDuration()
    {
        while (drinkBuffDuration > 0f)
        {
            drinkBuffDurationDisplay.text = drinkBuffDurationTimeSpan.ToString("mm':'ss");
            drinkBuffDuration--;
            drinkBuffDurationTimeSpan = TimeSpan.FromSeconds(drinkBuffDuration);
            yield return new WaitForSeconds(1f);
        }
        drinkBuffDurationDisplay.text = ("");
        drinkBuffActive = false;
        ChangeBuffIcon(ConsumableType.Drink, null);
        yield return 0;
    }

    void ChangeBuffIcon(ConsumableType type, ConsumableEffect? effect)
    {
        Image buffIcon;
        if (type == ConsumableType.Food)
        {
            buffIcon = foodBuffIcon;
        }
        else if (type == ConsumableType.Drink)
        {
            buffIcon = drinkBuffIcon;
        }
        else
        {
            return;
        }

        Color tempColor = buffIcon.color;

        switch (effect)//change displayed buff icon, based on buff type
        {
            case ConsumableEffect.Damage:
                buffIcon.sprite = attackBuffSprite;
                buffIcon.canvasRenderer.SetAlpha(1f);
                break;

            case ConsumableEffect.Armor:
                buffIcon.sprite = defenseBuffSprite;
                buffIcon.canvasRenderer.SetAlpha(1f);
                break;

            case ConsumableEffect.Regeneration:
                buffIcon.sprite = regenerationBuffSprite;
                buffIcon.canvasRenderer.SetAlpha(1f);
                break;

            case null:
                buffIcon.sprite = null;
                buffIcon.canvasRenderer.SetAlpha(0f);
                break;
        }
    }

    public void UpdateDisplay(ConsumableType type, ConsumableEffect effect, float duration)
    {
        if (type == ConsumableType.Food)
        {
            Debug.Log("Update food buff");

            if (foodBuffActive)
            {
                StopCoroutine(foodDurationDisplayUpdate);
            }

            foodBuffDurationTimeSpan = TimeSpan.FromSeconds(duration);
            foodBuffDuration = duration;
            foodBuffActive = true;

            foodDurationDisplayUpdate = UpdateFoodBuffDuration();
            StartCoroutine(foodDurationDisplayUpdate);
        }
        else if (type == ConsumableType.Drink)
        {
            Debug.Log("Update drink buff");

            if (drinkBuffActive)
            {
                StopCoroutine(drinkDurationDisplayUpdate);
            }

            drinkBuffDurationTimeSpan = TimeSpan.FromSeconds(duration);
            drinkBuffDuration = duration;
            drinkBuffActive = true;

            drinkDurationDisplayUpdate = UpdateDrinkBuffDuration();
            StartCoroutine(drinkDurationDisplayUpdate);
        }
        else
        {
            Debug.Log("Wrong buff slot");
            return;
        }

        ChangeBuffIcon(type, effect);
    }
}