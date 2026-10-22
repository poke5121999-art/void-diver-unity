using System.Collections.Generic;
using UnityEngine;

public class MeshRuntimeCombineObject : MonoBehaviour
{
	private MeshRenderer[] _targetMeshRenderers;

	private const float _numberPositionUnit = 50f;

	private void Start()
	{
		_targetMeshRenderers = GetComponentsInChildren<MeshRenderer>();
		if (_targetMeshRenderers == null || _targetMeshRenderers.Length == 0)
		{
			Debug.LogWarning("병합할 대상 오브젝트를 지정해주세요.");
			return;
		}
		Transform root = base.transform;
		Matrix4x4 rootInverse = ((root != null) ? root.localToWorldMatrix : Matrix4x4.identity).inverse;
		Dictionary<Material, List<CombineInstance>> instancesByMaterial = new Dictionary<Material, List<CombineInstance>>();
		Dictionary<Material, List<Mesh>> tempMeshesByMaterial = new Dictionary<Material, List<Mesh>>();
		MeshRenderer[] targets = _targetMeshRenderers;
		foreach (MeshRenderer renderer in targets)
		{
			if (renderer == null)
			{
				continue;
			}
			MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
			if (!(meshFilter != null) || !(meshFilter.sharedMesh != null))
			{
				continue;
			}
			Mesh sharedMesh = meshFilter.sharedMesh;
			for (int sub = 0; sub < sharedMesh.subMeshCount; sub++)
			{
				if (sub >= renderer.sharedMaterials.Length)
				{
					continue;
				}
				Material material = renderer.sharedMaterials[sub];
				if (!instancesByMaterial.ContainsKey(material))
				{
					instancesByMaterial[material] = new List<CombineInstance>();
					tempMeshesByMaterial[material] = new List<Mesh>();
				}
				Mesh meshCopy = Object.Instantiate(sharedMesh);
				tempMeshesByMaterial[material].Add(meshCopy);
				Vector3 position = meshFilter.transform.position;
				if (root != null)
				{
					position = root.InverseTransformPoint(position);
				}
				// Encode the (normalized) local position of this piece into vertex color.
				Vector3 normalized = (position + Vector3.one * 50f * 0.5f) / 50f;
				float r = Mathf.Min(Mathf.Max(Vector3.zero.x, normalized.x), Vector3.one.x);
				float g = Mathf.Min(Mathf.Max(Vector3.zero.y, normalized.y), Vector3.one.y);
				float b = Mathf.Min(Mathf.Max(Vector3.zero.z, normalized.z), Vector3.one.z);
				Color[] colors = new Color[meshCopy.vertexCount];
				for (int i = 0; i < colors.Length; i++)
				{
					colors[i] = new Color(r, g, b, 1f);
				}
				meshCopy.colors = colors;
				CombineInstance combine = default(CombineInstance);
				combine.mesh = meshCopy;
				combine.subMeshIndex = sub;
				combine.transform = rootInverse * meshFilter.transform.localToWorldMatrix;
				instancesByMaterial[material].Add(combine);
			}
			renderer.gameObject.SetActive(false);
		}
		if (instancesByMaterial.Count == 0)
		{
			Debug.LogWarning("MeshFilter 또는 MeshRenderer가 유효하지 않습니다.");
			return;
		}
		List<Mesh> combinedMeshes = new List<Mesh>();
		List<Material> combinedMaterials = new List<Material>();
		foreach (KeyValuePair<Material, List<CombineInstance>> pair in instancesByMaterial)
		{
			Mesh combined = new Mesh();
			combined.name = "Combined_" + pair.Key.name;
			combined.CombineMeshes(pair.Value.ToArray(), true, true);
			combinedMeshes.Add(combined);
			combinedMaterials.Add(pair.Key);
			foreach (Mesh temp in tempMeshesByMaterial[pair.Key])
			{
				Object.Destroy(temp);
			}
		}
		List<CombineInstance> finalInstances = new List<CombineInstance>();
		for (int j = 0; j < combinedMeshes.Count; j++)
		{
			CombineInstance final = default(CombineInstance);
			final.mesh = combinedMeshes[j];
			final.transform = Matrix4x4.identity;
			finalInstances.Add(final);
		}
		Mesh finalMesh = new Mesh();
		finalMesh.name = "Combined_Final";
		finalMesh.CombineMeshes(finalInstances.ToArray(), false, false);
		GameObject combinedObject = new GameObject("Combined Object");
		combinedObject.AddComponent<MeshFilter>().sharedMesh = finalMesh;
		combinedObject.AddComponent<MeshRenderer>().materials = combinedMaterials.ToArray();
		combinedObject.transform.SetParent(root);
		combinedObject.transform.localPosition = Vector3.zero;
		combinedObject.transform.localRotation = Quaternion.identity;
		combinedObject.transform.localScale = Vector3.one;
		combinedObject.layer = root.gameObject.layer;
		Debug.Log(string.Format("런타임 메시 병합 완료: {0}개 오브젝트가 병합되었습니다.", _targetMeshRenderers.Length));
	}
}
