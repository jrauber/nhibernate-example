using nHibernate4.Mapping.Base;
using nHibernate4.Model;

namespace nHibernate4.Mapping
{
    public class GeneratorEnhancedSequenceMap : MapBaseEnhancedSequence<GeneratorEnhancedSequence>
    {
        public GeneratorEnhancedSequenceMap()
        {
            Property(x => x.Value);
        }
    }
}