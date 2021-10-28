using UnityEngine;

[CreateAssetMenu(fileName = "item_coin_name", menuName = "Inventory System/Items/Coin")]
public class CoinObject : DefaultItem
{
    [SerializeField] int amount;

    public override void SetType()
    {
        type = ItemType.GoldCoin;
    }

    public int Amount { get => amount; }
}