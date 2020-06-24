#if !UNITY_WINRT || UNITY_EDITOR || (UNITY_WP8 &&  !UNITY_WP_8_1)

namespace Newtonsoft.Json.ObservableSupport
{
	public enum NotifyCollectionChangedAction
	{
		Add,
		Remove,
		Replace,
		Move,
		Reset
	}
}

#endif