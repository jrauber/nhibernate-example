using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Config;
using nHibernate4.Model;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;

namespace nHibernate4
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Program));

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure(); //Log4Net

            var cfg = new Configuration().Configure();

            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            cfg.AddMapping(mapping);

            var sf = cfg.BuildSessionFactory();

            #region LazyLoaded-Columns ([NH-429])

            // Erzeugen der Daten
            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var lazy = new LazyColumn();

                lazy.NonLazyLoadedColumn = "NonLazy";
                lazy.LazyLoadedColumn1 = "Lazy1";
                lazy.LazyLoadedColumn2 = "Lazy2";
                lazy.LazyLoadedColumn3 = "Lazy3";

                session.Save(lazy);
                tx.Commit();
            }

            // Abfragen der Daten
            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var test = session.Query<LazyColumn>().First();

                var cap1 = test.LazyLoadedColumn1;
                var cap2 = test.LazyLoadedColumn2;
                var cap3 = test.LazyLoadedColumn3;

                tx.Commit();
            }

            #endregion

            #region Mapping-By-Code (SET, One-To-Many, Many-To-One)

            // Mapping per Code wir in den Klassen OrderHeadMap und 
            // OrderLineMap vorgenommen.
            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                for (var i = 0; i < 100; i++)
                {
                    var orderHead = new OrderHead
                    {
                        OrderNumber = "ORDER#" + i,
                        OrderLines = new HashSet<OrderLine>()
                    };


                    var orderLine = new OrderLine
                    {
                        OrderHead = orderHead,
                        Product = "PRODUCT#" + i,
                        Quantity = i
                    };

                    orderHead.OrderLines.Add(orderLine);

                    session.SaveOrUpdate(orderHead);
                }

                tx.Commit();
            }

            #endregion

            #region ReadOnly Objects in Querys ([NH-908])

            long orderId;

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var orders = session.CreateQuery("from " + typeof (OrderHead))
                    .SetReadOnly(true)
                    .List<OrderHead>();

                var order = orders.First();
                var orderLine = orders.First().OrderLines.First(); // Da LazyLoaded gilt hier das 
                // ReadOnly nicht mehr!

                orderId = order.Id;
                order.OrderNumber = "CHANGED"; // Wird nicht gespeichert

                orderLine.Product = "CHANGED"; // Wird gespeichert

                session.SaveOrUpdate(order);

                tx.Commit();
            }

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var order = session.Get<OrderHead>(orderId);

                Debug.Assert(order.OrderNumber.StartsWith("ORDER#"), "ORDERNUMBER DARF NICHT GEÄNDERT SEIN!");
                Debug.Assert(order.OrderLines.First().Product.Equals("CHANGED"), "PRODUCT MUSS GEÄNDERT SEIN!");
            }

            #endregion

            #region EnhancedGenerator ([NH-2980] und [NH-2953])

            // Schreibt Objekte die mit einer ID aus einer Tabelle
            // versehen werden in die DB.

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                for (var i = 0; i < 100; i++)
                {
                    var obj = new GeneratorEnhancedTable
                    {
                        Value = "Obj" + i
                    };

                    session.SaveOrUpdate(obj);
                }

                tx.Commit();
            }

            // Schreibt Objekte die mit einer ID aus einer Sequence
            // versehen werden in die DB. Sequence Funktion allerdings
            // nicht in jedem DBMS vorhanden, dann Fallback auf Table

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                for (var i = 0; i < 100; i++)
                {
                    var obj = new GeneratorEnhancedSequence
                    {
                        Value = "Obj" + i
                    };

                    session.SaveOrUpdate(obj);
                }

                tx.Commit();
            }

            #endregion

            #region Paging in HQL ([NH-2533])

            using (var session = sf.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                //Abfrage Variante mit HQL
                var ordersHQL = session.CreateQuery("from " + typeof (OrderHead) + " SKIP 20 TAKE 10").List<OrderHead>();

                //Abfrage Variante per LINQ
                var ordersLinq = session.Query<OrderHead>().Skip(20).Take(10).ToList();

                // Page drei abrufen
                var pageSize = 20;
                var currentPage = 3;

                var pageThree = session.CreateQuery("from " + typeof (OrderHead) + " SKIP :skipVal TAKE :takeVal")
                    .SetInt32("skipVal", pageSize*(currentPage - 1))
                    .SetInt32("takeVal", pageSize)
                    .List<OrderHead>();
            }

            #endregion
        }
    }
}