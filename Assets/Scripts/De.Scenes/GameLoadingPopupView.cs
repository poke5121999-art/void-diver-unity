using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLoadingPopupView : View
{
	[SerializeField]
	private GameObject root;

	[SerializeField]
	private Image background;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private Image layout_LoadingSprite;

	[SerializeField]
	private Image layout_LoadingSpriteDeath;

	[SerializeField]
	private Image layout_LoadingSpriteEscape;

	[SerializeField]
	private TextMeshProUGUI layout_LoadingText;

	[SerializeField]
	private GameObject tipLayout;

	[SerializeField]
	private Image tipLayout_TipSprite;

	[SerializeField]
	private TextMeshProUGUI tipLayout_TipText;

	public GameObject Root => root;

	public Image Background => background;

	public GameObject Layout => layout;

	public Image Layout_LoadingSprite => layout_LoadingSprite;

	public Image Layout_LoadingSpriteDeath => layout_LoadingSpriteDeath;

	public Image Layout_LoadingSpriteEscape => layout_LoadingSpriteEscape;

	public TextMeshProUGUI Layout_LoadingText => layout_LoadingText;

	public GameObject TipLayout => tipLayout;

	public Image TipLayout_TipSprite => tipLayout_TipSprite;

	public TextMeshProUGUI TipLayout_TipText => tipLayout_TipText;
}
