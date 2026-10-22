using GameCommon.Enum;
using UnityEngine;

public interface IInteractiveObject
{
	NetworkBase NetworkBase { get; }

	Vector3 Position { get; }

	float HoldingTime { get; }

	EOutlineType OutlineType { get; }

	string HoldingSfx { get; }

	string InteractionSfx { get; }

	ulong NetworkObjectId => NetworkBase.NetworkObjectId;

	Color OutlineColor => OutlineType.ToColor();

	float Stroke => OutlineType.ToStroke();

	bool CanInteract(UnitModel sourceModel);

	void OnStartHolding(ulong sourceClientId);

	void OnStartInteraction(ulong sourceClientId);

	void OnEndInteraction(ulong sourceClientId);
}
