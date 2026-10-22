using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Popup;
using NemoLib.Scene;
using UniRx;
using Unity.Netcode;
using UnityEngine.UI;
using VContainer;

public class InGameScene : BaseScenePresenter
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass65_0
	{
		public NpcObject npcObject;

		public InGameScene _003C_003E4__this;

		public List<DialogInfo> dialogInfos;

		internal bool _003COnNpcInteractionAsync_003Eb__24(int changedNpcId)
		{
			return false;
		}

		internal SelectableMessageData _003COnNpcInteractionAsync_003Eb__33(DialogInfo _)
		{
			return null;
		}

		internal bool _003COnNpcInteractionAsync_003Eb__35(KeyValuePair<ulong, ECampaignState> _)
		{
			return false;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnNpcInteractionAsync_003Ed__65 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public NpcObject npcObject;

		public InGameScene _003C_003E4__this;

		private _003C_003Ec__DisplayClass65_0 _003C_003E8__1;

		private UniTask<int>.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnSceneLoading_003Ed__51 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InGameScene _003C_003E4__this;

		private BlockScope _003CblockScope_003E5__2;

		private UniTask<GameLoadingPopupPresenter>.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[Inject]
	protected AppManager AppManager;

	[Inject]
	protected RoomContext RoomContext;

	[Inject]
	protected NetworkManager NetworkManager;

	[Inject]
	protected GameHost GameHost;

	[Inject]
	protected GameChannel GameChannel;

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected GameCampaignManager CampaignManager;

	[Inject]
	protected GameContext GameContext;

	[Inject]
	protected ToastManager ToastManager;

	[Inject]
	protected SoundManager SoundManager;

	[Inject]
	protected GameLuaController LuaController;

	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameMapManager MapManager;

	[Inject]
	protected IRoomMatchmaker RoomMatchmaker;

	[Inject]
	protected EnvironmentSettings Settings;

	[Inject]
	protected GameEventSystem EventSystem;

	[Inject]
	protected PlayerManager PlayerManager;

	[Inject]
	protected TycoonManager TycoonManager;

	[Inject]
	protected ArtifactDealManager ArtifactDealManager;

	[Inject]
	protected GameChatManager GameChatManager;

	[Inject]
	protected GameCutsceneManager GameCutsceneManager;

	[Inject]
	protected GameNpcDialogManager GameNpcDialogManager;

	[Inject]
	protected GameNpcTalkManager GameNpcTalkManager;

	[Inject]
	protected GameUiEventSystem UiEventSystem;

	[Inject]
	protected GraphicSettingService GraphicSettings;

	[Inject]
	protected StorageService StorageService;

	private Selectable _selectedObject;

	private bool _stagePlaying;

	private bool _isLooting;

	private ulong MyClientId => 0uL;

	private GamePlayer MyPlayer => null;

	private CharacterModel MyCharacterModel => null;

	private GameInputManager GameInputManager => null;

	private InputActions.InGameActions InGameActions => default;

	private InputActions.InGameKeyboardActions InGameKeyboardActions => default;

	protected override Selectable SelectedObject => _selectedObject;

	protected override void Start()
	{
	}

	private void OnSaveFailed(ESaveResult result)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void EnableInGameInput()
	{
	}

	private void DisableInGameInput()
	{
	}

	protected override void OnDestroy()
	{
	}

	[AsyncStateMachine(typeof(_003COnSceneLoading_003Ed__51))]
	private UniTask OnSceneLoading()
	{
		return default;
	}

	protected virtual void OnSceneLoadComplete()
	{
	}

	private void OnGameClosed()
	{
	}

	private void OnStagePlaying()
	{
	}

	private void OnStageNotPlaying()
	{
	}

	private void OnMyCharacterEscaped()
	{
	}

	private void OnStageEnded()
	{
	}

	private void OnMenuClick()
	{
	}

	private void OnMinimapClick()
	{
	}

	private void OnMinimapEscapeClick()
	{
	}

	private void OnInventoryClick()
	{
	}

	private void OnSceneEvent(SceneEvent sceneEvent)
	{
	}

	private void OnEndInteraction(NtfEndInteraction message)
	{
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
	}

	[AsyncStateMachine(typeof(_003COnNpcInteractionAsync_003Ed__65))]
	private UniTask OnNpcInteractionAsync(NpcObject npcObject)
	{
		return default;
	}

	private void OpenTycoonMenuOrNotify(int initialSalesSlotId = 0)
	{
	}

	private void OnCampaignVoteStarted((int campaignId, EDifficulty difficulty, EAnomalyType anomalyType) voteData)
	{
	}

	private void OnDisplayNoticeToast(NtfDisplayNoticeToast message)
	{
	}

	private void OnPlayerKilledToast(NtfPlayerKilledToast message)
	{
	}

	private void OnGiveUpCampaignVoteStarted(Unit unit)
	{
	}

	private void OnItemUsed(NtfItemUsed message)
	{
	}

	private void OnCreateLobby(Unit _)
	{
	}

	private void OnJoinLobby(bool isHost)
	{
	}

	private void OnPlayerEntered(GamePlayer player)
	{
	}

	private void OnPlayerLeft(GamePlayer player)
	{
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
	}
}
