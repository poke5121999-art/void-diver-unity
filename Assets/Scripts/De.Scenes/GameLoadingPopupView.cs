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

	public GameObject Root => null;

	public Image Background => null;

	public GameObject Layout => null;

	public Image Layout_LoadingSprite => null;

	public Image Layout_LoadingSpriteDeath => null;

	public Image Layout_LoadingSpriteEscape => null;

	public TextMeshProUGUI Layout_LoadingText => null;

	public GameObject TipLayout => null;

	public Image TipLayout_TipSprite => null;

	public TextMeshProUGUI TipLayout_TipText => null;
}
