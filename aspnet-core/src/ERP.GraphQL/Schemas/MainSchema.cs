using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using ERP.Queries.Container;

namespace ERP.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}