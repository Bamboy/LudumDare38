using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayString : MonoBehaviour 
{
	public static DisplayString singleton;
	private AudioSource dialogAudio;
	private Text display;

	private static Image pointer;
	private static Image window;
	void Awake () 
	{
		singleton = this;
		display = GetComponentInChildren<Text>();
		dialogAudio = GetComponent<AudioSource>();

		pointer = transform.FindChild("Pointer").GetComponent<Image>();
		window = GetComponent<Image>();

		StartCoroutine(Display());
	}


	public float characterDelay = 0.1f;
	private string _fullText = "Ludum Dare 38";
	public string text
	{
		get{ return _fullText; }
		set{
			charIndex = 0;
			//doReset = true;
			_fullText = value;
		}
	}
	public int charIndex = 0;
	private bool useButtonDown = true;
	public static bool dialogIsOpen{ get{ return sequence != null; } }
	public static bool displayingCharacters{ get{ return dialogIsOpen && (singleton.charIndex >= sequence.dialog[ dialogIndex ].Length - 1); } }

	public static DialogSequence sequence = null;
	public static int dialogIndex = 0;
	public static void StartDialog( DialogSequence seq )
	{
		dialogIndex = 0;
		sequence = seq;
	}

	private bool moveNext = false;
	void Update()
	{
		if( Input.GetButtonDown("Use") && dialogIsOpen )
		{
			useButtonDown = true;
			if( displayingCharacters )
			{
				charIndex = sequence.dialog[ dialogIndex ].Length - 1; //Skip text animation
				display.text = sequence.dialog[ dialogIndex ];
				skippedAnim = true;
			}

		}
		debugSequence = sequence;
		dialogSeqIndex = dialogIndex;
	}
	void LateUpdate()
	{
		useButtonDown = false;
	}
	public DialogSequence debugSequence = null;
	public int dialogSeqIndex;
	public bool skippedAnim = false;
	public bool charBreak = false;
	IEnumerator Display()
	{
		while( true )
		{
			if( sequence == null )
			{
				pointer.enabled = false;
				window.enabled = false;
			}
			while( sequence == null )
				yield return null;
			
			window.enabled = true;
			pointer.enabled = false;

			for (int i = 0; i < sequence.dialog[ dialogIndex ].Length; i++) //Display char Loop
			{
				charIndex = i;

				if( skippedAnim )
				{
					break;
				}
				else
				{
					if( charIndex == sequence.dialog[ dialogIndex ].Length - 1 )
					{
						charBreak = true;
					}
					else
					{
						dialogAudio.volume = 0.4f;
						DisplayPartial( sequence.dialog[ dialogIndex ], charIndex );
					}
				}

				if( charBreak || useButtonDown )
				{
					dialogAudio.volume = 0.0f;
					display.text = sequence.dialog[ dialogIndex ];

					pointer.enabled = true;

					useButtonDown = false;
					while( useButtonDown == false )
						yield return null;
					break; //Wait until the user presses use before moving to the next string
				}

				yield return null;
			}
			charBreak = false;

			if( skippedAnim )
			{
				skippedAnim = false;
				useButtonDown = false;
				while( true )
				{
					yield return null;
					if( useButtonDown = true )
						break;
				}
			}

			//useButtonDown = false;
			//while( useButtonDown == false )
			//	yield return null;

			//Always reset text when we get here.
			charIndex = 0;
			useButtonDown = false;

			//if( moveNext )
			//{
				if( sequence.dialog.Count - 1 <= dialogIndex ) //Check if we've reached the end of dialog sequence
				{
					//sequence.dialog[ dialogIndex ] = sequence.dialog[ dialogIndex ];
					sequence = null;
					dialogIndex = 0;
					pointer.enabled = false;
					window.enabled = false;
				}
				else
				{
					dialogIndex++;
				}
			//	moveNext = false;
			//}
			skippedAnim = false;
		}
	}

	void DisplayPartial( string full, int charCount )
	{
		display.text = full.Remove( charCount );
	}
}
