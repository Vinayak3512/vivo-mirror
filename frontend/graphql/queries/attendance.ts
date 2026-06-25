import { gql } from "@apollo/client";

export const GET_ALL_ATTENDANCE = gql`
  query GetAllAttendance($request: GetAllAttendanceRequestInput!) {
    getAllAttendance(request: $request) {
      data {
        attendanceRecords {
          id
          employeeId
          date
          clockInTime
          clockOutTime
          status
          totalHours
          overtimeHours
        }
      }
      success
      message
      statusCode
    }
  }
`;
