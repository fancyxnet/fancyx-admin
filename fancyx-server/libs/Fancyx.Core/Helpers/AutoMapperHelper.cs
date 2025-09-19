using AutoMapper;

namespace Fancyx.Core.Helpers
{
    public class AutoMapperHelper
    {
        private static readonly Lazy<IMapper> lazyInstance = new(() => _mapper!);

        public static IMapper Instance => lazyInstance.Value;

        private static readonly IMapper _mapper;

        static AutoMapperHelper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(FrameConfiguration.LoadAssemblies);
            });

            _mapper = config.CreateMapper();
        }
    }
}