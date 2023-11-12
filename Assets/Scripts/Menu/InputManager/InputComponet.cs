using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputComponent : MonoBehaviour
{
    [SerializeField] private Text buttonText;
	[SerializeField] private string defaultKeyName;
	[SerializeField] private KeyCode defaultKeyCode;

	public KeyCode keyCode { get; set; }
    
    private IEnumerator coroutine;

	private string tmpKey;

	public Text ButtonText
	{
		get{ return buttonText; }
	}

	public string DefaultKeyName
	{
		get{ return defaultKeyName; }
	}
	
	public KeyCode DefaultKeyCode
	{
		get{ return defaultKeyCode; }
	}

	public void ButtonSetKey()
	{
		tmpKey = buttonText.text;
		buttonText.text = "Press Key";
        coroutine = Wait();
		StartCoroutine(coroutine);
	}

	IEnumerator Wait()
	{
		while(true)
		{
			yield return null;

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				buttonText.text = tmpKey;
				StopCoroutine(Wait());
			}

			foreach(KeyCode key in KeyCode.GetValues(typeof(KeyCode)))
			{
				if(Input.GetKeyDown(key) && !Input.GetKeyDown(KeyCode.Escape))
				{
					keyCode = key;
					buttonText.text = key.ToString();
					StopCoroutine(coroutine);
				}
			}
		}
	}
}
