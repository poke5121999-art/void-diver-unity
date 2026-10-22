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

	public Button BackButton => null;

	public Image Scroll => null;

	public Image Viewport => null;

	public GameObject Content => null;

	public GameObject Description => null;

	public TextMeshProUGUI Description_Text => null;

	public GameObject Elemental => null;

	public GameObject Elemental_Fire => null;

	public GameObject Elemental_Fire_NormalTag => null;

	public GameObject Elemental_Fire_StrongTag => null;

	public TextMeshProUGUI Elemental_Fire_Text => null;

	public GameObject Elemental_Fire_WeakTag => null;

	public GameObject Elemental_Water => null;

	public GameObject Elemental_Water_NormalTag => null;

	public GameObject Elemental_Water_StrongTag => null;

	public TextMeshProUGUI Elemental_Water_Text => null;

	public GameObject Elemental_Water_WeakTag => null;

	public GameObject Elemental_Wind => null;

	public GameObject Elemental_Wind_NormalTag => null;

	public GameObject Elemental_Wind_StrongTag => null;

	public TextMeshProUGUI Elemental_Wind_Text => null;

	public GameObject Elemental_Wind_WeakTag => null;

	public GameObject Stat => null;

	public GameObject Stat_Sight => null;

	public TextMeshProUGUI Stat_Sight_Value => null;

	public GameObject Stat_Speed => null;

	public TextMeshProUGUI Stat_Speed_Value => null;

	public GameObject Title => null;

	public TextMeshProUGUI Name => null;

	public GameObject Type => null;

	public GameObject BossTag => null;

	public TextMeshProUGUI CategoryText => null;

	public GameObject MiniBossTag => null;
}
