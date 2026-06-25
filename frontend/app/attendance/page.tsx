'use client';

import { ColumnDef } from "@tanstack/react-table";
import { DataTable, FilterConfig } from "../../components/table/DataTable";
import { useMemo } from "react";
import Link from "next/link";

type AttendanceRecord = {
  id: string;
  date: string;
  clockIn: string;
  clockOut: string;
  status: "present" | "absent" | "late" | "half-day" | "on-leave";
  workingHours: number;
  location: string;
};

const columns: ColumnDef<AttendanceRecord>[] = [
  {
    accessorKey: "date",
    header: "Date",
    cell: ({ getValue }) => {
      const date = new Date(getValue<string>());
      return <span className="tabular-nums">{date.toLocaleDateString()}</span>;
    },
  },
  {
    accessorKey: "clockIn",
    header: "Clock In",
    cell: ({ getValue }) => {
      const time = getValue<string>();
      return <span className="tabular-nums">{time || "—"}</span>;
    },
  },
  {
    accessorKey: "clockOut",
    header: "Clock Out",
    cell: ({ getValue }) => {
      const time = getValue<string>();
      return <span className="tabular-nums">{time || "—"}</span>;
    },
  },
  {
    accessorKey: "workingHours",
    header: "Working Hours",
    cell: ({ getValue }) => {
      const hours = getValue<number>();
      return <span className="tabular-nums">{hours.toFixed(2)}h</span>;
    },
  },
  {
    accessorKey: "status",
    header: "Status",
    cell: ({ getValue }) => {
      const status = getValue<AttendanceRecord["status"]>();
      const colors: Record<AttendanceRecord["status"], string> = {
        present: "bg-green-100 text-green-800",
        absent: "bg-red-100 text-red-800",
        late: "bg-yellow-100 text-yellow-800",
        "half-day": "bg-blue-100 text-blue-800",
        "on-leave": "bg-purple-100 text-purple-800",
      };
      return (
        <span className={`rounded px-2 py-0.5 text-xs font-medium ${colors[status]}`}>
          {status.charAt(0).toUpperCase() + status.slice(1)}
        </span>
      );
    },
  },
  {
    accessorKey: "location",
    header: "Location",
  },
];

function generateMockData(): AttendanceRecord[] {
  const data: AttendanceRecord[] = [];
  const statuses: AttendanceRecord["status"][] = [
    "present",
    "absent",
    "late",
    "half-day",
    "on-leave",
  ];
  const locations = ["Office", "WFH", "Field", "Client Site"];

  for (let i = 0; i < 30; i++) {
    const date = new Date();
    date.setDate(date.getDate() - i);
    const status = statuses[i % statuses.length];

    data.push({
      id: `att-${i}`,
      date: date.toISOString().split("T")[0],
      clockIn:
        status === "absent" ? "—" : `${9 + Math.floor(Math.random() * 2)}:${Math.floor(Math.random() * 60).toString().padStart(2, "0")}`,
      clockOut:
        status === "absent" || status === "on-leave"
          ? "—"
          : `${17 + Math.floor(Math.random() * 2)}:${Math.floor(Math.random() * 60).toString().padStart(2, "0")}`,
      status,
      workingHours:
        status === "absent" ? 0 : status === "half-day" ? 4 : 8 + Math.random() * 2,
      location: locations[i % locations.length],
    });
  }

  return data;
}

export default function AttendancePage() {
  const data = useMemo(() => generateMockData(), []);

  const filters: FilterConfig = [
    { type: "search", placeholder: "Search by location..." },
    {
      type: "checkboxGroup",
      columnId: "status",
      label: "Status",
      options: [
        { label: "Present", value: "present" },
        { label: "Absent", value: "absent" },
        { label: "Late", value: "late" },
        { label: "Half Day", value: "half-day" },
        { label: "On Leave", value: "on-leave" },
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
                Attendance
              </h1>
            </div>
            <button className="rounded-lg bg-teal-600 px-4 py-2 text-white hover:bg-teal-700">
              Clock In
            </button>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <DataTable<AttendanceRecord>
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
