'use client';

import { ColumnDef } from "@tanstack/react-table";
import { DataTable, FilterConfig } from "../../components/table/DataTable";
import { useMemo } from "react";
import Link from "next/link";

type Expense = {
  id: string;
  date: string;
  category: "travel" | "food" | "accommodation" | "communication" | "medical" | "office-supplies" | "other";
  amount: number;
  description: string;
  status: "draft" | "submitted" | "pending-approval" | "approved" | "rejected" | "paid";
  approvedBy: string;
};

const columns: ColumnDef<Expense>[] = [
  {
    accessorKey: "date",
    header: "Date",
    cell: ({ getValue }) => {
      const date = new Date(getValue<string>());
      return <span className="tabular-nums">{date.toLocaleDateString()}</span>;
    },
  },
  {
    accessorKey: "category",
    header: "Category",
    cell: ({ getValue }) => {
      const category = getValue<Expense["category"]>();
      return <span className="capitalize">{category.replace("-", " ")}</span>;
    },
  },
  {
    accessorKey: "description",
    header: "Description",
    cell: ({ getValue }) => {
      const desc = getValue<string>();
      return <span className="truncate">{desc}</span>;
    },
  },
  {
    accessorKey: "amount",
    header: "Amount",
    cell: ({ getValue }) => {
      const amount = getValue<number>();
      return (
        <span className="tabular-nums font-medium">
          ${amount.toLocaleString("en-US", { minimumFractionDigits: 2 })}
        </span>
      );
    },
  },
  {
    accessorKey: "status",
    header: "Status",
    cell: ({ getValue }) => {
      const status = getValue<Expense["status"]>();
      const colors: Record<Expense["status"], string> = {
        draft: "bg-gray-100 text-gray-800",
        submitted: "bg-blue-100 text-blue-800",
        "pending-approval": "bg-yellow-100 text-yellow-800",
        approved: "bg-green-100 text-green-800",
        rejected: "bg-red-100 text-red-800",
        paid: "bg-purple-100 text-purple-800",
      };
      return (
        <span className={`rounded px-2 py-0.5 text-xs font-medium ${colors[status]}`}>
          {status.charAt(0).toUpperCase() + status.slice(1).replace("-", " ")}
        </span>
      );
    },
  },
];

function generateMockData(): Expense[] {
  const data: Expense[] = [];
  const categories: Expense["category"][] = [
    "travel",
    "food",
    "accommodation",
    "communication",
    "medical",
    "office-supplies",
    "other",
  ];
  const statuses: Expense["status"][] = [
    "draft",
    "submitted",
    "pending-approval",
    "approved",
    "rejected",
    "paid",
  ];
  const descriptions = [
    "Flight to NYC",
    "Hotel accommodation",
    "Meal during conference",
    "Mobile recharge",
    "Medical checkup",
    "Office supplies",
    "Taxi fare",
  ];

  for (let i = 0; i < 20; i++) {
    const date = new Date();
    date.setDate(date.getDate() - Math.floor(Math.random() * 30));

    data.push({
      id: `expense-${i}`,
      date: date.toISOString().split("T")[0],
      category: categories[i % categories.length],
      amount: 50 + Math.random() * 500,
      description: descriptions[i % descriptions.length],
      status: statuses[i % statuses.length],
      approvedBy:
        statuses[i % statuses.length] === "pending-approval"
          ? "Pending"
          : "Manager Name",
    });
  }

  return data;
}

export default function ExpensesPage() {
  const data = useMemo(() => generateMockData(), []);

  const filters: FilterConfig = [
    { type: "search", placeholder: "Search expenses..." },
    {
      type: "checkboxGroup",
      columnId: "category",
      label: "Category",
      options: [
        { label: "Travel", value: "travel" },
        { label: "Food", value: "food" },
        { label: "Accommodation", value: "accommodation" },
        { label: "Communication", value: "communication" },
        { label: "Medical", value: "medical" },
        { label: "Office Supplies", value: "office-supplies" },
        { label: "Other", value: "other" },
      ],
    },
    {
      type: "checkboxGroup",
      columnId: "status",
      label: "Status",
      options: [
        { label: "Draft", value: "draft" },
        { label: "Submitted", value: "submitted" },
        { label: "Pending Approval", value: "pending-approval" },
        { label: "Approved", value: "approved" },
        { label: "Rejected", value: "rejected" },
        { label: "Paid", value: "paid" },
      ],
    },
    { type: "dateRange", columnId: "date", label: "Date Range" },
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
                Expenses & Reimbursements
              </h1>
            </div>
            <button className="rounded-lg bg-orange-600 px-4 py-2 text-white hover:bg-orange-700">
              Submit Expense
            </button>
          </div>
        </div>
      </header>

      {/* Expense Summary */}
      <div className="border-b border-teal-200 bg-white dark:border-slate-700 dark:bg-slate-800">
        <div className="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
          <h2 className="mb-4 text-lg font-semibold text-slate-900 dark:text-white">
            Expense Summary
          </h2>
          <div className="grid gap-4 md:grid-cols-4">
            {[
              { label: "Total Submitted", amount: "$2,450", color: "bg-blue-100 text-blue-800" },
              { label: "Pending Approval", amount: "$850", color: "bg-yellow-100 text-yellow-800" },
              { label: "Approved", amount: "$1,200", color: "bg-green-100 text-green-800" },
              { label: "Rejected", amount: "$400", color: "bg-red-100 text-red-800" },
            ].map((item) => (
              <div
                key={item.label}
                className={`rounded-lg p-4 ${item.color}`}
              >
                <p className="text-sm font-medium">{item.label}</p>
                <p className="mt-2 text-2xl font-bold">{item.amount}</p>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Main Content */}
      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <DataTable<Expense>
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
