import { gql } from "@apollo/client";

export const GET_ALL_EMPLOYEES = gql`
  query GetAllEmployees($request: GetAllEmployeesRequestInput!) {
    getAllEmployees(request: $request) {
      data {
        employees {
          id
          firstName
          lastName
          email
          phone
          designation
          department
          dateOfJoining
          role
          status
        }
      }
      success
      message
      statusCode
    }
  }
`;
