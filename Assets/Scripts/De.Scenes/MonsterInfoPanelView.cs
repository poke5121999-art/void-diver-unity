using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoPanelView : View
{
	[SerializeField]
	private Button backButton;

	[SerializeField]
	private Image scroll;

	[SerializeField]
	private Image viewport;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject description;

	[SerializeField]
	private TextMeshProUGUI description_Text;

	[SerializeField]
	private GameObject elemental;

	[SerializeField]
	private GameObject elemental_Fire;

	[SerializeField]
	private GameObject elemental_Fire_NormalTag;

	[SerializeField]
	private GameObject elemental_Fire_StrongTag;

	[SerializeField]
	private TextMeshProUGUI elemental_Fire_Text;

	[SerializeField]
	private GameObject elemental_Fire_WeakTag;

	[SerializeField]
	private GameObject elemental_Water;

	[SerializeField]
	private GameObject elemental_Water_NormalTag;

	[SerializeField]
	private GameObject elemental_Water_StrongTag;

	[SerializeField]
	private TextMeshProUGUI elemental_Water_Text;

	[SerializeField]
	private GameObject elemental_Water_WeakTag;

	[SerializeField]
	private GameObject elemental_Wind;

	[SerializeField]
	private GameObject elemental_Wind_NormalTag;

	[SerializeField]
	private GameObject elemental_Wind_StrongTag;

	[SerializeField]
	private TextMeshProUGUI elemental_Wind_Text;

	[SerializeField]
	private GameObject elemental_Wind_WeakTag;

	[SerializeField]
	private GameObject stat;

	[SerializeField]
	private GameObject stat_Sight;

	[SerializeField]
	private TextMeshProUGUI stat_Sight_Value;

	[SerializeField]
	private GameObject stat_Speed;

	[SerializeField]
	private TextMeshProUGUI stat_Speed_Value;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private GameObject type;

	[SerializeField]
	private GameObject bossTag;

	[SerializeField]
	private TextMeshProUGUI categoryText;

	[SerializeField]
	private GameObject miniBossTag;

	public Button BackButton => backButton;

	public Image Scroll => scroll;

	public Image Viewport => viewport;

	public GameObject Content => content;

	public GameObject Description => description;

	public TextMeshProUGUI Description_Text => description_Text;

	public GameObject Elemental => elemental;

	public GameObject Elemental_Fire => elemental_Fire;

	public GameObject Elemental_Fire_NormalTag => elemental_Fire_NormalTag;

	public GameObject Elemental_Fire_StrongTag => elemental_Fire_StrongTag;

	public TextMeshProUGUI Elemental_Fire_Text => elemental_Fire_Text;

	public GameObject Elemental_Fire_WeakTag => elemental_Fire_WeakTag;

	public GameObject Elemental_Water => elemental_Water;

	public GameObject Elemental_Water_NormalTag => elemental_Water_NormalTag;

	public GameObject Elemental_Water_StrongTag => elemental_Water_StrongTag;

	public TextMeshProUGUI Elemental_Water_Text => elemental_Water_Text;

	public GameObject Elemental_Water_WeakTag => elemental_Water_WeakTag;

	public GameObject Elemental_Wind => elemental_Wind;

	public GameObject Elemental_Wind_NormalTag => elemental_Wind_NormalTag;

	public GameObject Elemental_Wind_StrongTag => elemental_Wind_StrongTag;

	public TextMeshProUGUI Elemental_Wind_Text => elemental_Wind_Text;

	public GameObject Elemental_Wind_WeakTag => elemental_Wind_WeakTag;

	public GameObject Stat => stat;

	public GameObject Stat_Sight => stat_Sight;

	public TextMeshProUGUI Stat_Sight_Value => stat_Sight_Value;

	public GameObject Stat_Speed => stat_Speed;

	public TextMeshProUGUI Stat_Speed_Value => stat_Speed_Value;

	public GameObject Title => title;

	public TextMeshProUGUI Name => name;

	public GameObject Type => type;

	public GameObject BossTag => bossTag;

	public TextMeshProUGUI CategoryText => categoryText;

	public GameObject MiniBossTag => miniBossTag;
}
