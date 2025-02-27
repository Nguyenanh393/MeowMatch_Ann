using Cysharp.Threading.Tasks;

public class ButtonPauseExit : ButtonBaseExit
{
    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<PausePopUpUI>();
        base.DoWhenClicked();
    }
}
