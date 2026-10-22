using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCommon.Components
{
	[RequireComponent(typeof(Presenter))]
	[RequireComponent(typeof(EnhancedScroller))]
	public class FlickSnap : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler
	{
		public EnhancedScroller.TweenType SnapTweenType = EnhancedScroller.TweenType.easeOutQuad;

		public float SnapTweenTime = 0.2f;

		private EnhancedScroller _scroller;

		private IEnhancedScrollerDelegate _scrollerDelegate;

		private Vector2 _dragStartPosition;

		private bool _isDragging;

		private int _currentIndex = -1;

		private EnhancedScroller.ScrollDirectionEnum SnapDirection => _scroller.scrollDirection;

		private int NumberOfCells => _scrollerDelegate.GetNumberOfCells(_scroller);

		private void Awake()
		{
			_scroller = GetComponent<EnhancedScroller>();
			_scrollerDelegate = GetComponent<IEnhancedScrollerDelegate>();
		}

		public void OnBeginDrag(PointerEventData data)
		{
			if (_scroller != null)
			{
				if (_scroller.IsTweening)
				{
					return;
				}
				_isDragging = true;
				_dragStartPosition = data.position;
				_currentIndex = _scroller.StartDataIndex;
			}
		}

		public void OnEndDrag(PointerEventData data)
		{
			if (!_isDragging)
			{
				return;
			}
			_isDragging = false;
			if (_scroller == null || !_scroller.gameObject.activeInHierarchy)
			{
				return;
			}
			int num = -1;
			Vector2 vector = data.position - _dragStartPosition;
			if (SnapDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
			{
				if (vector.y < 0f)
				{
					num = _currentIndex - 1;
				}
				else if (vector.y > 0f)
				{
					num = _currentIndex + 1;
				}
				if (num != -1)
				{
					_scroller.JumpToDataIndex(Mathf.Clamp(num, 0, NumberOfCells - 1), 0f, 0f, useSpacing: true, SnapTweenType, SnapTweenTime);
				}
			}
			if (SnapDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal)
			{
				if (vector.x > 0f)
				{
					num = _currentIndex - 1;
				}
				else if (vector.x < 0f)
				{
					num = _currentIndex + 1;
				}
				if (num != -1)
				{
					_scroller.JumpToDataIndex(Mathf.Clamp(num, 0, NumberOfCells - 1), 0f, 0f, useSpacing: true, SnapTweenType, SnapTweenTime);
				}
			}
		}
	}
}
