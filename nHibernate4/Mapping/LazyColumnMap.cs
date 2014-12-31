using nHibernate4.Mapping.Base;
using nHibernate4.Model;

namespace nHibernate4.Mapping
{
    public class LazyColumnMap : MapBase<LazyColumn>
    {
        public LazyColumnMap()
        {
            Property(x => x.LazyLoadedColumn1, x => x.Lazy(true));
            Property(x => x.LazyLoadedColumn2, x => x.Lazy(true));
            Property(x => x.LazyLoadedColumn3, x => x.Lazy(true));

            Property(x => x.NonLazyLoadedColumn, x => x.Lazy(false));
        }
    }
}