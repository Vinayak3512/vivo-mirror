import { gql } from "@apollo/client";

export const GET_ALL_DOCUMENTS = gql`
  query GetAllDocuments($request: GetAllDocumentsRequestInput!) {
    getAllDocuments(request: $request) {
      data {
        documents {
          id
          employeeId
          name
          type
          url
          uploadDate
          status
        }
      }
      success
      message
      statusCode
    }
  }
`;
