using System;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

public class TitleScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        SceneType = Define.EScene.TitleScene;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;  
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
	}

	protected override void Start()
    {
        base.Start();

		
		Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
		{
			if (count == totalCount)
			{
				OnAssetLoaded();

				Managers.Scene.LoadScene(Define.EScene.GameScene);
			}
		});

		//IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
		//IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);
		//Managers.Network.GameServer.Connect(endPoint);
		//CoSendTestPackets();
	}

	private void OnAssetLoaded()
	{
		Managers.Data.Init();
	}

	public override void Clear()
	{

	}

	//IEnumerator CoSendTestPackets()
	//{
	//	while (true)
	//	{
	//		yield return new WaitForSeconds(1);

	//		C_Test pkt = new C_Test();
	//		pkt.Temp = 1;
	//		Managers.Network.Send(pkt);
	//	}
	//}
}
