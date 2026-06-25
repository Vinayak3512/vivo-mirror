import { gql } from "@apollo/client";

export const GET_ALL_PAYROLLS = gql`
  query GetAllPayrolls($request: GetAllPayrollsRequestInput!) {
    getAllPayrolls(request: $request) {
      data {
        payrolls {
          id
          employeeId
          month
          year
          basicSalary
          allowances
          deductions
          netSalary
          status
        }
      }
      success
      message
      statusCode
    }
  }
`;
