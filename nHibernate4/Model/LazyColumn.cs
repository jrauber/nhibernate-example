using nHibernate4.Model.Base;

namespace nHibernate4.Model
{
    public class LazyColumn : ModelBase
    {
        public virtual string LazyLoadedColumn1 { get; set; }
        public virtual string LazyLoadedColumn2 { get; set; }
        public virtual string LazyLoadedColumn3 { get; set; }
        public virtual string NonLazyLoadedColumn { get; set; }
    }
}