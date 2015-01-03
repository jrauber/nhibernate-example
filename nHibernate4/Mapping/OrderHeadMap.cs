﻿using nHibernate4.Mapping.Base;
using nHibernate4.Model;
using NHibernate.Mapping.ByCode;

namespace nHibernate4.Mapping
{
    public class OrderHeadMap : MapBaseEnhSeq<OrderHead>
    {
        public OrderHeadMap()
        {
            Property(x => x.OrderNumber);

            Set(x => x.OrderLines, m => { m.Cascade(Cascade.All); }, z => { z.OneToMany(o => { }); });
        }
    }
}