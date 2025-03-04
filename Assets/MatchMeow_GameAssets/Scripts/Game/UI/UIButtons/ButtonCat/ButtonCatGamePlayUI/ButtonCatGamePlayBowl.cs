using Cysharp.Threading.Tasks;
using MatchMeow_GameAssets.Scripts.Game.UI.UICanvas.CatUI;
using UnityEngine;

public class ButtonCatGamePlayBowl : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        // LevelManager.Instance.ReloadLevel();
        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<MultiFunctionBowlUI>();

    }
}

