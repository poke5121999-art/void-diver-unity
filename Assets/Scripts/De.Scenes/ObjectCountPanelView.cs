using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class ObjectCountPanelView : View
{
	[SerializeField]
	private GameObject interactive;

	[SerializeField]
	private GameObject interactive_DisposableObject;

	[SerializeField]
	private GameObject interactive_DisposableObject_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_DisposableObject_Value;

	[SerializeField]
	private GameObject interactive_DropGoods;

	[SerializeField]
	private GameObject interactive_DropGoods_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_DropGoods_Value;

	[SerializeField]
	private GameObject interactive_Entrance;

	[SerializeField]
	private GameObject interactive_Entrance_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_Entrance_Value;

	[SerializeField]
	private GameObject interactive_InteractiveTrigger;

	[SerializeField]
	private GameObject interactive_InteractiveTrigger_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_InteractiveTrigger_Value;

	[SerializeField]
	private GameObject interactive_MimicObject;

	[SerializeField]
	private GameObject interactive_MimicObject_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_MimicObject_Value;

	[SerializeField]
	private GameObject interactive_NpcObject;

	[SerializeField]
	private GameObject interactive_NpcObject_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_NpcObject_Value;

	[SerializeField]
	private GameObject interactive_PhoneBooth;

	[SerializeField]
	private GameObject interactive_PhoneBooth_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_PhoneBooth_Value;

	[SerializeField]
	private GameObject interactive_Portal;

	[SerializeField]
	private GameObject interactive_Portal_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_Portal_Value;

	[SerializeField]
	private GameObject interactive_RewardBox;

	[SerializeField]
	private GameObject interactive_RewardBox_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_RewardBox_Value;

	[SerializeField]
	private GameObject interactive_SkillExecutor;

	[SerializeField]
	private GameObject interactive_SkillExecutor_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_SkillExecutor_Value;

	[SerializeField]
	private GameObject interactive_Total;

	[SerializeField]
	private TextMeshProUGUI interactive_Total_Value;

	[SerializeField]
	private GameObject interactive_WaveExecutor;

	[SerializeField]
	private GameObject interactive_WaveExecutor_Pivot;

	[SerializeField]
	private TextMeshProUGUI interactive_WaveExecutor_Value;

	[SerializeField]
	private GameObject map;

	[SerializeField]
	private GameObject map_BreakableProp;

	[SerializeField]
	private GameObject map_BreakableProp_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_BreakableProp_Value;

	[SerializeField]
	private GameObject map_CollectableObject;

	[SerializeField]
	private GameObject map_CollectableObject_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_CollectableObject_Value;

	[SerializeField]
	private GameObject map_CollisionTrigger;

	[SerializeField]
	private GameObject map_CollisionTrigger_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_CollisionTrigger_Value;

	[SerializeField]
	private GameObject map_IntervalTrap;

	[SerializeField]
	private GameObject map_IntervalTrap_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_IntervalTrap_Value;

	[SerializeField]
	private GameObject map_LockedAreaObject;

	[SerializeField]
	private GameObject map_LockedAreaObject_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_LockedAreaObject_Value;

	[SerializeField]
	private GameObject map_SafeExit;

	[SerializeField]
	private GameObject map_SafeExit_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_SafeExit_Value;

	[SerializeField]
	private GameObject map_SpecialField;

	[SerializeField]
	private GameObject map_SpecialField_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_SpecialField_Value;

	[SerializeField]
	private GameObject map_Total;

	[SerializeField]
	private TextMeshProUGUI map_Total_Value;

	[SerializeField]
	private GameObject map_TriggerTrap;

	[SerializeField]
	private GameObject map_TriggerTrap_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_TriggerTrap_Value;

	[SerializeField]
	private GameObject map_WaveExit;

	[SerializeField]
	private GameObject map_WaveExit_Pivot;

	[SerializeField]
	private TextMeshProUGUI map_WaveExit_Value;

	[SerializeField]
	private GameObject unit;

	[SerializeField]
	private GameObject unit_Character;

	[SerializeField]
	private GameObject unit_Character_Pivot;

	[SerializeField]
	private TextMeshProUGUI unit_Character_Value;

	[SerializeField]
	private GameObject unit_Extra;

	[SerializeField]
	private GameObject unit_Extra_Pivot;

	[SerializeField]
	private TextMeshProUGUI unit_Extra_Value;

	[SerializeField]
	private GameObject unit_Monster;

	[SerializeField]
	private GameObject unit_Monster_Pivot;

	[SerializeField]
	private TextMeshProUGUI unit_Monster_Value;

	[SerializeField]
	private GameObject unit_Total;

	[SerializeField]
	private TextMeshProUGUI unit_Total_Value;

	public GameObject Interactive => null;

	public GameObject Interactive_DisposableObject => null;

	public GameObject Interactive_DisposableObject_Pivot => null;

	public TextMeshProUGUI Interactive_DisposableObject_Value => null;

	public GameObject Interactive_DropGoods => null;

	public GameObject Interactive_DropGoods_Pivot => null;

	public TextMeshProUGUI Interactive_DropGoods_Value => null;

	public GameObject Interactive_Entrance => null;

	public GameObject Interactive_Entrance_Pivot => null;

	public TextMeshProUGUI Interactive_Entrance_Value => null;

	public GameObject Interactive_InteractiveTrigger => null;

	public GameObject Interactive_InteractiveTrigger_Pivot => null;

	public TextMeshProUGUI Interactive_InteractiveTrigger_Value => null;

	public GameObject Interactive_MimicObject => null;

	public GameObject Interactive_MimicObject_Pivot => null;

	public TextMeshProUGUI Interactive_MimicObject_Value => null;

	public GameObject Interactive_NpcObject => null;

	public GameObject Interactive_NpcObject_Pivot => null;

	public TextMeshProUGUI Interactive_NpcObject_Value => null;

	public GameObject Interactive_PhoneBooth => null;

	public GameObject Interactive_PhoneBooth_Pivot => null;

	public TextMeshProUGUI Interactive_PhoneBooth_Value => null;

	public GameObject Interactive_Portal => null;

	public GameObject Interactive_Portal_Pivot => null;

	public TextMeshProUGUI Interactive_Portal_Value => null;

	public GameObject Interactive_RewardBox => null;

	public GameObject Interactive_RewardBox_Pivot => null;

	public TextMeshProUGUI Interactive_RewardBox_Value => null;

	public GameObject Interactive_SkillExecutor => null;

	public GameObject Interactive_SkillExecutor_Pivot => null;

	public TextMeshProUGUI Interactive_SkillExecutor_Value => null;

	public GameObject Interactive_Total => null;

	public TextMeshProUGUI Interactive_Total_Value => null;

	public GameObject Interactive_WaveExecutor => null;

	public GameObject Interactive_WaveExecutor_Pivot => null;

	public TextMeshProUGUI Interactive_WaveExecutor_Value => null;

	public GameObject Map => null;

	public GameObject Map_BreakableProp => null;

	public GameObject Map_BreakableProp_Pivot => null;

	public TextMeshProUGUI Map_BreakableProp_Value => null;

	public GameObject Map_CollectableObject => null;

	public GameObject Map_CollectableObject_Pivot => null;

	public TextMeshProUGUI Map_CollectableObject_Value => null;

	public GameObject Map_CollisionTrigger => null;

	public GameObject Map_CollisionTrigger_Pivot => null;

	public TextMeshProUGUI Map_CollisionTrigger_Value => null;

	public GameObject Map_IntervalTrap => null;

	public GameObject Map_IntervalTrap_Pivot => null;

	public TextMeshProUGUI Map_IntervalTrap_Value => null;

	public GameObject Map_LockedAreaObject => null;

	public GameObject Map_LockedAreaObject_Pivot => null;

	public TextMeshProUGUI Map_LockedAreaObject_Value => null;

	public GameObject Map_SafeExit => null;

	public GameObject Map_SafeExit_Pivot => null;

	public TextMeshProUGUI Map_SafeExit_Value => null;

	public GameObject Map_SpecialField => null;

	public GameObject Map_SpecialField_Pivot => null;

	public TextMeshProUGUI Map_SpecialField_Value => null;

	public GameObject Map_Total => null;

	public TextMeshProUGUI Map_Total_Value => null;

	public GameObject Map_TriggerTrap => null;

	public GameObject Map_TriggerTrap_Pivot => null;

	public TextMeshProUGUI Map_TriggerTrap_Value => null;

	public GameObject Map_WaveExit => null;

	public GameObject Map_WaveExit_Pivot => null;

	public TextMeshProUGUI Map_WaveExit_Value => null;

	public GameObject Unit => null;

	public GameObject Unit_Character => null;

	public GameObject Unit_Character_Pivot => null;

	public TextMeshProUGUI Unit_Character_Value => null;

	public GameObject Unit_Extra => null;

	public GameObject Unit_Extra_Pivot => null;

	public TextMeshProUGUI Unit_Extra_Value => null;

	public GameObject Unit_Monster => null;

	public GameObject Unit_Monster_Pivot => null;

	public TextMeshProUGUI Unit_Monster_Value => null;

	public GameObject Unit_Total => null;

	public TextMeshProUGUI Unit_Total_Value => null;
}
