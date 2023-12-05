using GraphQL.Types;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.SystemTextJson;

namespace GraphQLPractice
{
    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Character
    {
        public string Name { get; set; }
    }

    public class Query
    {
        [GraphQLMetadata("hero")]
        public Droid GetHero()
        {
            return new Droid { Id = "1", Name = "R2-D2" };
        }
    }

    [GraphQLMetadata("Droid", IsTypeOf = typeof(Droid))]
    public class DroidType
    {
        public string Id([FromSource] Droid droid) => droid.Id;
        public string Name([FromSource] Droid droid) => droid.Name;

        // these two parameters are optional
        // IResolveFieldContext provides contextual information about the field
        public Character Friend(IResolveFieldContext context, [FromSource] Droid source)
        {
            return new Character { Name = "C3-PO" };
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var schema = Schema.For(@"
          type Droid {
            id: String!
            name: String!
            friend: Character
          }

          type Character {
            name: String!
          }

          type Query {
            hero: Droid
          }
        ", _ =>
            {
                _.Types.Include<DroidType>();
                _.Types.Include<Query>();
            });

            var json = await schema.ExecuteAsync(_ =>
            {
                _.Query = "{ hero { id name friend { name } } }";
            });

            Console.WriteLine(json);
        }
    }
}
