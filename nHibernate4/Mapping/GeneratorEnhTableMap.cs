using nHibernate4.Mapping.Base;
using nHibernate4.Model;

namespace nHibernate4.Mapping
{
    public class GeneratorEnhTableMap : MapBaseEnhTable<GeneratorEnhTable>
    {
        public GeneratorEnhTableMap()
        {
            Property(x => x.Value);
        }
    }
}