using System.Collections;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] BoxCollider2D pickupTrigger;
    [SerializeField] CapsuleCollider2D pickupCollider;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Space]
    [SerializeField] LayerMask groudLayers;
    [SerializeField] ItemObject item;
    [SerializeField] bool playSpawnAnimation;
    [SerializeField] bool affectedByPhysics;
    [SerializeField] bool worldSpawnPickup; //world spawned pickup have floating effect and cant be affected by physics;
    [SerializeField] FloatingEffectParameters floatingEffect;

    public ItemObject Item { get => item; set => item = value; }
    public bool PlaySpawnAnimation { get => playSpawnAnimation; set => playSpawnAnimation = value; }

    [System.Serializable]
    struct FloatingEffectParameters
    {
        public float speedMax;
        public float speedMin;
        public float timeBeforeChangingDirection;
    }

    [ContextMenu("Set sprite and name")]
    void SetSpriteAndName()
    {
        if (Item == null || Item.Icon == null || spriteRenderer == null) return;

        spriteRenderer.sprite = item.Icon;
        gameObject.name = item.DisplayName + "_Pickup";
    }

    void OnValidate()
    {
        if (worldSpawnPickup) affectedByPhysics = false;
    }

    void OnEnable()
    {
        SetBehaviour();
    }

    public void SetupPickup(ItemObject itemArg, bool playSpawnAnimationArg)
    {
        item = itemArg;
        if (item == null)
        {
            gameObject.name = "null_Pickup";
        }
        else
        {
            gameObject.name = item.DisplayName + "_Pickup";
            spriteRenderer.sprite = item.Icon;
        }
        playSpawnAnimation = playSpawnAnimationArg;
    }

    public void PlayerThrowedItem(ItemObject item, Vector2 throwForce)
    {
        SetupPickup(item, false);

        affectedByPhysics = true;
        worldSpawnPickup = false;

        SetBehaviour();

        rb.AddForce(throwForce, ForceMode2D.Impulse);
        transform.parent = null;
    }

    void SetBehaviour()
    {
        if (!affectedByPhysics)
        {
            rb.bodyType = RigidbodyType2D.Static;
            playSpawnAnimation = false;

            if (worldSpawnPickup) StartCoroutine(FloatingEffect());
        }

        if (playSpawnAnimation)
        {
            pickupTrigger.enabled = false;
            rb.AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
        }
    }

    IEnumerator FloatingEffect()
    {
        float speed = Random.Range(floatingEffect.speedMax, floatingEffect.speedMin);
        float timer;

        yield return new WaitForSeconds(Random.Range(0f, 1f));

        while (true)
        {
            timer = floatingEffect.timeBeforeChangingDirection;

            while (timer > 0f)
            {
                transform.position = transform.position + new Vector3(0f, speed * Time.fixedDeltaTime, 0f);
                yield return new WaitForFixedUpdate();
                timer -= Time.fixedDeltaTime;
            }
            speed *= -1;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) //Pickup is pickupable after it hits the ground.
    {
        Debug.Log(collision.collider.name);

        gameObject.layer = LayerMask.NameToLayer("Default");
        pickupTrigger.enabled = true;
        playSpawnAnimation = false;

        if (affectedByPhysics)
        {
            affectedByPhysics = false;
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
            rb.bodyType = RigidbodyType2D.Static;
            pickupCollider.enabled = false;
            StartCoroutine(FloatingEffect());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.PLAYER)
        {
            var playerInventory = other.GetComponent<PlayerInventory>();

            playerInventory.PickUpItem(item);
            Destroy(this.gameObject);

            SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_ItemPickedUp, transform);
        }
    }
}