using nHibernate4.Model.Base;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace nHibernate4.Mapping.Base
{
    public class MapBaseEnhTable<T> : ClassMapping<T> where T : ModelBase
    {
        public MapBaseEnhTable()
        {
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.EnhancedTable, g => g.Params(new
                {
                    segment_value = "enhanced_table",
                    optimizer = "pooled",
                    schema = "",
                    catalog = "",
                    parameters = "",
                    increment_size = 20
                }));
            });

            Version(x => x.Version, m =>
            {
                m.Generated(VersionGeneration.Never);
                m.UnsavedValue(0);
            });
        }
    }
}