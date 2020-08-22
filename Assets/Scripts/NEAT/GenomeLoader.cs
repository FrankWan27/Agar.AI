using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;

public class GenomeLoader : MonoBehaviour
{
	NEATManager neat;
	GameManager gm;
	private void Start()
	{
		neat = GetComponent<NEATManager>();
		gm = GetComponent<GameManager>();
	}

	public void LoadGenome()
	{
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(false, false, null, "Load Genome", "Load");

		if (FileBrowser.Success)
		{
			neat.LoadGenome(FileBrowser.Result[0], true);
			gm.ForceLoad();
		}
	}
}
