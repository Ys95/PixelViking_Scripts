using System.Collections;
using UnityEngine;

public class PlayerCombo : MonoBehaviour
{
    [SerializeField] PlayerScriptReferences player;

    [SerializeField] int value;

    [Space]
    [SerializeField] ParticleSystem particles;
    [SerializeField] Material particlesMat;

    [Space]
    [SerializeField] Material playerMat;
    [SerializeField] WeaponGlowEffect normalWeapon;
    [SerializeField] WeaponGlowEffect comboWeapon;

    int comboMeter;
    bool comboReady;
    bool comboInterrupted;
    IEnumerator fillMeterCoroutine;

    public int ComboMeter { get => comboMeter; }
    public bool IsComboReady { get => comboReady; }

    [System.Serializable]
    struct WeaponGlowEffect
    {
        [SerializeField] Color GlowColor;
        [SerializeField] float GlowIntensinity;

        public Color Effect { get => GlowColor * GlowIntensinity; }
    }

    void OnValidate()
    {
        particlesMat.color = comboWeapon.Effect;
        playerMat.color = normalWeapon.Effect;
    }

    private void Awake()
    {
        fillMeterCoroutine = FillMeter();
        PlayerHealth.PlayerDamaged += InterruptCombo;
    }

    public void StartComboMeter()
    {
        comboInterrupted = false;
        StopCoroutine(fillMeterCoroutine);
        fillMeterCoroutine = FillMeter();
        StartCoroutine(fillMeterCoroutine);
    }

    public void InterruptCombo()
    {
        comboMeter = 0;
        StopCoroutine(fillMeterCoroutine);
        comboInterrupted = true;
    }

    public void UseCombo()
    {
        particles.Play();
        comboReady = false;
        InterruptCombo();
    }

    IEnumerator FillMeter()
    {
        comboMeter = 0;
        while (comboMeter != value)
        {
            comboMeter++;
            yield return new WaitForSeconds(0.1f);
        }
        if (!comboInterrupted) StartCoroutine(ComboReady());
        else
        {
            comboMeter = 0;
        }
    }

    void MaterialComboEffect(bool enable)
    {
        if (enable)
        {
            playerMat.color = comboWeapon.Effect;
            return;
        }
        playerMat.color = normalWeapon.Effect;
    }

    IEnumerator ComboReady()
    {
        comboReady = true;
        MaterialComboEffect(true);
        player.Sounds.PlayerComboReady.Play(transform.position);

        yield return new WaitForSeconds(0.3f);

        MaterialComboEffect(false);
        comboReady = false;
        comboMeter = 0;
    }

    void OnDestroy()
    {
        PlayerHealth.PlayerDamaged -= InterruptCombo;
    }
}