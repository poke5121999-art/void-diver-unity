using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using UnityEngine;
using VContainer;

public class GameStageManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameContext _gameContext;

	private long _accumulatedExp;

	private long _baseExpFromRewardBox;

	private long _baseExpFromMonsterKill;

	private long _baseExpFromEscape;

	private long _difficultyBonusExp;

	private long _talentBonusExp;

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_eventSystem.OnEventOccured.Subscribe(OnEventOccured).AddTo(this);
	}

	private void OnEventOccured(IBaseEvent gameEvent)
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return;
		}
		if (_gameContext.CampaignId < 1)
		{
			return;
		}
		TCampaign campaignData = TCampaign.Table[_gameContext.CampaignId];
		if (campaignData.Category == ECampaignCategory.Tutorial)
		{
			return;
		}
		float difficultyRate = TDifficulty.Table[_gameContext.Difficulty].UserExpPercent;
		float variantRate = TVariantDifficulty.Table[campaignData.VariantDifficulty].UserExpPercent;
		float partyRate = (_gameContext.RxPlayers.Count < 2) ? 100f : TConst.PartyPlayExpMultiplyPercent.FloatValue;
		float difficultyMultiplier = difficultyRate / 100f * variantRate / 100f * partyRate / 100f;
		IList<BuffEffectInfo<ExpGainAmplifierEffect>> buffEffects = _gameManager.MyCharacterModel.GetBuffEffects<ExpGainAmplifierEffect>();
		float talentMultiplier = (buffEffects == null) ? 0f : buffEffects.Sum((BuffEffectInfo<ExpGainAmplifierEffect> e) => e.BuffEffect.Percent / 100f * (float)e.Stack);
		ulong myClientId = _gameManager.MyCharacterModel.ClientId;
		if (gameEvent is OpenRewardBoxEvent openRewardBoxEvent)
		{
			if (myClientId != openRewardBoxEvent.SourceClientId)
			{
				return;
			}
			int baseExp = TConst.ExpFromRewardBox.IntValue;
			long total = (long)((float)baseExp * talentMultiplier) + (long)((float)baseExp * difficultyMultiplier);
			if (total > 0)
			{
				_accumulatedExp += total;
			}
			_talentBonusExp += (long)((float)baseExp * talentMultiplier);
			_baseExpFromRewardBox += baseExp;
			_difficultyBonusExp = _difficultyBonusExp - baseExp + (long)((float)baseExp * difficultyMultiplier);
			return;
		}
		if (gameEvent is MonsterKillEvent monsterKillEvent)
		{
			EMonsterType monsterType = TMonster.Table[monsterKillEvent.MonsterId].MonsterType;
			int exp = 0;
			switch (monsterType)
			{
			case EMonsterType.Normal:
				exp = TConst.ExpFromNormalMonster.IntValue;
				break;
			case EMonsterType.Boss:
			case EMonsterType.MiniBoss:
				exp = TConst.ExpFromBossMonster.IntValue;
				break;
			}
			if (myClientId == monsterKillEvent.SourceClientId)
			{
				int bonus = 0;
				switch (monsterType)
				{
				case EMonsterType.Normal:
					bonus = TConst.ExpBonusFromNormalMonsterKill.IntValue;
					break;
				case EMonsterType.Boss:
				case EMonsterType.MiniBoss:
					bonus = TConst.ExpBonusFromBossMonsterKill.IntValue;
					break;
				}
				exp += bonus;
			}
			if (exp < 1)
			{
				return;
			}
			long total = (long)((float)exp * talentMultiplier) + (long)((float)exp * difficultyMultiplier);
			if (total > 0)
			{
				_accumulatedExp += total;
			}
			_talentBonusExp += (long)((float)exp * talentMultiplier);
			_baseExpFromMonsterKill += exp;
			_difficultyBonusExp = _difficultyBonusExp - exp + (long)((float)exp * difficultyMultiplier);
			return;
		}
		if (gameEvent is StageEndEvent stageEndEvent)
		{
			// Only the escape bonus depends on IsEscaped; a death still reports its kill/box buckets and flushes the EXP
			// (ApplyExpGain @0x180760720: the IsEscaped test closes before the StageResultLocalInfo is built).
			if (stageEndEvent.IsEscaped && TCampaign.Table[stageEndEvent.CampaignId].HasEscapeExpReward)
			{
				int escapeExp = TConst.ExpFromEscape.IntValue;
				long difficultyExp = (long)((float)escapeExp * difficultyMultiplier);
				if (difficultyExp > 0)
				{
					_accumulatedExp += difficultyExp;
				}
				_baseExpFromEscape += escapeExp;
				_difficultyBonusExp = _difficultyBonusExp - escapeExp + difficultyExp;
			}
			SendStageLocalInfo();
			FlushAccumulatedExp();
			return;
		}
		if (gameEvent is CampaignClearEvent)
		{
			long clearExp = (long)((float)campaignData.GainedExp * difficultyMultiplier);
			if (clearExp > 0)
			{
				_accumulatedExp += clearExp;
			}
			FlushAccumulatedExp();
		}
	}

	private void AccumulateExp(long value)
	{
		if (value > 0)
		{
			_accumulatedExp += value;
		}
	}

	private void FlushAccumulatedExp()
	{
		if (_accumulatedExp > 0)
		{
			_gameChannel.SendToHost(new ReqGainAccumulatedExp
			{
				AccumulatedExp = _accumulatedExp
			});
			_accumulatedExp = 0L;
		}
	}

	private void SendStageLocalInfo()
	{
		StageResultLocalInfo localInfo = new StageResultLocalInfo
		{
			ClientId = _gameManager.LocalClientId,
			BaseExpFromRewardBox = _baseExpFromRewardBox,
			BaseExpFromMonsterKill = _baseExpFromMonsterKill,
			BaseExpFromEndStage = _baseExpFromEscape,
			DifficultyBonusExp = _difficultyBonusExp,
			TalentBonusExp = _talentBonusExp
		};
		_gameChannel.SendToHost(new ReqStageResultPlayerLocalInfo
		{
			LocalInfo = localInfo
		});
		ResetBuckets();
	}

	private void ResetBuckets()
	{
		_baseExpFromRewardBox = 0L;
		_baseExpFromMonsterKill = 0L;
		_baseExpFromEscape = 0L;
		_difficultyBonusExp = 0L;
		_talentBonusExp = 0L;
	}
}
