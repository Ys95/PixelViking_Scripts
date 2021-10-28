using UnityEngine;

[CreateAssetMenu(fileName = "item_default_itemname", menuName = "Inventory System/Items/Default")]
public class DefaultItem : ItemObject
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
        type = ItemType.Default;
    }
}