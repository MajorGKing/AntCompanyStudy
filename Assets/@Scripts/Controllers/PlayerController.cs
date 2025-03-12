using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{
    public enum EEmoticonType
    {
        None,
        Exclamation,
        Question
    }

    PlayerData _data;
    GameObject _exclamation;
	GameObject _question;
	PlayerState _playerState;
    EEmoticonType _emoticon = EEmoticonType.None;

    public Define.EJobTitleType JobTitle { get; set; }

    public Define.EAnimState State
	{
		get { return _playerState.state; }
		set 
		{ 
			_playerState.state = value; 
			UpdateAnimation(); 
		}
	}

    public bool GoHomeEvent 
	{ 
		get { return _playerState.goHomeEvent; }
		set 
		{
			_playerState.goHomeEvent = value;
			if (value)
				SetEmoticon(EEmoticonType.Exclamation); 
			else
				SetEmoticon(EEmoticonType.None);
		}
	}

    public bool DialogueEvent 
	{ 
		get { return _playerState.dialogueEvent; }
		set 
		{
			_playerState.dialogueEvent = value;
			if (value)
				SetEmoticon(EEmoticonType.Question);
			else
				SetEmoticon(EEmoticonType.None);
		}
	}

    protected override void Awake()
    {
        base.Awake();

        _playerState = Managers.Game.GetPlayerState(JobTitle);

        gameObject.BindEvent(OnClickPlayer);

        Managers.Data.Players.TryGetValue((int)JobTitle, out _data);
        _anim.skeletonDataAsset = Managers.Resource.Load<SkeletonDataAsset>(_data.spine);
        State = Define.EAnimState.Working;

        _exclamation = Utils.FindChild(gameObject, "Exclamation");
		_question = Utils.FindChild(gameObject, "Question");
		SetEmoticon(EEmoticonType.None);
    }

    public void SetInfo(Define.EJobTitleType jobTitle)
	{
		JobTitle = jobTitle;
		UpdateAnimation();
	}

    public override void LookLeft(bool flag)
    {
        base.LookLeft(flag);

        Vector3 scale = transform.localScale;

        if (flag)
		{
			_exclamation.transform.localScale = new Vector3(Math.Abs(scale.x), scale.y, scale.z);
			_question.transform.localScale = new Vector3(Math.Abs(scale.x), scale.y, scale.z);
		}			
		else
		{
			_exclamation.transform.localScale = new Vector3(-Math.Abs(scale.x), scale.y, scale.z);
			_question.transform.localScale = new Vector3(-Math.Abs(scale.x), scale.y, scale.z);
		}
    }

    public void SetEmoticon(EEmoticonType type)
    {
        if (_emoticon != EEmoticonType.None && _emoticon == type)
			return;

        _emoticon = type;

        switch (type)
        {
            case EEmoticonType.None:
				_exclamation?.SetActive(false);
				_question?.SetActive(false);
				break;
            case EEmoticonType.Exclamation:
				_exclamation?.SetActive(true);
				_question?.SetActive(false);
				Managers.Sound.Play(Define.ESound.Effect, "Sound_Exclamation");
				break;
			case EEmoticonType.Question:
				_exclamation?.SetActive(false);
				_question?.SetActive(true);
				Managers.Sound.Play(Define.ESound.Effect, "Sound_Question");
				break;
        }
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        if (JobTitle == Define.EJobTitleType.Cat)
			return;

        switch (State)
		{
            case Define.EAnimState.Idle:
				PlayAnimation(_data.aniIdle);
				ChangeSkin(_data.aniIdleSkin);
				break;
            case Define.EAnimState.Sweat:
				PlayAnimation(_data.aniSweat);
				ChangeSkin(_data.aniSweatSkin);
				break;
			case Define.EAnimState.Working:
				PlayAnimation(_data.aniWorking);
				ChangeSkin(_data.aniWorkingSkin);
				break;
			case Define.EAnimState.Walking:
				PlayAnimation(_data.aniWalk);
				ChangeSkin(_data.aniWalkSkin);
				break;
			case Define.EAnimState.Attack:
				PlayAnimationOnce(_data.aniAttack, _data.aniAttackSkin);
				break;  
        }
    }

    void OnClickPlayer(PointerEventData evt)
    {
        
    }
}
