import { gql } from "@apollo/client";

export const GET_ALL_LEAVES = gql`
  query GetAllLeaves($request: GetAllLeavesRequestInput!) {
    getAllLeaves(request: $request) {
      data {
        leaves {
          id
          employeeId
          leaveType
          startDate
          endDate
          totalDays
          status
          reason
        }
      }
      success
      message
      statusCode
    }
  }
`;
