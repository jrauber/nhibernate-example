using nHibernate4.Model.Base;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace nHibernate4.Mapping.Base
{
    public class MapBase<T> : ClassMapping<T> where T : ModelBase
    {
        public MapBase()
        {
            Id(x => x.Id, m => m.Generator(Generators.HighLow));

            Version(x => x.Version, m =>
            {
                m.Generated(VersionGeneration.Never);
                m.UnsavedValue(0);
            });
        }
    }
}