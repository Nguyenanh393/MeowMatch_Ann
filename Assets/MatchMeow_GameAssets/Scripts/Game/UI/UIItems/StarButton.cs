using _Pool.Pool;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StarButton : PoolUnit
{
    [SerializeField] private RectTransform starButtonRect;

    public RectTransform StarButtonRect => starButtonRect;
    public void OnInit()
    {
        TF.localScale = new Vector3(0f, 0f, 0f);
    }
}

