using Cysharp.Threading.Tasks;

public class ButtonReplayExit : ButtonBaseExit
{
    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<ReplayPopUpUI>();
        base.DoWhenClicked();
    }
}
