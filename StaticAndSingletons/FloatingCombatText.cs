using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingCombatText : MonoBehaviour
{
    #region Singleton

    private static FloatingCombatText instance = null;
    public static FloatingCombatText Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion Singleton

    [Header("References")]
    [SerializeField] GameObject textPrefab;

    [Space]
    [SerializeField] FloatingTextProperties dealedDamageText;

    [Space]
    [SerializeField] FloatingTextProperties dealedComboDamageText;

    [Space]
    [SerializeField] FloatingTextProperties receivedDamageText;

    [Space]
    [SerializeField] FloatingTextProperties healedDamageText;

    [Space]
    [SerializeField] FloatingTextProperties customText;

    [System.Serializable]
    struct FloatingTextProperties
    {
        public Color Color;
        public float Lifetime;
        public float FloatSpeed;
        public float Size;
    }

    public void SpawnDealedDamageText(int value, Vector3 position)
    {
        StartCoroutine(FloatingText(dealedDamageText, value.ToString(), position));
    }

    public void SpawnCustomText(string text, Vector3 position)
    {
        StartCoroutine(FloatingText(customText, text, position));
    }

    public void SpawnComboDamageText(int value, Vector3 position)
    {
        StartCoroutine(FloatingText(dealedComboDamageText, value.ToString(), position));
    }

    public void SpawnReceivedDamageText(int value, Vector3 position)
    {
        string text = "-" + value.ToString();
        StartCoroutine(FloatingText(receivedDamageText, text, position));
    }

    public void SpawnHealedDamageText(int value, Vector3 position)
    {
        string text = "+" + value.ToString();
        StartCoroutine(FloatingText(healedDamageText, text, position));
    }

    IEnumerator FloatingText(FloatingTextProperties textProperties, string text, Vector3 position)
    {
        GameObject floatingTextObject = ObjectPool.Instance.GetPooledObject(Tags.FLOATING_TEXT);
        floatingTextObject.transform.position = position;

        TextMeshPro floatingText = floatingTextObject.GetComponent<TextMeshPro>();

        floatingText.color = textProperties.Color;
        floatingText.text = text;
        floatingText.fontSize = textProperties.Size;

        floatingTextObject.SetActive(true);

        float timer = textProperties.Lifetime;
        while (timer > 0f)
        {
            floatingTextObject.transform.position = floatingTextObject.transform.position + new Vector3(0f, textProperties.FloatSpeed * Time.fixedDeltaTime, 0f);
            yield return new WaitForFixedUpdate();
            timer -= Time.fixedDeltaTime;
        }
        floatingTextObject.SetActive(false);
    }
}