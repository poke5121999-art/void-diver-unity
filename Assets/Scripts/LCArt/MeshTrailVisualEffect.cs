using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MeshTrailVisualEffect : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTrailing_CO_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MeshTrailVisualEffect _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CTrailing_CO_003Ed__28(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Header("Trail Setting")]
	[SerializeField]
	private int _trailCount;

	private const int _trailMaxCount = 20;

	[SerializeField]
	private float _trailPeriod;

	[SerializeField]
	private Color _trailStartColor;

	[SerializeField]
	private Color _trailEndColor;

	private int _trailIndex;

	[SerializeField]
	private List<MeshFilter> _meshFilters;

	[SerializeField]
	private List<MeshRenderer> _meshRenderers;

	private List<MeshTrailClone> _trailClones;

	private GameObject _trailCloneHolder;

	private IEnumerator _trailing;

	private bool _stillTrailing;

	[Header("Shader Start Setting")]
	[SerializeField]
	private Shader _shader;

	[SerializeField]
	private Material _material;

	private void Start()
	{
	}

	private void OnValidate()
	{
	}

	public void SetTrailCount(int count)
	{
	}

	public void SetTrailPeriod(float period)
	{
	}

	public void SetTrailColor(Color startColor, Color endColor)
	{
	}

	private bool IsValidFilters()
	{
		return false;
	}

	private bool IsValidRenderers()
	{
		return false;
	}

	private bool IsValidIndexClone()
	{
		return false;
	}

	private void InitializeMeshInformations()
	{
	}

	private void InitializeClones()
	{
	}

	public void SetActiveTrail(bool active)
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void ResetTrail()
	{
	}

	[IteratorStateMachine(typeof(_003CTrailing_CO_003Ed__28))]
	private IEnumerator Trailing_CO()
	{
		return null;
	}

	private void ActiveTrailClone()
	{
	}

	private Mesh GetActiveMesh()
	{
		return null;
	}

	[ContextMenu("ActiveTrue")]
	public void SetActiveTrue()
	{
	}

	[ContextMenu("ActiveFalse")]
	public void SetActiveFalse()
	{
	}

	[ContextMenu("Assign Renderer Components")]
	private void GetRendererComponents()
	{
	}
}
