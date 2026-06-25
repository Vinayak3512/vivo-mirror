using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TeamFeature.GraphQL
{
    public static class TeamGraphQLExtensions
    {
        public static IRequestExecutorBuilder AddTeamGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<TeamQuery>()
                   .AddTypeExtension<TeamMutation>();
            return builder;
        }
    }
}
