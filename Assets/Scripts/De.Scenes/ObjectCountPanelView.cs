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

	public GameObject Interactive => interactive;

	public GameObject Interactive_DisposableObject => interactive_DisposableObject;

	public GameObject Interactive_DisposableObject_Pivot => interactive_DisposableObject_Pivot;

	public TextMeshProUGUI Interactive_DisposableObject_Value => interactive_DisposableObject_Value;

	public GameObject Interactive_DropGoods => interactive_DropGoods;

	public GameObject Interactive_DropGoods_Pivot => interactive_DropGoods_Pivot;

	public TextMeshProUGUI Interactive_DropGoods_Value => interactive_DropGoods_Value;

	public GameObject Interactive_Entrance => interactive_Entrance;

	public GameObject Interactive_Entrance_Pivot => interactive_Entrance_Pivot;

	public TextMeshProUGUI Interactive_Entrance_Value => interactive_Entrance_Value;

	public GameObject Interactive_InteractiveTrigger => interactive_InteractiveTrigger;

	public GameObject Interactive_InteractiveTrigger_Pivot => interactive_InteractiveTrigger_Pivot;

	public TextMeshProUGUI Interactive_InteractiveTrigger_Value => interactive_InteractiveTrigger_Value;

	public GameObject Interactive_MimicObject => interactive_MimicObject;

	public GameObject Interactive_MimicObject_Pivot => interactive_MimicObject_Pivot;

	public TextMeshProUGUI Interactive_MimicObject_Value => interactive_MimicObject_Value;

	public GameObject Interactive_NpcObject => interactive_NpcObject;

	public GameObject Interactive_NpcObject_Pivot => interactive_NpcObject_Pivot;

	public TextMeshProUGUI Interactive_NpcObject_Value => interactive_NpcObject_Value;

	public GameObject Interactive_PhoneBooth => interactive_PhoneBooth;

	public GameObject Interactive_PhoneBooth_Pivot => interactive_PhoneBooth_Pivot;

	public TextMeshProUGUI Interactive_PhoneBooth_Value => interactive_PhoneBooth_Value;

	public GameObject Interactive_Portal => interactive_Portal;

	public GameObject Interactive_Portal_Pivot => interactive_Portal_Pivot;

	public TextMeshProUGUI Interactive_Portal_Value => interactive_Portal_Value;

	public GameObject Interactive_RewardBox => interactive_RewardBox;

	public GameObject Interactive_RewardBox_Pivot => interactive_RewardBox_Pivot;

	public TextMeshProUGUI Interactive_RewardBox_Value => interactive_RewardBox_Value;

	public GameObject Interactive_SkillExecutor => interactive_SkillExecutor;

	public GameObject Interactive_SkillExecutor_Pivot => interactive_SkillExecutor_Pivot;

	public TextMeshProUGUI Interactive_SkillExecutor_Value => interactive_SkillExecutor_Value;

	public GameObject Interactive_Total => interactive_Total;

	public TextMeshProUGUI Interactive_Total_Value => interactive_Total_Value;

	public GameObject Interactive_WaveExecutor => interactive_WaveExecutor;

	public GameObject Interactive_WaveExecutor_Pivot => interactive_WaveExecutor_Pivot;

	public TextMeshProUGUI Interactive_WaveExecutor_Value => interactive_WaveExecutor_Value;

	public GameObject Map => map;

	public GameObject Map_BreakableProp => map_BreakableProp;

	public GameObject Map_BreakableProp_Pivot => map_BreakableProp_Pivot;

	public TextMeshProUGUI Map_BreakableProp_Value => map_BreakableProp_Value;

	public GameObject Map_CollectableObject => map_CollectableObject;

	public GameObject Map_CollectableObject_Pivot => map_CollectableObject_Pivot;

	public TextMeshProUGUI Map_CollectableObject_Value => map_CollectableObject_Value;

	public GameObject Map_CollisionTrigger => map_CollisionTrigger;

	public GameObject Map_CollisionTrigger_Pivot => map_CollisionTrigger_Pivot;

	public TextMeshProUGUI Map_CollisionTrigger_Value => map_CollisionTrigger_Value;

	public GameObject Map_IntervalTrap => map_IntervalTrap;

	public GameObject Map_IntervalTrap_Pivot => map_IntervalTrap_Pivot;

	public TextMeshProUGUI Map_IntervalTrap_Value => map_IntervalTrap_Value;

	public GameObject Map_LockedAreaObject => map_LockedAreaObject;

	public GameObject Map_LockedAreaObject_Pivot => map_LockedAreaObject_Pivot;

	public TextMeshProUGUI Map_LockedAreaObject_Value => map_LockedAreaObject_Value;

	public GameObject Map_SafeExit => map_SafeExit;

	public GameObject Map_SafeExit_Pivot => map_SafeExit_Pivot;

	public TextMeshProUGUI Map_SafeExit_Value => map_SafeExit_Value;

	public GameObject Map_SpecialField => map_SpecialField;

	public GameObject Map_SpecialField_Pivot => map_SpecialField_Pivot;

	public TextMeshProUGUI Map_SpecialField_Value => map_SpecialField_Value;

	public GameObject Map_Total => map_Total;

	public TextMeshProUGUI Map_Total_Value => map_Total_Value;

	public GameObject Map_TriggerTrap => map_TriggerTrap;

	public GameObject Map_TriggerTrap_Pivot => map_TriggerTrap_Pivot;

	public TextMeshProUGUI Map_TriggerTrap_Value => map_TriggerTrap_Value;

	public GameObject Map_WaveExit => map_WaveExit;

	public GameObject Map_WaveExit_Pivot => map_WaveExit_Pivot;

	public TextMeshProUGUI Map_WaveExit_Value => map_WaveExit_Value;

	public GameObject Unit => unit;

	public GameObject Unit_Character => unit_Character;

	public GameObject Unit_Character_Pivot => unit_Character_Pivot;

	public TextMeshProUGUI Unit_Character_Value => unit_Character_Value;

	public GameObject Unit_Extra => unit_Extra;

	public GameObject Unit_Extra_Pivot => unit_Extra_Pivot;

	public TextMeshProUGUI Unit_Extra_Value => unit_Extra_Value;

	public GameObject Unit_Monster => unit_Monster;

	public GameObject Unit_Monster_Pivot => unit_Monster_Pivot;

	public TextMeshProUGUI Unit_Monster_Value => unit_Monster_Value;

	public GameObject Unit_Total => unit_Total;

	public TextMeshProUGUI Unit_Total_Value => unit_Total_Value;
}
