using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CleanLitterBoxUI : PopUpUI
{
    [SerializeField] private List<RectTransform> catLitters = new List<RectTransform>();
    [SerializeField] private List<Image> catLitterTargets = new List<Image>();

    private Dictionary<RectTransform, Image> _catLitterPairs = new Dictionary<RectTransform, Image>();
    private Dictionary<RectTransform, bool> _catLitterStates = new Dictionary<RectTransform, bool>();

    private void Awake()
    {
        GetCatLitterPairs();
        GetCatLitterStates();
        OnInit();
    }
    // protected override void OnEnable()
    // {
    //     base.OnEnable();
    // }

    private void OnInit()
    {
        MakeAllCatLittersVisible();
        ChangeAllStatesToFalse();
    }

    private void MakeAllCatLittersVisible()
    {
        for (int i = 0; i < catLitters.Count; i++)
        {
            catLitters[i].gameObject.SetActive(true);
            catLitters[i].localScale = Vector3.one;
            catLitterTargets[i].DOFade(0.5f, 0f);
        }
    }

    private void ChangeAllStatesToFalse()
    {
        for (int i = 0; i < catLitters.Count; i++)
        {
            _catLitterStates[catLitters[i]] = false;
        }
    }

    private void GetCatLitterPairs()
    {
        for (int i = 0; i < catLitters.Count; i++)
        {
            _catLitterPairs.Add(catLitters[i], catLitterTargets[i]);
        }
    }

    private void GetCatLitterStates()
    {
        for (int i = 0; i < catLitters.Count; i++)
        {
            _catLitterStates.Add(catLitters[i], false);
        }
    }

    public void OnCatLitterButtonClick(RectTransform catLitter)
    {
        if (_catLitterStates[catLitter])
        {
            return;
        }

        _catLitterStates[catLitter] = true;

        SoundManager.Instance.PlayScoreSound();
        catLitter.DOScale(0, 0.1f).OnComplete(() =>
        {
            catLitter.gameObject.SetActive(false);
            _catLitterPairs[catLitter].DOFade(1, 0.1f).OnComplete(() =>
            {
                CheckAllCatLittersCleaned();
            });
        });
    }

    private void CheckAllCatLittersCleaned()
    {
        for (int i = 0; i < catLitters.Count; i++)
        {
            if (!_catLitterStates[catLitters[i]])
            {
                return;
            }
        }

        OnAllCatLittersCleaned();
    }

    private void OnAllCatLittersCleaned()
    {
        // Set cooldown for next cleaning
        CooldownManager.Instance.SetLitterBoxCooldown();

        // Show reward UI
        UIManager.Instance.OpenUI<CatRegardUI>();
    }

    private void OnDisable()
    {
        OnInit();
    }
}

