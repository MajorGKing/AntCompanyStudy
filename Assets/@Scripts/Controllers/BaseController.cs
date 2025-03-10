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
		_anim.skeletonDataAsset = Managers.Resource.Load<SkeletonDataAsset>(path);
		_anim.Initialize(true);
	}

    public void PlayAnimation(string name, bool loop = true)
	{
		_anim.startingAnimation = name;
		_anim.startingLoop = loop;
	}


    #endregion
}

