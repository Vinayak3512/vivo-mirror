'use client';

import { ColumnDef } from "@tanstack/react-table";
import { DataTable, FilterConfig } from "../../components/table/DataTable";
import { useMemo } from "react";
import Link from "next/link";

type TeamMember = {
  id: string;
  name: string;
  designation: string;
  department: string;
  email: string;
  status: "active" | "inactive" | "on-leave";
  joinDate: string;
};

const columns: ColumnDef<TeamMember>[] = [
  {
    accessorKey: "name",
    header: "Name",
    cell: ({ getValue }) => {
      const name = getValue<string>();
      return (
        <div className="flex items-center gap-2">
          <div className="h-8 w-8 rounded-full bg-teal-100 flex items-center justify-center">
            <span className="text-xs font-bold text-teal-700">
              {name.split(" ").map((n) => n[0]).join("")}
            </span>
          </div>
          <span className="font-medium">{name}</span>
        </div>
      );
    },
  },
  {
    accessorKey: "designation",
    header: "Designation",
  },
  {
    accessorKey: "department",
    header: "Department",
    cell: ({ getValue }) => {
      const dept = getValue<string>();
      return (
        <span className="rounded-full bg-slate-100 px-3 py-1 text-xs font-medium dark:bg-slate-700">
          {dept}
        </span>
      );
    },
  },
  {
    accessorKey: "email",
    header: "Email",
    cell: ({ getValue }) => {
      const email = getValue<string>();
      return <span className="text-sm text-slate-600 dark:text-slate-400">{email}</span>;
    },
  },
  {
    accessorKey: "joinDate",
    header: "Join Date",
    cell: ({ getValue }) => {
      const date = new Date(getValue<string>());
      return <span className="tabular-nums">{date.toLocaleDateString()}</span>;
    },
  },
  {
    accessorKey: "status",
    header: "Status",
    cell: ({ getValue }) => {
      const status = getValue<TeamMember["status"]>();
      const colors: Record<TeamMember["status"], string> = {
        active: "bg-green-100 text-green-800",
        inactive: "bg-gray-100 text-gray-800",
        "on-leave": "bg-yellow-100 text-yellow-800",
      };
      return (
        <span className={`rounded px-2 py-0.5 text-xs font-medium ${colors[status]}`}>
          {status.charAt(0).toUpperCase() + status.slice(1).replace("-", " ")}
        </span>
      );
    },
  },
];

function generateMockData(): TeamMember[] {
  const data: TeamMember[] = [];
  const designations = [
    "Software Engineer",
    "Product Manager",
    "Designer",
    "Data Analyst",
    "DevOps Engineer",
    "QA Engineer",
  ];
  const departments = ["Engineering", "Product", "Design", "Analytics", "HR"];
  const statuses: TeamMember["status"][] = ["active", "inactive", "on-leave"];

  const names = [
    "Alice Johnson",
    "Bob Smith",
    "Carol Davis",
    "David Wilson",
    "Eve Martinez",
    "Frank Brown",
    "Grace Lee",
    "Henry Taylor",
  ];

  for (let i = 0; i < names.length; i++) {
    const joinDate = new Date();
    joinDate.setFullYear(joinDate.getFullYear() - Math.floor(Math.random() * 5));

    data.push({
      id: `team-${i}`,
      name: names[i],
      designation: designations[i % designations.length],
      department: departments[i % departments.length],
      email: `${names[i].toLowerCase().replace(" ", ".")}@company.com`,
      status: statuses[i % statuses.length],
      joinDate: joinDate.toISOString().split("T")[0],
    });
  }

  return data;
}

export default function TeamPage() {
  const data = useMemo(() => generateMockData(), []);

  const filters: FilterConfig = [
    { type: "search", placeholder: "Search team members..." },
    {
      type: "checkboxGroup",
      columnId: "department",
      label: "Department",
      options: [
        { label: "Engineering", value: "Engineering" },
        { label: "Product", value: "Product" },
        { label: "Design", value: "Design" },
        { label: "Analytics", value: "Analytics" },
        { label: "HR", value: "HR" },
      ],
    },
    {
      type: "checkboxGroup",
      columnId: "status",
      label: "Status",
      options: [
        { label: "Active", value: "active" },
        { label: "Inactive", value: "inactive" },
        { label: "On Leave", value: "on-leave" },
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
                Team Management
              </h1>
            </div>
            <button className="rounded-lg bg-teal-600 px-4 py-2 text-white hover:bg-teal-700">
              Add Team Member
            </button>
          </div>
        </div>
      </header>

      {/* Team Summary */}
      <div className="border-b border-teal-200 bg-white dark:border-slate-700 dark:bg-slate-800">
        <div className="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
          <h2 className="mb-4 text-lg font-semibold text-slate-900 dark:text-white">
            Team Overview
          </h2>
          <div className="grid gap-4 md:grid-cols-4">
            {[
              { label: "Total Members", count: 8, color: "bg-blue-100 text-blue-800" },
              { label: "Active", count: 6, color: "bg-green-100 text-green-800" },
              { label: "On Leave", count: 1, color: "bg-yellow-100 text-yellow-800" },
              { label: "Inactive", count: 1, color: "bg-gray-100 text-gray-800" },
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
        <DataTable<TeamMember>
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
