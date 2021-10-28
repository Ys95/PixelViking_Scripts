using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [SerializeField] UIManager uiScript;

    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] GameObject saveText;
    [SerializeField] GameObject activateText;

    [Header("Effects")]
    [SerializeField] ParticleSystem particles;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D light2d;

    bool playerInRange;
    bool wasActivated;

    public bool WasInteractedWith { get => wasActivated; }

    void OnValidate()
    {
        if (uiScript != null) return;

        var uiCanvas = GameObject.FindGameObjectWithTag("UIManager");
        if (uiCanvas == null) return;

        uiScript = uiCanvas.GetComponent<UIManager>();
    }

    void Awake()
    {
        if (uiScript != null) return;

        var uiCanvas = GameObject.FindGameObjectWithTag("UIManager");
        if (uiCanvas == null) return;

        uiScript = uiCanvas.GetComponent<UIManager>();
    }

    public void Interact(GameObject whoInteracts)
    {
        if (whoInteracts.tag != Tags.PLAYER || !playerInRange) return;

        if (wasActivated)
        {
            uiScript.SavePointMenu();
        }
        else
        {
            Activate();
        }
    }

    void Activate()
    {
        wasActivated = true;
        Invoke(nameof(ActivateEffects), 0.2f);
        animator.SetTrigger("activateSavePoint");

        activateText.SetActive(false);
        saveText.SetActive(true);
        Debug.Log("Player in save point");

        SaveSystem.Instance.CreateTempSave();
    }

    public void LoadState(bool activated)
    {
        if (activated)
        {
            particles.Play();
            light2d.gameObject.SetActive(true);
            animator.SetTrigger("activateSavePoint");
            wasActivated = true;
        }
    }

    void ActivateEffects()
    {
        particles.Play();
        SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_SpawnPointActivated, transform);
        light2d.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;

        if (wasActivated)
        {
            saveText.SetActive(true);
            playerInRange = true;
            return;
        }
        else
        {
            activateText.SetActive(true);
            playerInRange = true;
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;

        playerInRange = false;

        saveText.SetActive(false);
        activateText.SetActive(false);
        Debug.Log("Player no longer in save point");
    }
}