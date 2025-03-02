 using Cysharp.Threading.Tasks;

 public class ButtonWinReplay : ButtonBaseReplay
 {
     protected override void DoWhenClicked()
     {
         UIManager.Instance.CloseUI<WinUI>();
         UIManager.Instance.GetUI<WinUI>().IsRegardOn = false;
         base.DoWhenClicked();
     }
 }
