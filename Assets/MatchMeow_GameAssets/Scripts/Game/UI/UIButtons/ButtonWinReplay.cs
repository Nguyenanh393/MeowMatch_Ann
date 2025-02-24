 using Cysharp.Threading.Tasks;

 public class ButtonWinReplay : ButtonBase
 {
     protected override async UniTask OnClickUniTask()
     {
         await base.OnClickUniTask();

         DoWhenClicked();

     }

     private void DoWhenClicked()
     {
         UIManager.Instance.CloseUI<WinUI>();
         GameManager.Instance.OnReloadGame().Forget();
         // UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(true);
     }
 }
