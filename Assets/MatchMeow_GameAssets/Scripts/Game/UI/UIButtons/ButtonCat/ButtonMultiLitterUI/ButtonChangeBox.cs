public class ButtonChangeBox : ButtonBaseItemtype
{
    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<MultiFuctionLitterUI>();
        base.DoWhenClicked();
    }
}
