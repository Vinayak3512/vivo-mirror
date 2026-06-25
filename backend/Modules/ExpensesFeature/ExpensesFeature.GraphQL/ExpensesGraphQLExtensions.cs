using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesFeature.GraphQL
{
    public static class ExpensesGraphQLExtensions
    {
        public static IRequestExecutorBuilder AddExpensesGraphQL(this IRequestExecutorBuilder builder)
        {
            builder.AddTypeExtension<ExpenseQuery>()
                   .AddTypeExtension<ExpenseMutation>();
            return builder;
        }
    }
}
