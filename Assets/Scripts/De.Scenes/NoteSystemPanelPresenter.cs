using System.Threading;
using Cysharp.Threading.Tasks;
using Febucci.UI;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;

public class NoteSystemPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private NoteSystemPanelModel _model;

	[View]
	private NoteSystemPanelView _view;

	private TypewriterByCharacter _typewriter;

	private bool _isWriting;

	protected override BasePanelModel PanelModel => _model;

	public TextMeshProUGUI Message => _view.Message;

	public bool IsWriting => _isWriting;

	private void OnInject(NoteSystemPanelModel model)
	{
		_view.Message.text = _model.NoteText;
	}

	public async UniTask PlayMessageAsync(CancellationToken ct, string text)
	{
		while (true)
		{
			// MethodRef at 0x180908ed3 is Component.GetComponentInParent<Canvas> (generic argument read from the method spec)
			Canvas canvas = ((_view != null) ? _view.GetComponentInParent<Canvas>(includeInactive: true) : null);
			if (canvas != null && canvas.isActiveAndEnabled)
			{
				break;
			}
			await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, ct);
		}
		if (!(_view == null) && !(_view.Message == null))
		{
			_typewriter = _view.Message.GetComponent<TypewriterByCharacter>();
			if (!(_typewriter == null))
			{
				_typewriter.ShowText(text);
				_isWriting = true;
				_typewriter.onTextShowed.AsObservable().Subscribe(delegate
				{
					_isWriting = false;
				}).AddTo(this);
				_typewriter.StartShowingText(true);
			}
		}
	}

	public void SkipTypewriter()
	{
		if (_isWriting && _typewriter != null)
		{
			_typewriter.SkipTypewriter();
		}
	}
}
