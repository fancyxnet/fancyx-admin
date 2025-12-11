namespace Fancyx.Utils
{
    public static class CollectionUtils
    {
        /// <summary>
        /// 将扁平列表构建成树形结构（森林），支持层级内排序
        /// </summary>
        /// <typeparam name="TEntity">源数据类型</typeparam>
        /// <typeparam name="TNode">目标节点类型</typeparam>
        /// <typeparam name="TKey">主键类型（如 long, int, string）</typeparam>
        /// <param name="sourceList">扁平数据源</param>
        /// <param name="mapFunc">实体到节点的映射函数</param>
        /// <param name="keySelector">获取 Id 的表达式</param>
        /// <param name="parentIdSelector">获取 ParentId 的表达式（null 表示根）</param>
        /// <param name="childrenSetter">设置子节点集合的方法（解耦 Children 属性）</param>
        /// <param name="sortKeySelector">用于排序的字段选择器（如 x => x.Sort），若为 null 则不排序</param>
        /// <returns>根节点列表（森林）</returns>
        public static List<TNode> BuildTree<TEntity, TNode, TKey>(
            IEnumerable<TEntity> sourceList,
            Func<TEntity, TNode> mapFunc,
            Func<TEntity, TKey> keySelector,
            Func<TEntity, TKey?> parentIdSelector,
            Action<TNode, List<TNode>> childrenSetter,
            Func<TNode, object>? sortKeySelector = null)
            where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(sourceList);
            ArgumentNullException.ThrowIfNull(mapFunc);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(parentIdSelector);
            ArgumentNullException.ThrowIfNull(childrenSetter);

            var list = sourceList as IList<TEntity> ?? sourceList.ToList();
            if (list.Count == 0)
                return new List<TNode>();

            // 1. 构建节点字典：Id -> Node
            var nodeMap = new Dictionary<TKey, TNode>(list.Count);
            var entityByKey = new Dictionary<TKey, TEntity>(list.Count);

            foreach (var entity in list)
            {
                var key = keySelector(entity);
                nodeMap[key] = mapFunc(entity);
                entityByKey[key] = entity;
            }

            // 2. 按 ParentId 分组子节点（ParentId -> List<ChildNode>）
            var childrenGroups = new Dictionary<TKey, List<TNode>>();

            var roots = new List<TNode>();

            foreach (var entity in list)
            {
                var key = keySelector(entity);
                var node = nodeMap[key];
                var parentId = parentIdSelector(entity);

                if (parentId == null || !entityByKey.ContainsKey(parentId))
                {
                    // 根节点：ParentId 为空 或 父不存在
                    roots.Add(node);
                }
                else
                {
                    // 添加到父节点的子列表中
                    if (!childrenGroups.TryGetValue(parentId, out var children))
                    {
                        children = new List<TNode>();
                        childrenGroups[parentId] = children;
                    }
                    children.Add(node);
                }
            }

            // 3. 为每个父节点设置 Children，并对当前层级排序
            foreach (var kvp in childrenGroups)
            {
                var parentKey = kvp.Key;
                var children = kvp.Value;

                // 🔑 关键：只对当前父节点下的 children 做内部排序
                if (sortKeySelector != null)
                {
                    children.Sort((x, y) =>
                    {
                        var xVal = sortKeySelector(x);
                        var yVal = sortKeySelector(y);
                        return Comparer<object>.Default.Compare(xVal, yVal);
                    });
                }

                childrenSetter(nodeMap[parentKey], children);
            }

            // 4. 对根节点列表（顶层）进行内部排序
            if (sortKeySelector != null)
            {
                roots.Sort((x, y) =>
                {
                    var xVal = sortKeySelector(x);
                    var yVal = sortKeySelector(y);
                    return Comparer<object>.Default.Compare(xVal, yVal);
                });
            }

            return roots;
        }

        public static void SetLayerNames<TNode>(
            IEnumerable<TNode> roots,
            Func<TNode, string> getTitle,
            Action<TNode, string> setLayerName,
            Func<TNode, IList<TNode>?> getChildren,
            string separator = "/")
        {
            if (roots == null) return;
            foreach (var root in roots)
            {
                SetLayerNameRecursive(root, "", getTitle, setLayerName, getChildren, separator);
            }
        }

        private static void SetLayerNameRecursive<TNode>(
            TNode node,
            string parentPath,
            Func<TNode, string> getTitle,
            Action<TNode, string> setLayerName,
            Func<TNode, IList<TNode>?> getChildren,
            string separator)
        {
            var currentTitle = getTitle(node);
            var currentPath = string.IsNullOrEmpty(parentPath)
                ? currentTitle
                : $"{parentPath}{separator}{currentTitle}";

            setLayerName(node, currentPath);

            var children = getChildren(node);
            if (children?.Count > 0)
            {
                foreach (var child in children)
                {
                    SetLayerNameRecursive(child, currentPath, getTitle, setLayerName, getChildren, separator);
                }
            }
        }
    }
}
