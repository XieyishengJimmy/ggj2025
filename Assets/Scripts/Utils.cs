using UnityEngine;

public static class Utils
{
    /// <summary>
    /// 清空Transform下的所有子物体
    /// </summary>
    /// <param name="transform">目标Transform</param>
    public static void ClearChildren(this Transform transform)
    {
        if (transform == null)
        {
            Debug.LogError("Transform is null!");
            return;
        }

        // 缓存子物体数量，避免多次访问transform.childCount
        int childCount = transform.childCount;

        // 如果没有子物体，直接返回
        if (childCount == 0) return;

        // 从后向前遍历，这样在销毁时不会影响索引
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        // 清除所有子物体的引用
        transform.DetachChildren();
    }
}