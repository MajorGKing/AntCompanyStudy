using System.Collections;
using UnityEngine;
using Spine.Unity;
using System;

public class BaseController : UI_Base
{
    protected SkeletonGraphic _anim = null;


    protected override void Awake()
    {
        base.Awake();

        _anim = GetComponent<SkeletonGraphic>();
    }

    protected virtual void UpdateAnimation() {}

    public virtual void LookLeft(bool flag)
    {
        Vector3 scale = transform.localScale;
		if (flag)
			transform.localScale = new Vector3(Math.Abs(scale.x), scale.y, scale.z);
		else
			transform.localScale = new Vector3(-Math.Abs(scale.x), scale.y, scale.z);
    }

    #region Spine Animation
    public void SetSkeletonAsset(string path)
	{
		if (_init == false)
			return;

		if(string.IsNullOrEmpty(path))
			return;

		_anim.skeletonDataAsset = Managers.Resource.Load<SkeletonDataAsset>(path);
		_anim.Initialize(true);
	}

    public void PlayAnimation(string name, bool loop = true)
	{
		_anim.startingAnimation = name;
		_anim.startingLoop = loop;
	}

    public void ChangeSkin(string name)
    {
        _anim.initialSkinName = name;
		_anim.Initialize(true);
    }

	public void Refresh()
	{
		_anim.Initialize(true);
	}

    public void PlayAnimationOnce(string name)
	{
		StartCoroutine(CoPlayAnimationOnce(name));
	}

	IEnumerator CoPlayAnimationOnce(string name)
	{
		bool defaultLoop = _anim.startingLoop;
		string defaultName = _anim.startingAnimation;

		_anim.startingLoop = false;
		_anim.startingAnimation = name;

		float length = _anim.skeletonDataAsset.GetSkeletonData(true).FindAnimation(name).Duration;
		yield return new WaitForSeconds(length);

		PlayAnimation(defaultName, defaultLoop);
	}

    	public void PlayAnimationOnce(string skin, string name)
	{
		StartCoroutine(CoPlayAnimationOnce(skin, name));
	}

	IEnumerator CoPlayAnimationOnce(string skin, string name)
	{
		bool defaultLoop = _anim.startingLoop;
		string defaultSkin = _anim.initialSkinName;
		string defaultName = _anim.startingAnimation;

		_anim.startingLoop = false;
		_anim.startingAnimation = name;
		ChangeSkin(skin);

		float length = _anim.skeletonDataAsset.GetSkeletonData(true).FindAnimation(name).Duration;
		yield return new WaitForSeconds(length);

		PlayAnimation(defaultName, defaultLoop);
		ChangeSkin(defaultSkin);
	}
    #endregion
}

