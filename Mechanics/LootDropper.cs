using System.Collections;
using UnityEngine;

public class LootDropper : MonoBehaviour, IWaitsForSingleton
{
    [SerializeField] Transform lootSpawnPosition;

    [Space]
    [SerializeField] LootTableObject lootTable;

    GameObject[] lootArray;

    void Awake()
    {
        StartCoroutine(WaitTillSingletonReady());
    }

    public IEnumerator WaitTillSingletonReady()
    {
        yield return new WaitUntil(() => Databases.Instance != null);
        yield return new WaitUntil(() => Databases.Instance.IsReady);
        AfterSingletonIsReady();
    }

    public void AfterSingletonIsReady()
    {
        GenerateLoot();
    }

    void GenerateLoot()
    {
        ItemObject[] items;
        if (lootTable == null) return;

        items = lootTable.CreateLoot();

        GameObject lootTemplate = Databases.Instance.PrefabsDB.LootTemplate;

        lootArray = new GameObject[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            lootArray[i] = Instantiate(lootTemplate);
            PickupScript pickupScript = lootArray[i].GetComponent<PickupScript>();

            pickupScript.SetupPickup(items[i], true);
            lootArray[i].SetActive(false);
        }
    }

    public void DropLoot()
    {
        float offsetDirectionX = 1f;
        for (int i = 0; i < lootArray.Length; i++)
        {
            float offset = (i * 0.6f) * offsetDirectionX; // to avoid items stacking on each other
            offsetDirectionX *= -1f;

            Vector2 lootPos = new Vector2(lootSpawnPosition.position.x + offset, lootSpawnPosition.position.y);

            lootArray[i].transform.position = lootPos;
            lootArray[i].transform.parent = null;
            lootArray[i].SetActive(true);
        }
    }
}