 using Cysharp.Threading.Tasks;

 public class ButtonBaseHome : ButtonBase
 {
     protected override async UniTask OnClickUniTask()
     {
         await base.OnClickUniTask();

         DoWhenClicked();

     }

     protected virtual void DoWhenClicked()
     {
         UIManager.Instance.CloseAll();
         UIManager.Instance.GamePlayObject.SetActive(false);
         UIManager.Instance.OpenUI<MainMenuUI>();
         UIManager.Instance.GetUI<GamePlayUI>().CountDownText.ResetCountdown();
     }
 }
