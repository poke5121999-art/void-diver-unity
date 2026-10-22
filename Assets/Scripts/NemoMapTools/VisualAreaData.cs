using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VisualAreaData : MonoBehaviour
{
	[FormerlySerializedAs("waterAreaPositions")]
	[HideInInspector]
	public List<Vector3> areaPositions = new List<Vector3>();

	[HideInInspector]
	public float gridSize = 1f;

	[FormerlySerializedAs("waterMapTexture")]
	public Texture2D areaMapTexture;

	[FormerlySerializedAs("waterBounds")]
	public Bounds[] areaBounds = System.Array.Empty<Bounds>();
}
