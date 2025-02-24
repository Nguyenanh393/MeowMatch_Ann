 using Cysharp.Threading.Tasks;

 public class ButtonWinReplay : ButtonBaseReplay
 {
 protected override void DoWhenClicked()
 {
     UIManager.Instance.CloseUI<WinUI>();
     base.DoWhenClicked();
 }
 }
