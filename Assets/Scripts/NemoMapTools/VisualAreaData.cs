using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VisualAreaData : MonoBehaviour
{
	[FormerlySerializedAs("waterAreaPositions")]
	[HideInInspector]
	public List<Vector3> areaPositions;

	[HideInInspector]
	public float gridSize;

	[FormerlySerializedAs("waterMapTexture")]
	public Texture2D areaMapTexture;

	[FormerlySerializedAs("waterBounds")]
	public Bounds[] areaBounds;
}
