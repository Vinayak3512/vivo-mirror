'use client';

import { ColumnDef } from "@tanstack/react-table";
import { DataTable, FilterConfig } from "../../components/table/DataTable";
import { useMemo } from "react";
import Link from "next/link";

type Document = {
  id: string;
  fileName: string;
  category: "identity" | "employment" | "work-auth" | "tax" | "education" | "other";
  status: "missing" | "uploaded" | "verified" | "rejected";
  uploadedDate: string;
  expiryDate: string;
  rejectionReason: string;
};

const columns: ColumnDef<Document>[] = [
  {
    accessorKey: "fileName",
    header: "Document Name",
    cell: ({ getValue }) => {
      const name = getValue<string>();
      return (
        <div className="flex items-center gap-2">
          <svg
            className="h-4 w-4 text-slate-400"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
            />
          </svg>
          <span>{name}</span>
        </div>
      );
    },
  },
  {
    accessorKey: "category",
    header: "Category",
    cell: ({ getValue }) => {
      const category = getValue<Document["category"]>();
      return <span className="capitalize">{category.replace("-", " ")}</span>;
    },
  },
  {
    accessorKey: "uploadedDate",
    header: "Uploaded",
    cell: ({ getValue }) => {
      const date = getValue<string>();
      return date ? (
        <span className="tabular-nums">{new Date(date).toLocaleDateString()}</span>
      ) : (
        <span className="text-slate-400">—</span>
      );
    },
  },
  {
    accessorKey: "expiryDate",
    header: "Expiry Date",
    cell: ({ getValue }) => {
      const date = getValue<string>();
      if (!date) return <span className="text-slate-400">No expiry</span>;
      const expiryDate = new Date(date);
      const today = new Date();
      const isExpiring = expiryDate < today;
      return (
        <span
          className={isExpiring ? "text-red-600 font-medium" : "tabular-nums"}
        >
          {expiryDate.toLocaleDateString()}
        </span>
      );
    },
  },
  {
    accessorKey: "status",
    header: "Status",
    cell: ({ getValue }) => {
      const status = getValue<Document["status"]>();
      const colors: Record<Document["status"], string> = {
        missing: "bg-red-100 text-red-800",
        uploaded: "bg-yellow-100 text-yellow-800",
        verified: "bg-green-100 text-green-800",
        rejected: "bg-red-100 text-red-800",
      };
      return (
        <span className={`rounded px-2 py-0.5 text-xs font-medium ${colors[status]}`}>
          {status.charAt(0).toUpperCase() + status.slice(1)}
        </span>
      );
    },
  },
];

function generateMockData(): Document[] {
  const data: Document[] = [];
  const categories: Document["category"][] = [
    "identity",
    "employment",
    "work-auth",
    "tax",
    "education",
    "other",
  ];
  const statuses: Document["status"][] = [
    "missing",
    "uploaded",
    "verified",
    "rejected",
  ];
  const fileNames = [
    "Passport.pdf",
    "Aadhar Card.pdf",
    "PAN Card.pdf",
    "Employment Letter.pdf",
    "Visa.pdf",
    "Degree Certificate.pdf",
    "Work Authorization.pdf",
    "Tax Form.pdf",
  ];

  for (let i = 0; i < fileNames.length; i++) {
    const uploadedDate = new Date();
    uploadedDate.setDate(uploadedDate.getDate() - Math.floor(Math.random() * 30));
    const expiryDate = new Date();
    expiryDate.setFullYear(expiryDate.getFullYear() + Math.floor(Math.random() * 5));

    data.push({
      id: `doc-${i}`,
      fileName: fileNames[i],
      category: categories[i % categories.length],
      status: statuses[i % statuses.length],
      uploadedDate: uploadedDate.toISOString().split("T")[0],
      expiryDate: expiryDate.toISOString().split("T")[0],
      rejectionReason:
        statuses[i % statuses.length] === "rejected"
          ? "Document quality too low"
          : "",
    });
  }

  return data;
}

export default function DocumentsPage() {
  const data = useMemo(() => generateMockData(), []);

  const filters: FilterConfig = [
    { type: "search", placeholder: "Search documents..." },
    {
      type: "checkboxGroup",
      columnId: "category",
      label: "Category",
      options: [
        { label: "Identity", value: "identity" },
        { label: "Employment", value: "employment" },
        { label: "Work Authorization", value: "work-auth" },
        { label: "Tax", value: "tax" },
        { label: "Education", value: "education" },
        { label: "Other", value: "other" },
      ],
    },
    {
      type: "checkboxGroup",
      columnId: "status",
      label: "Status",
      options: [
        { label: "Missing", value: "missing" },
        { label: "Uploaded", value: "uploaded" },
        { label: "Verified", value: "verified" },
        { label: "Rejected", value: "rejected" },
      ],
    },
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-teal-50 to-orange-50 dark:from-slate-900 dark:to-slate-800">
      {/* Header */}
      <header className="border-b border-teal-200 bg-white shadow-sm dark:border-slate-700 dark:bg-slate-800">
        <div className="mx-auto max-w-7xl px-4 py-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between">
            <div>
              <Link href="/dashboard" className="text-sm text-teal-600 hover:text-teal-700 dark:text-teal-400">
                ← Back to Dashboard
              </Link>
              <h1 className="mt-2 text-2xl font-bold text-slate-900 dark:text-white">
                Documents
              </h1>
            </div>
            <button className="rounded-lg bg-blue-600 px-4 py-2 text-white hover:bg-blue-700">
              Upload Document
            </button>
          </div>
        </div>
      </header>

      {/* Document Status Summary */}
      <div className="border-b border-teal-200 bg-white dark:border-slate-700 dark:bg-slate-800">
        <div className="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
          <h2 className="mb-4 text-lg font-semibold text-slate-900 dark:text-white">
            Document Status Overview
          </h2>
          <div className="grid gap-4 md:grid-cols-4">
            {[
              { label: "Verified", count: 5, color: "bg-green-100 text-green-800" },
              { label: "Uploaded", count: 2, color: "bg-yellow-100 text-yellow-800" },
              { label: "Pending", count: 0, color: "bg-blue-100 text-blue-800" },
              { label: "Rejected", count: 1, color: "bg-red-100 text-red-800" },
            ].map((item) => (
              <div
                key={item.label}
                className={`rounded-lg p-4 ${item.color}`}
              >
                <p className="text-sm font-medium">{item.label}</p>
                <p className="mt-2 text-2xl font-bold">{item.count}</p>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Main Content */}
      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <DataTable<Document>
          data={data}
          columns={columns}
          pageSizeOptions={[10, 20, 50]}
          initialPageSize={10}
          filters={filters}
          className="rounded-md"
        />
      </main>
    </div>
  );
}
