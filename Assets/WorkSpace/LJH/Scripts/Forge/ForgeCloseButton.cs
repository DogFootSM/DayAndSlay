using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeCloseButton : MonoBehaviour
{
    [SerializeField] private SystemWindowController controller;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(Close);
	}


	public void Close()
	{
		controller.AllCloseSystemWindows();
	}
}
