using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using VContainer;

public class AccountRecordManager : NativeBase, IDisposable
{
	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private StorageService _saveStorage;

	[Inject]
	private AppManager _appManager;

	private AccountRecordContext _context;

	private CompositeDisposable _disposable;

	private bool _isDirty;

	private bool _isInitialized;

	private string _accountId;

	private string ContextFileName => "AccountRecordContext";

	public void Initialize()
	{
		if (_isInitialized)
		{
			return;
		}
		_accountId = _appManager.AccountId;
		StorageLoadResult<AccountRecordContextData> storageLoadResult = _saveStorage.Load<AccountRecordContextData>(ContextFileName, _accountId);
		AccountRecordContextData accountRecordContextData;
		if (storageLoadResult.LoadResult == ELoadResult.Success)
		{
			accountRecordContextData = storageLoadResult.Data;
		}
		else
		{
			accountRecordContextData = new AccountRecordContextData();
			accountRecordContextData.AccountId = _accountId;
		}
		_context = new AccountRecordContext(accountRecordContextData);
		_disposable = new CompositeDisposable();
		_eventSystem.OnEventOccured.Where((IBaseEvent e) => !(e is AccountRecordEvent)).Subscribe(OnGameEventOccured).AddTo(_disposable);
		_isInitialized = true;
	}

	public void Dispose()
	{
		_disposable?.Dispose();
	}

	public long GetCount(int recordId)
	{
		return _context.GetCount(recordId);
	}

	public void Save()
	{
		if (_isDirty)
		{
			_saveStorage.Save(ContextFileName, _context.ToData(_accountId));
			_isDirty = false;
		}
	}

	public async UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (_isDirty)
		{
			await _saveStorage.SaveAsync(ContextFileName, _context.ToData(_accountId), cancellationToken);
			_isDirty = false;
		}
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
		foreach (TAccountRecord value in TAccountRecord.Table.Values)
		{
			long num = triggerEvent.CalculateProgress(value.EventCondition, 0L, long.MaxValue);
			if (num > 0)
			{
				long newCount = _context.Add(value.Id, num);
				_eventSystem.Trigger(new AccountRecordEvent(value.Id, newCount));
				_isDirty = true;
			}
		}
	}
}
