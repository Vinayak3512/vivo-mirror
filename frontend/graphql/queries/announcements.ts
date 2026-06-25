import { gql } from "@apollo/client";

export const GET_ALL_ANNOUNCEMENTS = gql`
  query GetAllAnnouncements($request: GetAllAnnouncementsRequestInput!) {
    getAllAnnouncements(request: $request) {
      data {
        announcements {
          id
          title
          content
          postedBy
          postedDate
          priority
          category
        }
      }
      success
      message
      statusCode
    }
  }
`;
