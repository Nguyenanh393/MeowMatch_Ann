using Cysharp.Threading.Tasks;

public class ButtonReplayNo : ButtonBaseExit
{
    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<ReplayPopUpUI>();
        base.DoWhenClicked();
    }
}

