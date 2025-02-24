using Cysharp.Threading.Tasks;

public class ButtonLoseReplay : ButtonBaseReplay
{
    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<LoseUI>();
        base.DoWhenClicked();
    }
}
