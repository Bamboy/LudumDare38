using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour 
{
	public static Inventory singleton;

	public Transform iconContainer;
	public GameObject iconPrefab;


	public List<string> ItemNames;
	public List<Sprite> ItemIcons;


	void Awake () 
	{
		singleton = this;
		items = new Dictionary<string, bool>();
		//icons = new Dictionary<string, Sprite>();
		itemObjects = new Dictionary<string, GameObject>();

		for (int i = 0; i < ItemNames.Count; i++) 
		{
			AddItem( ItemNames[i], ItemIcons[i] );
		}
	}

	static Dictionary<string, GameObject> itemObjects;
	public static Dictionary<string, bool> items;
	//public static Dictionary<string, Sprite> icons;


	public static bool HasItem( string item )
	{
		try
		{
			return items[ item ];
		}
		catch
		{
			return false;
		}
	}

	public static void GiveItem( string item )
	{
		items[ item ] = true;
		itemObjects[ item ].SetActive( true );
	}
	public static void TakeItem( string item )
	{
		items[ item ] = false;
		itemObjects[ item ].SetActive( false );
	}

	void AddItem( string name, Sprite icon )
	{
		items.Add( name, false );

		GameObject obj = GameObject.Instantiate( iconPrefab, iconContainer, true );
		obj.GetComponent<Image>().sprite = icon;

		itemObjects.Add( name, obj );
		obj.SetActive( false );
	}
}
