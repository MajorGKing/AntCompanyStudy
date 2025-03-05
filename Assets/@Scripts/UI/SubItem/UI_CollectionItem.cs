using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CollectionItem : UI_SubItem
{
	enum GameObjects
	{
		Collection,
		CollectionLock,
		Gallery,
	}

    enum Texts
    {
        CollectionNameText,
		LikeabilityText,
		WorkAbilityText,
		LuckText,
		MaxHpText,
    }

	enum Images
	{
		CollectionIcon,
		IconNotice,
		GalleryImage,
	}

	public enum CollectionItemType
	{
		Collection,
		Gallery
	}

	private int _collectionId;
	private int _galleryId;
	private CollectionItemType _type;

    protected override void Awake()
    {
        base.Awake();

		BindObjects(typeof(GameObjects));
		BindTexts(typeof(Texts));
		BindImages(typeof(Images));

		RefreshUI();
    }

	public void SetInfo()
	{

	}

	private void RefreshUI()
	{

	}
}
