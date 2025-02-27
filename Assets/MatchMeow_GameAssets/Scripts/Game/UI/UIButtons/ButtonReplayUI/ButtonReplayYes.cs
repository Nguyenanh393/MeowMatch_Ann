using Cysharp.Threading.Tasks;

public class ButtonReplayYes : ButtonBaseReplay
{
    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<ReplayPopUpUI>();
        base.DoWhenClicked();
    }
}

