using System.Collections.Generic;
using nHibernate4.Model.Base;

namespace nHibernate4.Model
{
    public class OrderHead : ModelBase
    {
        public OrderHead()
        {
            OrderLines = new HashSet<OrderLine>();
        }

        public virtual string OrderNumber { get; set; }
        public virtual ISet<OrderLine> OrderLines { get; set; }
    }
}