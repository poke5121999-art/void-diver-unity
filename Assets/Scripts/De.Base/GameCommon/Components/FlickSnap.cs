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
		public EnhancedScroller.TweenType SnapTweenType;

		public float SnapTweenTime;

		private EnhancedScroller _scroller;

		private IEnhancedScrollerDelegate _scrollerDelegate;

		private Vector2 _dragStartPosition;

		private bool _isDragging;

		private int _currentIndex;

		private EnhancedScroller.ScrollDirectionEnum SnapDirection => EnhancedScroller.ScrollDirectionEnum.Vertical;

		private int NumberOfCells => 0;

		private void Awake()
		{
		}

		public void OnBeginDrag(PointerEventData data)
		{
		}

		public void OnEndDrag(PointerEventData data)
		{
		}
	}
}
