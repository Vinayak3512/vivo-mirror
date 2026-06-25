import { gql } from "@apollo/client";

export const GET_DASHBOARD_SUMMARY = gql`
  query GetDashboardSummary($request: GetAllEmployeesRequestInput!) {
    getAllEmployees(request: $request) {
      data {
        employees {
          id
          status
        }
      }
    }
    getAllLeaves(request: { pageCriteria: { enablePage: true, pageSize: 100, skip: 0 } }) {
      data {
        leaves {
          id
          status
        }
      }
    }
    getAllAnnouncements(request: { pageCriteria: { enablePage: true, pageSize: 5, skip: 0 } }) {
      data {
        announcements {
          id
        }
      }
    }
  }
`;
