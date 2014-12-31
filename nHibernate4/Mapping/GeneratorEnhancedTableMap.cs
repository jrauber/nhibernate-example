using nHibernate4.Mapping.Base;
using nHibernate4.Model;

namespace nHibernate4.Mapping
{
    public class GeneratorEnhancedTableMap : MapBaseEnhancedTable<GeneratorEnhancedTable>
    {
        public GeneratorEnhancedTableMap()
        {
            Property(x => x.Value);
        }
    }
}