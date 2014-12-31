using nHibernate4.Model.Base;

namespace nHibernate4.Model
{
    public class OrderLine : ModelBase
    {
        public virtual OrderHead OrderHead { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string Product { get; set; }
    }
}