import { gql } from "@apollo/client";

export const CREATE_EMPLOYEE = gql`
  mutation CreateEmployee($request: CreateEmployeeRequestInput!) {
    createEmployee(request: $request) {
      success
      message
      statusCode
      data {
        employeeId
      }
    }
  }
`;

export const UPDATE_EMPLOYEE = gql`
  mutation UpdateEmployee($request: UpdateEmployeeRequestInput!) {
    updateEmployee(request: $request) {
      success
      message
      statusCode
      data {
        employeeId
      }
    }
  }
`;

export const DELETE_EMPLOYEE = gql`
  mutation DeleteEmployee($request: DeleteEmployeeRequestInput!) {
    deleteEmployee(request: $request) {
      success
      message
      statusCode
      data {
        employeeId
      }
    }
  }
`;
