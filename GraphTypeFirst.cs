using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLPractice
{
    using System;
    using System.Threading.Tasks;
    using GraphQL;
    using GraphQL.Types;
    using GraphQL.SystemTextJson;

    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType()
        {
            Field(x => x.Id).Description("The Id of the Droid.");
            Field(x => x.Name).Description("The name of the Droid.");
        }
    }

    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery()
        {
            Field<DroidType>("hero")
                .Resolve(context => new Droid { Id = "1", Name = "R2-D2" });
            Field<DroidType>("hero1")
                .Resolve(context => new Droid { Id = "2", Name = "R3-D3" });
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var schema = new Schema { Query = new StarWarsQuery() };

            var json = await schema.ExecuteAsync(_ =>
            {
                _.Query = "{ hero1 { id name } }";
            });

            Console.WriteLine(json);
        }
    }
}
