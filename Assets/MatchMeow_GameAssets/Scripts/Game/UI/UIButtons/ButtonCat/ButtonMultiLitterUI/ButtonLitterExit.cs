public class ButtonLitterExit : ButtonBaseExit
{

    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<MultiFuctionLitterUI>();
        base.DoWhenClicked();
    }
}

