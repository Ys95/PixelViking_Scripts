using UnityEngine;

[CreateAssetMenu(fileName = "item_key_itemname", menuName = "Inventory System/Items/Key")]
public class KeyItem : ItemObject
{
    public override void CreateAutoDescription()
    {
        //nothing
    }

    public override void SetProperties()
    {
        //nothing
    }

    public override void SetType()
    {
        type = ItemType.Key;
    }
}