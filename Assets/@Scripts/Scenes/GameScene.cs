using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class GameScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        gameObject.AddComponent<CaptureScreenShot>();
#endif
    }

    protected override void Start()
    {
        base.Start();

		Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
		{
			if (count == totalCount)
			{
				OnAssetLoaded();

                Managers.UI.ShowPopupUI<UI_TitlePopup>();
		        Debug.Log("Init");
			}
		});

        Debug.Log("@>> GameScene Init()");
        SceneType = EScene.GameScene;
    }

    private void OnAssetLoaded()
	{
		Managers.Data.Init();
	}

    public override void Clear()
    {
    }
}