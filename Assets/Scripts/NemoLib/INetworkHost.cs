using System.Collections.Generic;
using UniRx;

public interface INetworkHost
{
	HashSet<ulong> JoinedClientIds { get; }

	Subject<ulong> OnJoinCompleted { get; }
}
