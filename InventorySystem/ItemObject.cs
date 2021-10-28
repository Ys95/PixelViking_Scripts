using UnityEngine;

public enum ItemType
{
    Consumable,
    Key,
    GoldCoin,
    Spell,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    [SerializeField] protected string itemId = null;
    [SerializeField] protected string displayName;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected ItemType type;
    [SerializeField] [TextArea(5, 10)] protected string autoDescription;
    [SerializeField] [TextArea(15, 20)] protected string description;

    #region Getters

    public string ItemId { get => itemId; }
    public string DisplayName { get => displayName; }
    public Sprite Icon { get => icon; }
    public ItemType Type { get => type; }
    public string AutoDescription { get => autoDescription; }
    public string Description { get => description; }

    #endregion Getters

    public abstract void SetProperties();

    public abstract void SetType();

    [ContextMenu("Assign ID")]
    public void AssignId()
    {
        string id = IdManager.AssignId(itemId);
        if (string.IsNullOrEmpty(id)) return;
        itemId = id;
    }

    public string GenerateDisplayedDescription()
    {
        if (string.IsNullOrWhiteSpace(autoDescription)) return Description;
        if (string.IsNullOrWhiteSpace(Description)) return autoDescription;

        return (Description + "\n" + "\n" + autoDescription);
    }

    public abstract void CreateAutoDescription();

    public void OnValidate()
    {
        SetProperties();
        SetType();
        CreateAutoDescription();
    }
}