using nHibernate4.Mapping.Base;
using nHibernate4.Model;

namespace nHibernate4.Mapping
{
    public class GeneratorEnhSeqMap : MapBaseEnhSeq<GeneratorEnhSeq>
    {
        public GeneratorEnhSeqMap()
        {
            Property(x => x.Value);
        }
    }
}