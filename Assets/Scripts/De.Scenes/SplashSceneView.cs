using NemoLib.Mvp;
using UnityEngine;

public class SplashSceneView : View
{
	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private GameObject dim;

	[SerializeField]
	private GameObject splashVideo;

	public Canvas Canvas => canvas;

	public GameObject Dim => dim;

	public GameObject SplashVideo => splashVideo;
}
