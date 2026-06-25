import { gql } from "@apollo/client";

export const GET_ALL_EXPENSES = gql`
  query GetAllExpenses($request: GetAllExpensesRequestInput!) {
    getAllExpenses(request: $request) {
      data {
        expenses {
          id
          employeeId
          category
          amount
          currency
          date
          status
          description
        }
      }
      success
      message
      statusCode
    }
  }
`;
