using UnityEngine;

public class ConeEffectIndicator : SkillIndicator
{
	private static readonly int _coneAngleId = Shader.PropertyToID("_ConeAngle");

	private MaterialPropertyBlock _mpb;

	private float _angle;

	public void Set(float radius, float angle)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		base.transform.localScale = new Vector3(radius, 1f, radius);
		_angle = angle;
		ApplyConeAngle();
	}

	private void Update()
	{
		Vector3 deltaAimPos = unitModel.DeltaAimPos;
		if (deltaAimPos.sqrMagnitude > 0f)
		{
			base.transform.rotation = Quaternion.LookRotation(deltaAimPos, Vector3.up);
		}
	}

	private void ApplyConeAngle()
	{
		if (!(SkinnedMeshRenderer == null))
		{
			if (_mpb == null)
			{
				_mpb = new MaterialPropertyBlock();
			}
			SkinnedMeshRenderer.GetPropertyBlock(_mpb, 0);
			_mpb.SetFloat(_coneAngleId, _angle);
			SkinnedMeshRenderer.SetPropertyBlock(_mpb, 0);
		}
	}
}
