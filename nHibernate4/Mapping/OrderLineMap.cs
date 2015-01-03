using nHibernate4.Mapping.Base;
using nHibernate4.Model;

namespace nHibernate4.Mapping
{
    public class OrderLineMap : MapBaseEnhSeq<OrderLine>
    {
        public OrderLineMap()
        {
            Property(x => x.Quantity);
            Property(x => x.Product);

            ManyToOne(x => x.OrderHead);
        }
    }
}