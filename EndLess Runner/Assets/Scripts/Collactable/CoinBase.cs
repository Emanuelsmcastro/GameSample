using Items;


public class CoinBase : CollactableBase
{
    public int coinAmount = 1;
    public ItemType coinType;
    private SoInt soInt;
    private ItemSetup itemSetup;
    private ItemManager _itemManager;
    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.instance;
        _itemManager = ItemManager.instance;
    }

    public override void Collect()
    {
        base.Collect();
        itemSetup = _itemManager.GetItemSetupByType(itemType);
        soInt = itemSetup.soInt;
        soInt.value += coinAmount;
        itemSetup.currentValue += 1;
        UpdateCoinUi();

    }

    private void UpdateCoinUi()
    {
        _gameManager.UpdateCurrentUICoin();
    }
}
