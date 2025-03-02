using Cysharp.Threading.Tasks;
using UnityEngine;

public class ButtonEquip : ButtonBase
{
    private ShopItemType _itemType;

    public void SetItemType(ShopItemType type)
    {
        _itemType = type;
    }

    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        PlayerInventoryManager.Instance.EquipItem(_itemType);
        UIManager.Instance.GetUI<ShopUI>().RefreshAllItems();
    }
}
