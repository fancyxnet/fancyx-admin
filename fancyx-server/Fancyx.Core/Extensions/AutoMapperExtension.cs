using Fancyx.Core.Helpers;

namespace Fancyx.Core.Extensions
{
    public static class AutoMapperExtension
    {
        public static List<TTarget> MapperList<TSource, TTarget>(this List<TSource> sources)
        {
            return AutoMapperHelper.Instance.Map<List<TSource>, List<TTarget>>(sources);
        }
    }
}